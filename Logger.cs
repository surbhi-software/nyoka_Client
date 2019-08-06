using nyoka_Client;
using System.Collections.Generic;
using System.Linq;

public class Logger
{ 
    public static void logWarning(string message)
        {
            internalWriteLine($"WARNING: {message}",Constants.warningColor);
        }

        public static void logError(string message)
        {
            internalWriteLine($"ERROR: {message}", Constants.errorColor);
        }

        public static bool askYesOrNo(string question, bool acceptEnterAsYes = true)
        {
            bool? response = null;

            while (!response.HasValue)
            {
                System.Console.ResetColor();
                System.Console.ForegroundColor = Constants.questionColor;
                System.Console.Write(question + " [y/n] ");
                // internalWriteLine(question + " [y/n] ", questionColor);
                
                string input = System.Console.ReadLine();

                if (input.Trim().ToLower() == "y" || (acceptEnterAsYes && input.Trim() == ""))
                {
                    response = true;
                }
                else if (input.Trim().ToLower() == "n")
                {
                    response = false;
                }
                else
                {
                    internalWriteLine($"Invalid response \"{input.Trim()}\": type in \"y\" or \"n\"", Constants.errorColor);
                }
            }

            return response.Value;
        }

        public static void writeBottomLineOverwriteExisting(string str)
        {
            System.Console.ResetColor();
            System.Console.Write("\r" + str.PadRight(System.Console.WindowWidth));
        }

        public static void logLine(string str)
        {
            internalWriteLine(str);
        }

        private static void internalWriteLine(string str, System.ConsoleColor foregroundColor)
        {
            List<System.ConsoleColor> charColors = Enumerable.Repeat(foregroundColor, str.Length).ToList();
            
            internalWriteLine(str, charColors);
        }

        private static void internalWriteLine(string str) => internalWriteLine(str, System.Console.ForegroundColor);

        private static void internalWriteLine(string str, List<System.ConsoleColor> charColors)
        {            
            // number of returns to print
            int returnCount = str.Count(ch => ch == '\n');
            // create extra returns at bottom of screen and moves cursor to starting place
            System.Console.ResetColor();
            System.ConsoleColor currentColor = System.Console.ForegroundColor;
            for (int i = 0; i < str.Length; i++)
            {
                if (currentColor != charColors[i])
                {
                    currentColor = charColors[i];
                    System.Console.ForegroundColor = charColors[i];
                }
                System.Console.Write(str[i]);
            }

            System.Console.ResetColor();
            System.Console.Write("\n");
        }

}