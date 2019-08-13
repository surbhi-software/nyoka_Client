using nyoka_Client;
using System.Collections.Generic;
using System.Linq;

public static class Logger
{
    public static void logWarning(string message)
    {
        internalWriteLine($"WARNING: {message}", Constants.warningColor);
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

public static void logTable(PrintTable table, bool visibleLines = true)
        {
            char verticalSeparator = visibleLines ? '|' : ' ';
            char horizontalSeparator = visibleLines ? '=' : ' ';
            char lineIntersectionChar = visibleLines ? '+' : ' ';

            string headerLine = "";
            List<System.ConsoleColor> headerLineCharColors = new List<System.ConsoleColor>();
            
            for (int col = 0; col < table.columnNames.Count; col++)
            {
                headerLine += verticalSeparator;
                headerLineCharColors.Add(Constants.tableFrameColor);

                // don't pad rightmost header if lines are not supposed to be visible
                if (visibleLines || col != table.columnNames.Count - 1)
                {
                    string nameStr = table.columnNames[col].PadRight(table.columnWidths[col]);
                    
                    headerLine += nameStr;
                    headerLineCharColors.AddRange(Enumerable.Repeat(Constants.tableHeaderColor, nameStr.Length));
                }
                else
                {
                    string paddedNameStr = table.columnNames[col];
                    
                    headerLine += paddedNameStr;
                    headerLineCharColors.AddRange(Enumerable.Repeat(Constants.tableHeaderColor, paddedNameStr.Length));
                }
            }
            
            headerLine += verticalSeparator;
            headerLineCharColors.Add(Constants.tableFrameColor);
            
            internalWriteLine(headerLine, headerLineCharColors);

            
            string horizontalLine = "";
            // log line
            if (visibleLines)
            {
                for (int col = 0; col < table.columnNames.Count; col++)
                {
                    horizontalLine += lineIntersectionChar;
                    horizontalLine += new string(horizontalSeparator, table.columnWidths[col]);

                    if (col == table.columnNames.Count - 1) horizontalLine += lineIntersectionChar; 
                }

                internalWriteLine(horizontalLine, Constants.tableFrameColor);
            }
            else
            {
                internalWriteLine("");
            }

            foreach (List<string> row in table.rows)
            {
                string lineString = "";
                List<System.ConsoleColor> lineCharColors = new List<System.ConsoleColor>();
                
                for (int col = 0; col < table.columnWidths.Count; col++)
                {
                    lineString += verticalSeparator;
                    lineCharColors.Add(Constants.tableFrameColor);

                    string tableValue = row[col].PadRight(table.columnWidths[col]);
                    
                    lineString += tableValue;
                    lineCharColors.AddRange(Enumerable.Repeat(System.Console.ForegroundColor, tableValue.Length));
                }
                
                lineString += verticalSeparator;
                lineCharColors.Add(Constants.tableFrameColor);

                internalWriteLine(lineString, lineCharColors);
            }
        }
}