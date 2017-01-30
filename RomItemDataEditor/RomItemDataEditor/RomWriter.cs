

using System;
using System.Collections.Generic;
using System.IO;

namespace RomItemDataEditor
{
    class RomWriter : RomEditor
    {

        public RomWriter(string rompath, string xmlparserpath)
        {
            this.rompath = rompath;
            this.xmlparserpath = xmlparserpath;
            rompaths = rompath;
        }

        ~RomWriter()
        {
            CloseXMLParser();
            CloseRomWriter();
            CloseRomReader();
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
                binarywriter = new BinaryWriter(File.OpenWrite(rompath));
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

        public bool SetItemNameByIndex(int i, string newname)
        {
           
            string  gamecode = GetGameCode();
     
            OpenXMLParser();

            long globaloffset = xmlparser.GetGlobalItemOffsetByGameCode(gamecode);
            int offset = xmlparser.GetItemOffsetByName("name");
            int size = xmlparser.GetItemDataSizeByName("name");

            long pos = globaloffset + offset + (i * 0x2C) - 0x8000000;
            List<byte> array = new List<byte>();
            List<byte> empty = new List<byte>();

            for (int t = 0; t < size; t++)
            {
                empty.Add(0x00);
            }
            for (int m = 0; m < newname.Length; m++)
            {
                array.Add(xmlparser.GetHexByAscii(newname[m]));
            }

            OpenRomWriter();
            binarywriter.BaseStream.Position = pos;
            binarywriter.Write(empty.ToArray(), 0, empty.ToArray().Length);

            binarywriter.BaseStream.Position = pos;
            binarywriter.Write(array.ToArray(), 0, array.ToArray().Length);

            if (xmlparser == null)
                return true;
            else
                CloseXMLParser();

            if (binarywriter == null)
                return true;
            else
                CloseRomWriter();


            return true;

        }

        public bool SetItemStructValue(int i, string valuename, int newvalue)
        {
            string gamecode = GetGameCode();

            OpenXMLParser();
            OpenRomWriter();

            long globaloffset = xmlparser.GetGlobalItemOffsetByGameCode(gamecode);
            int offset = xmlparser.GetItemOffsetByName(valuename);
            int size = xmlparser.GetItemDataSizeByName(valuename);

            long pos = globaloffset + offset + (0x2C * i) - 0x8000000;

            byte[] val = BitConverter.GetBytes(newvalue);

            if(size == 1)
            {
                binarywriter.BaseStream.Position = pos;
                binarywriter.Write(val[0]);
            }
            else
            {
                binarywriter.BaseStream.Position = pos;
                binarywriter.Write(val, 0, size);
            }

            if (xmlparser == null)
                return true;
            else
                CloseXMLParser();

            if (binarywriter == null)
                return true;
            else
                CloseRomWriter();


            return true;

        }

    }
}
