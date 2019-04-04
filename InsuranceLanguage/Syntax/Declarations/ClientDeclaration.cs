using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax.Declarations
{
    public class ClientDeclaration : Declaration
    {
        public override SyntaxKind Kind => SyntaxKind.ClientDeclaration;
        public string FullName { get; }
        public DateTime DateOfBirthday { get; }
    }
}
