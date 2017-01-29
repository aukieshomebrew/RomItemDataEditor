using System;
using System.IO;
using CommandLine;

namespace RomItemDataEditor
{
    class RomItemDataEditor
    {

       

        static int Main(string[] args)
        {


            RomItemDataEditor po = new RomItemDataEditor();
            Options opt = new Options();
            Parser cmd = new Parser();



            if (!cmd.ParseArguments(args, opt))
            {
                Console.WriteLine("Couldn't parse arguments.");
                return 1;
            }
            else
            {

                if (!File.Exists(opt.ArgRomPath))
                {
                    Console.WriteLine("Cannot find rom file");

                    return 1;
                }
                else if (!File.Exists(opt.ArgDataPath))
                {
                    Console.WriteLine("Cannot find data file");
                    return 1;
                }

                

                RomReader rr = new RomReader(opt.ArgRomPath, opt.ArgDataPath);

                Console.WriteLine("Game: {0}", rr.GetGameName());
                
                if(opt.ArgStructName == "")
                {
                    Console.WriteLine("{1}> {0}", rr.GetItemNameByIndex(opt.ArgIndex), "name");

                }
                else
                {
                    
                    if (opt.ArgHexPrint)
                    {
                        Console.WriteLine("{1}> 0x{0:X}", rr.GetItemStructValueByIndex(opt.ArgIndex, opt.ArgStructName), opt.ArgStructName);
                    }
                    else
                    {
                        Console.WriteLine("{1}> {0}", rr.GetItemStructValueByIndex(opt.ArgIndex, opt.ArgStructName), opt.ArgStructName);
                    }
                }



                return 0;

            }

           
         
        }



    }
    
    class Options
    {
        [Option('f', "rom-file", Required = true, HelpText = "Path of the GBA ROM.")]
        public string ArgRomPath { get; set; }

        [Option('m', "xml-file", Required = false, HelpText = "Path of the XML file.", DefaultValue ="data.xml")]
        public string ArgDataPath { get; set; }

        [Option('i', "index-number", Required = false, HelpText = "Index of the item.", DefaultValue = 0)]
        public int ArgIndex { get; set; }

        [Option('n', "data-name", Required = false, HelpText = "Struct name to get value.", DefaultValue = "")]
        public string ArgStructName { get; set; }

        [Option('x', "print-hex", Required = false, HelpText = "Print hexidecimal value.")]
        public bool ArgHexPrint { get; set; }

    }
}
