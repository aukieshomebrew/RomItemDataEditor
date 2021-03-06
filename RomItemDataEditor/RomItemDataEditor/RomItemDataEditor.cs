﻿/*
 *  Created by Aukie's Homebrew
 */

using System;
using System.IO;
using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

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

            if(!File.Exists(opt.ArgRomPath))
            {
                log.PrintError("Error: ROM not found!");
                return main.ExitFailed();
            }
            if (!File.Exists(opt.ArgDataPath))
            {
                log.PrintError("Error: XML file not found!");
                return main.ExitFailed();
            }


            if (main.CheckModeGet(opt))
            {
                if(!main.GetMode(opt))
                {
                    return main.ExitFailed();
                }
                else
                {
                    return main.ExitSucceed();
                }
                
            }

            else if (main.CheckModeSet(opt))
            {
                main.SetMode(opt);
                return main.ExitSucceed();
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

            RomReader rr = new RomReader(opt.ArgRomPath, opt.ArgDataPath);
            
            
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
                if (!romwriter.SetItemNameByIndex(opt.ArgIndex, opt.ArgSetName))
                {
                    log.PrintError("New value too long!");
                    return false;
                }


                log.PrintStrValue(romreader.GetItemNameByIndex(opt.ArgIndex), "Name after");

                return true;


            }
            else if (!string.IsNullOrWhiteSpace(opt.ArgSetValueName))
            {
                RomReader romreader = new RomReader(opt.ArgRomPath, opt.ArgDataPath);
                if (opt.ArgSetValueInt == 0)
                {
                    log.PrintWarning("'--set-value-int' not set: default value is 0");
                }
                if (opt.ArgHexPrint)
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

        public int ExitFailed()

        {
            log.PrintError("Program failed");
            return 0;
        }

        public int ExitSucceed()

        {
            log.PrintSuccess("Program succeed");
            return 0;
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

        [Option("print-hex", Required = false, HelpText = "Print hexidecimal value.")]
        public bool ArgHexPrint { get; set; }

        [Option('n', "get-name",MutuallyExclusiveSet = "get-name", Required = false, HelpText = "Get item name.")]
        public bool ArgGetName { get; set; }

        [Option('v', "get-value", Required = false, MutuallyExclusiveSet = "get-value", HelpText = "Struct name to get value from.")]
        public string ArgGetValue { get; set; }

        [Option("set-name", Required = false, MutuallyExclusiveSet = "set-name", HelpText = "Set item name.")]
        public string ArgSetName { get; set; }

        [Option("set-value", Required =false, MutuallyExclusiveSet = "set-value", HelpText = "Struct name to set value to.")]
        public string ArgSetValueName { get; set; }
        
        [Option("set-value-int", Required =false, MutuallyExclusiveSet ="set-value", HelpText = "New value for the --set-value option" )]
        public int ArgSetValueInt { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            HelpText help = new HelpText
            {
                Heading = new HeadingInfo("RomItemDataEditor", "0.2.2"),
                Copyright = new CopyrightInfo("Aukie's Homebrew", 2017),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("Usage: romitemdataeditor --rom-file <*.gba file> [--data-file <*.xml file>] [--index-number <index number>] [--get-value <datamember name>] [--get-name] [--set-name <new name>] [--set-value <value-name> <integer>] [--print-hex]");
            help.AddOptions(this);
            return help;
        }

    }
}
