using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RomItemDataEditor
{
    class RomWriter
    {

        private BinaryWriter binarywriter;
        private BinaryReader binaryreader;
        private XMLParser xmlparser;

       

        private string rompath;
        //private string parserpath;

        public RomWriter(string rompath, string parserpath)
        {
            this.rompath = rompath;
            this.xmlparser = new XMLParser(parserpath);
  
        }


        public bool SetItemNameByIndex(int i, string itemname)
        {
 
            xmlparser.Open();
            string gamecode = GetGameCode();
            long globaloffset = xmlparser.GetGlobalItemOffsetByGameCode(gamecode);
            int offset = xmlparser.GetItemOffsetByName("name");
            int size = xmlparser.GetItemDataSizeByName("name");

            long pos = globaloffset + offset + (i * 0x2C) - 0x8000000;

            List<byte> array = new List<byte>();

            StringBuilder str = new StringBuilder();
            for (int m = 0; m < size; m++)
            {
                if (m + 1 > itemname.Length)
                {
                    array.Add(0x00);
                    str.Append(0x00);
                }
                    
                else
                {
                    array.Add(xmlparser.GetHexByAscii(itemname[m]));
                    str.Append(xmlparser.GetHexByAscii(itemname[m]));
                }
                    
            }
            

            binarywriter = new BinaryWriter(File.Open(rompath, FileMode.Open));

            binarywriter.BaseStream.Seek(pos, SeekOrigin.Begin);
            
           
            Console.WriteLine(str.ToString());
            binarywriter.Write(array.ToArray());


            xmlparser.Close();
            binarywriter.Close();
            binarywriter = null;

            return true;
        }

        public string GetGameCode()
        {

            binaryreader = new BinaryReader(File.Open(rompath, FileMode.Open));

            string ret;
            binaryreader.BaseStream.Seek(0xAC, SeekOrigin.Begin);


            ret = Encoding.UTF8.GetString(binaryreader.ReadBytes(4));


            binaryreader.Close();
            binaryreader = null;
            return ret;
        }
    }
}
