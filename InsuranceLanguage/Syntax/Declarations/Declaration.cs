using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax.Declarations
{
    public abstract class Declaration : SyntaxNode
    {
        public override SyntaxCatagory Catagory => SyntaxCatagory.Declaration;
        public string Name { get; }

        public Declaration(SourceSpan sourceSpan, string name) : base(sourceSpan)
        {
            Name = name;
        }
    }
}
