/*
 *      RomItemDataEditor.cs    Made by Aukie's Homebrew.
 *      Main class with the main function and cmd parsing abilities.
 */



using System;
using System.IO;
using CommandLine;
using CommandLine.Text;

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
               Console.WriteLine( opt.GetUsage());
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

                

                po.PrintStrValue(rr.GetGameName(), "Game");
                
                if(opt.ArgGetName)
                {
                    po.PrintStrValue(rr.GetItemNameByIndex(opt.ArgIndex), "Name");
                }
                else
                {
                    
                    if (opt.ArgHexPrint)
                    {
                        po.PrintHexValue(rr.GetItemStructValueByIndex(opt.ArgIndex, opt.ArgStructName), opt.ArgStructName);
                    }
                    else
                    {
                        po.PrintDecValue(rr.GetItemStructValueByIndex(opt.ArgIndex, opt.ArgStructName), opt.ArgStructName);
                    }
                }



                return 0;

            }


        }


        public void PrintHexValue(int val, string name)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("{1}: 0x{0:X}", val, name);
            Console.ResetColor();
        }
        public void PrintDecValue(int val, string name)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("{1}: {0}", val, name);
            Console.ResetColor();
        }

        public void PrintStrValue(string val, string name)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("{1}: {0}", val, name);
            Console.ResetColor();
        }



    }
    
    class Options
    {
        [Option('f', "rom-file", Required = true, HelpText = "Path of the GBA ROM.")]
        public string ArgRomPath { get; set; }

        [Option('x', "xml-file", Required = false, HelpText = "Path of the XML file.", DefaultValue ="data.xml")]
        public string ArgDataPath { get; set; }

        [Option('i', "index-number", Required = true, HelpText = "Index of the item.", DefaultValue = 0)]
        public int ArgIndex { get; set; }

        [Option('d', "data-name", Required = false, HelpText = "Struct name to get value from.")]
        public string ArgStructName { get; set; }

        [Option("print-hex", Required = false, HelpText = "Print hexidecimal value.")]
        public bool ArgHexPrint { get; set; }

        [Option('n', "get-name", Required = false, HelpText = "Get item name.")]
        public bool ArgGetName { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            HelpText help = new HelpText
            {
                Heading = new HeadingInfo("romitemdataeditor", "0.1"),
                Copyright = new CopyrightInfo("Aukie's Homebrew", 2017),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true

            };
            help.AddPreOptionsLine("Usage: romitemdataeditor --rom-file <*.gba file> [--data-file <*.xml file>] [--index-number <index number>] [--data-name <datamember name>] [--print-hex]");
            help.AddOptions(this);
            return help;
        }

    }
}
