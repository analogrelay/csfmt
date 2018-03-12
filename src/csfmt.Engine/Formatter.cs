using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using csfmt.Rules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace csfmt
{
    public static class Formatter
    {
        public static IList<Diagnostic> Check(string file, IReadOnlyList<FormattingRule> rules)
        {
            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file), path: file);

            var diags = new List<Diagnostic>();
            foreach (var rule in rules)
            {
                diags.AddRange(rule.Check(tree.GetRoot()));
            }

            return diags;
        }

        public static void Fix(Diagnostic diag, IReadOnlyList<FormattingRule> rules)
        {
            var mappedLineSpan = diag.Location.GetMappedLineSpan();
            var path = mappedLineSpan.Path;
            var fileContent = File.ReadAllText(path);
            var tree = CSharpSyntaxTree.ParseText(fileContent, path: path);

            var root = tree.GetRoot();
            foreach (var rule in rules)
            {
                root = rule.Fix(diag, root);
            }

            tree = tree.WithRootAndOptions(root, tree.Options);
            using (var writer = new StreamWriter(path))
            {
                tree.GetText().Write(writer);
            }
        }
    }
}