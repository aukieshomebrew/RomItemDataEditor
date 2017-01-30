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

        Logger log = new Logger();

        static int Main(string[] args)
        {
            Options opt = new Options();
            Parser par = new Parser();
            RomItemDataEditor main = new RomItemDataEditor();
            Logger log = new Logger();
            if(!par.ParseArguments(args, opt))
            {

                log.PrintError("Error parsing arguments.");
                Console.Write(opt.GetUsage());
                return 0;
            }


            if (main.CheckModeGet(opt))
            {
                main.GetMode(opt);
                return 0;
            }

            else if (main.CheckModeSet(opt))
            {
                main.SetMode(opt);
                return 0;
            }
            else
            {
                log.PrintError("Choose either a get or a set.");
                return 0;
            }
        }

        public bool CheckModeSet(Options opt)
        {
            bool ret = false;
            if(!string.IsNullOrWhiteSpace(opt.ArgSetName))
            {
                ret = true;
            }
            else if(!string.IsNullOrWhiteSpace(opt.ArgSetValueName))
            {
                ret = true;
            }
            else
            {
                ret = false;
            }

            return ret;
        }

        public bool CheckModeGet(Options opt)

        {
            bool ret = false;
            if(opt.ArgGetName)
            {
                ret = true;
             
            }
            else if(!string.IsNullOrEmpty(opt.ArgGetValue))
            {
                ret = true;
            }
            else
            {
                
            }
            return ret;
        }



        public bool GetMode(Options opt)
        {
            if (opt.ArgGetName)
            {
                RomReader romreader = new RomReader(opt.ArgRomPath, opt.ArgDataPath);
                log.PrintStrValue(romreader.GetItemNameByIndex(opt.ArgIndex), "Name");
                return true;
            }

            else if (!string.IsNullOrWhiteSpace(opt.ArgGetValue))
            {
                RomReader romreader = new RomReader(opt.ArgRomPath, opt.ArgDataPath);

                if(opt.ArgHexPrint)
                {
                    log.PrintHexValue(romreader.GetItemStructValue(opt.ArgIndex, opt.ArgGetValue), opt.ArgGetValue);
                }
                else
                {
                    log.PrintDecValue(romreader.GetItemStructValue(opt.ArgIndex, opt.ArgGetValue), opt.ArgGetValue);
                }
                
                return true;
            }
            else
            {
                return false;
            }


        }

        public bool SetMode(Options opt)
        {
            if (!string.IsNullOrWhiteSpace(opt.ArgSetName))
            {
                RomReader romreader = new RomReader(opt.ArgRomPath, opt.ArgDataPath);
                log.PrintStrValue(romreader.GetItemNameByIndex(opt.ArgIndex), "Name before");

                RomWriter romwriter = new RomWriter(opt.ArgRomPath, opt.ArgDataPath);
                romwriter.SetItemNameByIndex(opt.ArgIndex, opt.ArgSetName);


                log.PrintStrValue(romreader.GetItemNameByIndex(opt.ArgIndex), "Name after");

                return true;


            }
            else if (!string.IsNullOrWhiteSpace(opt.ArgSetValueName))
            {
                RomReader romreader = new RomReader(opt.ArgRomPath, opt.ArgDataPath);

                if(opt.ArgHexPrint)
                {
                    log.PrintHexValue(romreader.GetItemStructValue(opt.ArgIndex, opt.ArgSetValueName), opt.ArgSetValueName + " before");
                }
                else
                {
                    log.PrintDecValue(romreader.GetItemStructValue(opt.ArgIndex, opt.ArgSetValueName), opt.ArgSetValueName + " before");
                }
                

                RomWriter romwriter = new RomWriter(opt.ArgRomPath, opt.ArgDataPath);
                romwriter.SetItemStructValue(opt.ArgIndex, opt.ArgSetValueName, opt.ArgSetValueInt);

                if (opt.ArgHexPrint)
                {
                    log.PrintHexValue(romreader.GetItemStructValue(opt.ArgIndex, opt.ArgSetValueName), opt.ArgSetValueName + " after");
                }
                else
                {
                    log.PrintDecValue(romreader.GetItemStructValue(opt.ArgIndex, opt.ArgSetValueName), opt.ArgSetValueName + " after");
                }
                return true;

            }
            else
            {
                return false;
            }
        }



    }
    
    class Options
    {
        [Option('f', "rom-file", Required = true, HelpText = "Path of the GBA ROM.")]
        public string ArgRomPath { get; set; }

        [Option('x', "xml-file", Required = false, HelpText = "Path of the XML file.", DefaultValue ="data.xml")]
        public string ArgDataPath { get; set; }

        [Option('i', "index-number", Required = true, HelpText = "Index of the item.")]
        public int ArgIndex { get; set; }

        [Option('v', "get-value", Required = false, HelpText = "Struct name to get value from.")]
        public string ArgGetValue { get; set; }

        [Option("print-hex", Required = false, HelpText = "Print hexidecimal value.")]
        public bool ArgHexPrint { get; set; }

        [Option('n', "get-name", Required = false, HelpText = "Get item name.")]
        public bool ArgGetName { get; set; }

        [Option("set-name", Required = false, HelpText = "Set item name.")]
        public string ArgSetName { get; set; }

        [Option("set-value", Required =false, HelpText = "Struct name to set value to.")]
        public string ArgSetValueName { get; set; }

        [Option("set-value-int", Required = false, HelpText="Struct value to set value to", DefaultValue =0)]
        public int ArgSetValueInt { get; set; }
        

        [HelpOption]
        public string GetUsage()
        {
            HelpText help = new HelpText
            {
                Heading = new HeadingInfo("romitemdataeditor", "0.2"),
                Copyright = new CopyrightInfo("Aukie's Homebrew", 2017),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true

            };
            help.AddPreOptionsLine("Usage: romitemdataeditor --rom-file <*.gba file> [--data-file <*.xml file>] [--index-number <index number>] [--get-value <datamember name>] [--get-name] [--set-name <new name>] [--set-value <value-name>] [--set-value-int <value-integer>] [--print-hex]");
            help.AddOptions(this);
            return help;
        }

    }
}
