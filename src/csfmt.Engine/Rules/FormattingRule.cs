using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace csfmt.Rules
{
    public abstract class FormattingRule
    {
        public abstract IReadOnlyList<Diagnostic> Check(SyntaxNode node);

        public abstract SyntaxNode Fix(Diagnostic diagnostic, SyntaxNode root);
    }
}