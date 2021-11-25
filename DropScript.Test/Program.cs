using System.IO;
using System;
using DropScript.Parsing;

namespace DropScript.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = Parser.Parse(File.ReadAllText("./scripts/command.drop"));
            Console.WriteLine("a");
        }
    }
}
