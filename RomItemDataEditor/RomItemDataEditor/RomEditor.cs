

using System.IO;
using System.Text;

namespace RomItemDataEditor
{
    abstract class RomEditor
    {
        protected string rompath;
        protected string xmlparserpath;

        protected BinaryReader binaryreader;
        protected BinaryWriter binarywriter;
        protected XMLParser xmlparser;

        protected string rompaths;

        abstract public bool OpenRomReader();
        abstract public bool OpenRomWriter();
        abstract public bool OpenXMLParser();
        abstract public bool CloseXMLParser();
        abstract public bool CloseRomReader();
        abstract public bool CloseRomWriter();

        protected string GetGameCode()
        {

            OpenRomReader();

            string ret;
            binaryreader.BaseStream.Seek(0xAC, SeekOrigin.Begin);

            ret = Encoding.UTF8.GetString(binaryreader.ReadBytes(4));

            if (binaryreader == null)
            {
                
                return ret;
            }
                
            else
            {
                CloseRomReader();
                return ret;
            }
            
        }

    }
}
