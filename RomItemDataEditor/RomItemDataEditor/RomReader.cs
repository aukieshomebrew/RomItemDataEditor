/*
 *  RomReader.cs    Made by: Aukie's Homebrew 
 *  This class accesses the ROM file and extract data from it.
 */


using System;
using System.IO;
using System.Text;

namespace RomItemDataEditor
{


    class RomReader
    {
        private BinaryReader br;
        private XMLParser xp;

        string rp;
        string pp;
        public RomReader(string rompath, string parserpath)
        {
            rp = rompath;
            
            this.xp = new XMLParser(parserpath);
            this.xp.Open();
            this.pp = parserpath;
        }

        public string GetItemNameByIndex(int i)
        {
            
            string gamecode = GetGameCode();            
            long globaloffset = xp.GetGlobalItemOffsetByGameCode(gamecode);
            int offset = xp.GetItemOffsetByName("name");
            int size = xp.GetItemDataSizeByName("name");
            long pos = globaloffset + offset + (i * 0x2C) - 0x8000000;

           // Console.WriteLine("Game: {4}\nMemory address: 0x{0:X}:\nBase Offset: 0x{1:X}\nStruct Offset: 0x{2:X}\nSize: 0x{3:X}", pos, globaloffset, offset, size, xp.GetNameByGameCode(gamecode));

            br = new BinaryReader(File.Open(rp, FileMode.Open));
            br.BaseStream.Seek(pos, SeekOrigin.Begin);


            string ret = BytesToAscii(br.ReadBytes(size));

            if (string.IsNullOrWhiteSpace(ret))
            {
                Console.WriteLine("Only whitespaces detected");
                br.Close();
                br = null;
                return string.Empty;
            }

            br.Close();
            br = null;
            return ret;
            
        }

        public int GetItemStructValueByIndex(int i, string structname)
        {
            string gamecode = GetGameCode();
            long globaloffset = xp.GetGlobalItemOffsetByGameCode(gamecode);
            int offset = xp.GetItemOffsetByName(structname);
            int size = xp.GetItemDataSizeByName(structname);
            long pos = globaloffset + offset + (i * 0x2C) - 0x8000000;

            br = new BinaryReader(File.Open(rp, FileMode.Open));
            br.BaseStream.Seek(pos, SeekOrigin.Begin);

            byte[] bytes = br.ReadBytes(size);
            StringBuilder strbuilder = new StringBuilder();

            foreach(byte j in bytes)
            {
                strbuilder.Append(j);
            }
            //return Int32.Parse(strbuilder.ToString());
            //if (BitConverter.IsLittleEndian)
                //Array.Reverse(bytes);

            int ret = 0 ;
            if (size == 1)
                ret = bytes[0];
            if (size == 2)
                ret = BitConverter.ToInt16(bytes, 0);
            if (size == 4)
                ret = BitConverter.ToInt32(bytes, 0);


            return ret;
        }

        private string GetGameCode()
        {
            br = new BinaryReader(File.Open(rp, FileMode.Open));

            string ret;
            br.BaseStream.Seek(0xAC, SeekOrigin.Begin);
            

            ret = Encoding.UTF8.GetString(br.ReadBytes(4));
            br.Close();
            br = null;
            return ret;
        }

        public string GetGameName()
        {
            return xp.GetNameByGameCode(GetGameCode());
        }



        private string BytesToAscii(byte[] bytes)
        {
            StringBuilder stringbuilder = new StringBuilder();
            foreach (byte character in bytes)
            {
                
                stringbuilder.Append(xp.GetAsciiByHex(character));
            }

            return stringbuilder.ToString();
        }

    }
}
