

using System;

namespace RomItemDataEditor
{
    class Logger
    {
        public void PrintHexValue(int val, string name)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{1}: 0x{0:X}", val, name);
            Console.ResetColor();
        }
        public void PrintDecValue(int val, string name)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{1}: {0}", val, name);
            Console.ResetColor();
        }

        public void PrintStrValue(string val, string name)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{1}: {0}", val, name);
            Console.ResetColor();
        }

        public void PrintError(string err)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(err);
            Console.ResetColor();
        }
        public void PrintWarning(string warn)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(warn);
            Console.ResetColor();
        }
    }
}
