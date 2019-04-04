using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax.Expressions
{
    public abstract class Expression : SyntaxNode
    {
        public override SyntaxCatagory Catagory => SyntaxCatagory.Expression;

        public Expression(SourceSpan sourceSpan) : base(sourceSpan) { }
    }
}
