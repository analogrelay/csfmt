using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace csfmt.Rules
{
    public class ControlFlowKeywordSpacingRule : FormattingRule
    {
        public static readonly DiagnosticDescriptor Descriptor = new DiagnosticDescriptor(
            "FMT0001",
            "Control-Flow keywords should be separated from parenthesis with a space",
            "Add a space following the '{0}' keyword",
            "Formatting.Spacing",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true);

        public override IReadOnlyList<Diagnostic> Check(SyntaxNode node)
        {
            var v = new Visitor();
            v.Visit(node);
            return v.Diagnostics;
        }

        public override SyntaxNode Fix(Diagnostic diagnostic, SyntaxNode root)
        {
            // First, assert that we are the source of the diagnostic
            if (!ReferenceEquals(diagnostic.Descriptor, Descriptor))
            {
                // We aren't, just do nothing
                return root;
            }

            // Identify the token from the span
            var token = root.FindToken(diagnostic.Location.SourceSpan.Start);
            if (token == null)
            {
                throw new InvalidOperationException("Unable to locate token at the provided location");
            }

            // Prepend a space to the token
            var newToken = token.WithTrailingTrivia(token.TrailingTrivia.Prepend(SyntaxFactory.Whitespace(" ")));

            // Replace the token in the tree
            return root.ReplaceToken(token, newToken);
        }

        private class Visitor : CSharpSyntaxWalker
        {
            public List<Diagnostic> Diagnostics { get; } = new List<Diagnostic>();

            public override void VisitCatchClause(CatchClauseSyntax node)
                => CheckToken(node.CatchKeyword);
            public override void VisitDoStatement(DoStatementSyntax node)
                => CheckToken(node.WhileKeyword);
            public override void VisitSwitchStatement(SwitchStatementSyntax node)
                => CheckToken(node.SwitchKeyword);
            public override void VisitUsingStatement(UsingStatementSyntax node)
                => CheckToken(node.UsingKeyword);
            public override void VisitFixedStatement(FixedStatementSyntax node)
                => CheckToken(node.FixedKeyword);
            public override void VisitLockStatement(LockStatementSyntax node)
                => CheckToken(node.LockKeyword);
            public override void VisitWhileStatement(WhileStatementSyntax node)
                => CheckToken(node.WhileKeyword);
            public override void VisitForStatement(ForStatementSyntax node)
                => CheckToken(node.ForKeyword);
            public override void VisitForEachStatement(ForEachStatementSyntax node)
                => CheckToken(node.ForEachKeyword);
            public override void VisitIfStatement(IfStatementSyntax node)
                => CheckToken(node.IfKeyword);

            private void CheckToken(SyntaxToken token)
            {
                // Grab the first trailing trivia of the If Keyword
                var triviaList = token.TrailingTrivia;
                if (!triviaList.Any() || !triviaList.First().IsKind(SyntaxKind.WhitespaceTrivia))
                {
                    Diagnostics.Add(Diagnostic.Create(Descriptor, token.GetLocation(), token.Text));
                }
            }
        }
    }
}