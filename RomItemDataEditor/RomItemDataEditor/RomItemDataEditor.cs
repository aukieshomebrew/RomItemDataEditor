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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine( opt.GetUsage());
                Console.ResetColor();
                return 1;
            }
            else
            {

                if (!File.Exists(opt.ArgRomPath))
                {
                    po.PrintError("Cannot find rom file");

                    return 1;
                }
                else if (!File.Exists(opt.ArgDataPath))
                {
                    po.PrintError("Cannot find data file");
                    return 1;
                }

                if(string.IsNullOrWhiteSpace(opt.ArgSetName))
                {
                    if(opt.ArgGetName)
                    {
                        RomReader romreader = new RomReader(opt.ArgRomPath, opt.ArgDataPath);
                        string name = romreader.GetItemNameByIndex(opt.ArgIndex);
                        po.PrintStrValue(name, "name");
                        
                    }
                    else
                    {
                        po.PrintError("Please choose either a set or a get.");
                        return 1;
                    }
                    
                }
                else
                {
                    if(!opt.ArgGetName)
                    {

                        RomReader romreader1 = new RomReader(opt.ArgRomPath, opt.ArgDataPath);
                        string name1 = romreader1.GetItemNameByIndex(opt.ArgIndex);
                        po.PrintStrValue(name1, "oldname");
                        romreader1 = null;
                        

                        RomWriter romwriter = new RomWriter(opt.ArgRomPath, opt.ArgDataPath);
                        romwriter.SetItemNameByIndex(opt.ArgIndex, opt.ArgSetName);
                        romwriter = null;

                        RomReader romreader2 = new RomReader(opt.ArgRomPath, opt.ArgDataPath);
                        string name2 = romreader2.GetItemNameByIndex(opt.ArgIndex);
                        po.PrintStrValue(name2, "newname");
                        romreader2 = null;
                        
                    }
                    else
                    {
                        po.PrintError("Please choose either a set or a get.");
                        return 1;
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

        public void PrintError(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(err);
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

        [Option('v', "get-value", Required = false, HelpText = "Struct name to get value from.")]
        public string ArgStructName { get; set; }

        [Option("print-hex", Required = false, HelpText = "Print hexidecimal value.")]
        public bool ArgHexPrint { get; set; }

        [Option('n', "get-name", Required = false, HelpText = "Get item name.")]
        public bool ArgGetName { get; set; }

        [Option("set-name", Required = false, HelpText = "Set item name.")]
        public string ArgSetName { get; set; }

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
            help.AddPreOptionsLine("Usage: romitemdataeditor --rom-file <*.gba file> [--data-file <*.xml file>] [--index-number <index number>] [--get-value <datamember name>] [--get-name] [--set-name <new name>][--print-hex]");
            help.AddOptions(this);
            return help;
        }

    }
}
