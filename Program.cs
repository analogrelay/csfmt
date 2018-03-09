using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace fowlerize
{
    class Program
    {
        static int Main(string[] args)
        {
            var paths = new List<string>();
            for (var i = 0; i < args.Length; i += 1)
            {
                paths.Add(args[i]);
            }

            if (paths.Count == 0)
            {
                return Fail("Missing required argument: <PATHS...>");
            }

            var files = paths.SelectMany(p => Directory.EnumerateFiles(p, "*.cs", SearchOption.AllDirectories));
            foreach (var file in files)
            {
                Formatter.Format(file);
            }
            return 0;
        }

        private static int Fail(string message)
        {
            Usage();
            Console.Error.WriteLine(message);
            return 1;
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: fowlerize <PATHS...>");
        }
    }
}
