using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using csfmt.Rules;
using Microsoft.CodeAnalysis;

namespace csfmt
{
    class Program
    {
        static int Main(string[] args)
        {
            var fix = false;
            var paths = new List<string>();
            for (var i = 0; i < args.Length; i += 1)
            {
                switch (args[i])
                {
                    case "--force":
                    case "-f":
                        fix = true;
                        break;
                    default:
                        paths.Add(args[i]);
                        break;
                }
            }

            if (paths.Count == 0)
            {
                return Fail("Missing required argument: <PATHS...>");
            }

            var files = paths.SelectMany(p => Directory.EnumerateFiles(p, "*.cs", SearchOption.AllDirectories));
            var rules = new List<FormattingRule>();
            rules.Add(new ControlFlowKeywordSpacingRule());

            var diags = new List<Diagnostic>();
            foreach (var file in files)
            {
                diags.AddRange(Formatter.Check(file, rules));
            }

            foreach (var diag in diags)
            {
                if(fix) {
                    Formatter.Fix(diag, rules);
                }
                var mappedLineSpan = diag.Location.GetMappedLineSpan();
                Console.WriteLine($"{mappedLineSpan.Path}({mappedLineSpan.StartLinePosition.Line},{mappedLineSpan.StartLinePosition.Character}): {diag.GetMessage()}");
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
