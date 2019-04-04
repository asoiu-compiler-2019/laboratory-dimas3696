using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax.Statements
{
    public class VoidStatement : Statement
    {
        public override SyntaxKind Kind => SyntaxKind.VoidStatement;

        public VoidStatement(SourceSpan sourceSpan) : base(sourceSpan) { }
    }
}
