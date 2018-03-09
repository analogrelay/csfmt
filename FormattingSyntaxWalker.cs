using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace fowlerize
{
    public class FormattingSyntaxWalker : CSharpSyntaxRewriter
    {
        public FormattingSyntaxWalker(bool visitIntoStructuredTrivia = false) : base(visitIntoStructuredTrivia)
        {
        }

        public override SyntaxNode VisitIfStatement(IfStatementSyntax node)
        {
            // Grab the first trailing trivia of the If Keyword
            var triviaList = node.IfKeyword.TrailingTrivia;
            if (!triviaList.Any() || !triviaList.First().IsKind(SyntaxKind.WhitespaceTrivia))
            {
                var newIf = node.IfKeyword.WithTrailingTrivia(triviaList.Prepend(SyntaxFactory.Whitespace(" ")));
                return node.WithIfKeyword(newIf);
            }
            else {
                return node;
            }
        }
    }
}