using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace RomItemDataEditor
{
    class RomReader : RomEditor
    {
        public RomReader(string rompath, string xmlparserpath)
        {
            this.rompath = rompath;
            this.xmlparserpath = xmlparserpath;
            this.rompaths = rompath;
        }

        ~RomReader()
        {
            CloseRomReader();
            CloseRomWriter();
            CloseXMLParser();
        }

        public override bool OpenRomReader()
        {
            if (!File.Exists(rompath))
                return false;
            else
            {
                binaryreader = new BinaryReader(File.OpenRead(rompath));
                return true;
            }
        }

        public override bool OpenRomWriter()
        {
            if (!File.Exists(rompath))
                return false;
            else
            {
                binaryreader = new BinaryReader(File.OpenRead(rompath));
                return true;
            }
        }

        public override bool OpenXMLParser()
        {
            if (!File.Exists(xmlparserpath))
                return false;
            else
            {
                xmlparser = new XMLParser(xmlparserpath);
                xmlparser.Open();
                return true;
            }
        }

        public string GetItemNameByIndex(int i)
        {
            

            string ret = string.Empty;


            OpenXMLParser();
            string gamecode = GetGameCode();

            OpenRomReader();
            

            

            long globaloffset = xmlparser.GetGlobalItemOffsetByGameCode(gamecode);
            int offset = xmlparser.GetItemOffsetByName("name");
            int size = xmlparser.GetItemDataSizeByName("name");

            long pos = globaloffset + offset + (0x2C * i) - 0x8000000;

            binaryreader.BaseStream.Seek(pos, SeekOrigin.Begin);
            ret = BytesToString(binaryreader.ReadBytes(size));

            CloseXMLParser();
            CloseRomReader();
            
       
            return ret; 

        }

        public int GetItemStructValue(int i, string structname)
        {
            OpenXMLParser();
            

            string gamecode = GetGameCode();

            OpenRomReader();

            long globaloffset = xmlparser.GetGlobalItemOffsetByGameCode(gamecode);
            int offset = xmlparser.GetItemOffsetByName(structname);
            int size = xmlparser.GetItemDataSizeByName(structname);
            long pos = globaloffset + offset + (i * 0x2C) - 0x8000000;


            binaryreader.BaseStream.Position = pos;
            
            byte[] bytes = binaryreader.ReadBytes(size);

            int ret = 0;

            if (size == 1)
                ret = bytes[0];
            if (size == 2)
                ret = BitConverter.ToInt16(bytes, 0);
            if (size == 4)
                ret = BitConverter.ToInt32(bytes, 0);

            CloseRomReader();
            CloseXMLParser();

            return ret;
        }

        public string GetGameName()
        {
            string ret = GetGameCode();

            if (ret == string.Empty)
            {
                log.PrintError("Couldn't recieve the gamecode");
                return string.Empty;
            }
            return xmlparser.GetNameByGameCode(GetGameCode());
        }

        private string BytesToString(byte[] bytes)
        {
            StringBuilder str = new StringBuilder();

            foreach(byte i in bytes)
            {
                str.Append(xmlparser.GetAsciiByHex(i));
            }

            return str.ToString();
        }

        public override bool CloseXMLParser()
        {
            if (xmlparser == null)
                return true;
            xmlparser.Close();
            xmlparser = null;
            return true;
        }

        public override bool CloseRomReader()
        {
            if (binaryreader == null)
                return true;
            binaryreader.Close();
            binaryreader = null;
            return true;
        }

        public override bool CloseRomWriter()
        {
            if (binarywriter == null)
                return true;
            binarywriter.Close();
            binarywriter = null;
            return true;
        }
    }
}
