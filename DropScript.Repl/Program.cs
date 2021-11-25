using System.Drawing;
using System;
using System.Linq;
using DropScript.Parsing;

namespace DropScript.Repl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("DropScript REPL");

            while (true)
            {
                try
                {
                    Console.Write("> ");
                    var script = Console.ReadLine();
                    if (script == null) break;

                    var tokens = Lexer.Analyze(script);

                    foreach (var (type, value) in tokens)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(type.ToString());
                        Console.ResetColor();
                        if (!string.IsNullOrEmpty(value))
                        {
                            Console.Write(": ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(value);
                        }
                        Console.ResetColor();
                        Console.WriteLine();
                    }
                }
                catch (ParserException e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("Syntax Error: " + e.Message);
                    Console.ResetColor();
                }
            }
        }
    }
}
