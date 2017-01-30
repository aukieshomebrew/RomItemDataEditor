

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
        protected Logger log = new Logger();

        protected string rompaths;

        abstract public bool OpenRomReader();
        abstract public bool OpenRomWriter();
        abstract public bool OpenXMLParser();
        abstract public bool CloseXMLParser();
        abstract public bool CloseRomReader();
        abstract public bool CloseRomWriter();

        public string GetGameCode()
        {

            if(!OpenRomReader())
            {
                log.PrintError("Couldn't open ROM image");   
                return string.Empty;
            }

            string ret;
            binaryreader.BaseStream.Seek(0xAC, SeekOrigin.Begin);

            ret = Encoding.UTF8.GetString(binaryreader.ReadBytes(4));


            CloseRomReader();
            return ret;
            
            
        }

    }
}
