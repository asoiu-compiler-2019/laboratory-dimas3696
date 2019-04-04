using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax.Expressions
{
    public class IdentifierExpression : Expression
    {
        public string Identifier { get; }
        public override SyntaxKind Kind => SyntaxKind.IdentifierExpression;

        public IdentifierExpression(SourceSpan sourceSpan, string identifier) : base(sourceSpan)
        {
            Identifier = identifier;
        }
    }
}
