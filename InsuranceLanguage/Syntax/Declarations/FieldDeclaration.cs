using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuranceLanguage.Syntax.Declarations;

namespace InsuranceLanguage.Syntax.Declarations
{
    public class FieldDeclaration : Declaration
    {
        public Exception DefValue { get; }
        public string Type { get; }
        public override SyntaxKind Kind => SyntaxKind.FieldDeclaration;

        public FieldDeclaration(SourceSpan sourceSpan, string name, string type, Exception val) : base(sourceSpan, name)
        {
            Type = type;
            DefValue = val;
        }
    }
}
