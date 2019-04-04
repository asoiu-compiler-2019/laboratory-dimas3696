using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax.Declarations
{
    public class ClassDeclaration : Declaration
    {
        public IEnumerable<FieldDeclaration> Fields { get; }
        public override SyntaxKind Kind => SyntaxKind.ClassDeclaration;

        public ClassDeclaration(SourceSpan sourceSpan, string name, IEnumerable<FieldDeclaration> fieldDeclarations) : base(sourceSpan, name)
        {
            Fields = fieldDeclarations;
        }
    }
}
