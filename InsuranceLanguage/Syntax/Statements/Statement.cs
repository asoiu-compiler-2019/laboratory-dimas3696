using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax.Statements
{
    public abstract class Statement : SyntaxNode
    {
        public override SyntaxCatagory Catagory => SyntaxCatagory.Statement;

        public Statement(SourceSpan sourceSpan) : base(sourceSpan) { }
    }
}
