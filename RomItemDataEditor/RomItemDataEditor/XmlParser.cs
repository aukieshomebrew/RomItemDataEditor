using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Xml.Linq;

namespace RomItemDataEditor
{
    class XMLParser
    {
        private XDocument data;
        private string path;
        private XElement root;
        private XElement core;
        private XElement items;

        public XMLParser(string path)
        {
            this.path = path;
        }

        public bool Open()
        {
            if(path == string.Empty)
            {
                return false;
            }
            else
            {
                if (!File.Exists(path))
                {
                    return false;
                }
                else
                {
                    data = XDocument.Load(path);
                    root = data.Element("data");
                    core = root.Element("core");
                    items = root.Element("items");
                    return true;
                }
            }

        ;

        }

        public string GetNameByGameCode(string gamecode)
        {
            IEnumerable<XElement> games = core.Descendants("game");
            string ret = string.Empty;
            foreach (XElement game in games)
            {

                if (game.Attribute("gamecode").Value == gamecode)
                {
                    ret = game.Attribute("name").Value;
                }
                else
                {
                    continue;
                }
            }

            return ret;
        }

        public long GetGlobalItemOffsetByGameCode(string gamecode)
        {
            IEnumerable<XElement> offsets = items.Descendants("offset");
            long ret = 0;

            foreach (XElement offset in offsets)
            {
                if (offset.Attribute("name").Value == "base")
                {
                    if (offset.Attribute("gamecode").Value == gamecode)
                    {
                        if(!Int64.TryParse(offset.Attribute("offset").Value.Substring(2), NumberStyles.HexNumber, null, out ret))
                        {
                            return -1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
            return ret;
        }

        public int GetItemOffsetByName(string name)
        {
            IEnumerable<XElement> offsets = items.Descendants("offset");
            int ret = 0;

            foreach (XElement e in offsets)
            {
                if(name == e.Attribute("name").Value)
                {
                    if(!Int32.TryParse(e.Attribute("offset").Value.Substring(2), NumberStyles.HexNumber, null, out ret))
                    {
                        return -1;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    continue;
                }
            }

            return ret;
        }

        public int GetItemDataSizeByName(string name)
        {
            IEnumerable<XElement> offsets = items.Descendants("offset");
            int ret = 0;

            foreach (XElement e in offsets)
            {
                if (name == e.Attribute("name").Value)
                {
                    if (!Int32.TryParse(e.Attribute("size").Value.Substring(2), NumberStyles.HexNumber, null, out ret))
                    {
                        return -1;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    continue;
                }
            }

            return ret;
        }

        public char GetAsciiByHex(byte hex)
        {
            IEnumerable<XElement> table = root.Element("encoding-table").Descendants("encoding");
            char ret = ' ';

            foreach(XElement chararcter in table)
            {
                if (Byte.Parse(chararcter.Attribute("hex").Value.Substring(2), NumberStyles.HexNumber) == hex)
                {
                    ret = chararcter.Attribute("ascii").Value[0];
                    break;
                }
                else
                {
                    ret = ' ';
                    continue;
                }
            }

            return ret;
        }


    }
}
