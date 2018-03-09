using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace fowlerize
{
    public static class Formatter
    {
        public static void Format(string file)
        {
            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(file), path: file);

            var newRoot = new FormattingSyntaxWalker().Visit(tree.GetRoot());

            var newTree = tree.WithRootAndOptions(newRoot, tree.Options);

            var output = Path.ChangeExtension(file, ".fixed.cs");
            using (var writer = new StreamWriter(output))
            {
                newTree.GetText().Write(writer);
            }
        }
    }
}