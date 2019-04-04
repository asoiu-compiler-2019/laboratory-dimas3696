using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax
{
    public enum SyntaxKind
    {
        Invalid,
        SourceDocument,
        BinaryExpression,
        UnaryExpression,
        IdentifierExpression,
        BlockStatement,
        IfStatement,
        ElseStatement,
        VoidStatement,
        ForStatement,
        VariableDeclaration,
        ClassDeclaration,//
        FieldDeclaration,

        //
        ClientDeclaration,
        CompanyDeclaration,
        CarDeclaration,
        ConditionsDeclaration,
        
    }

    public enum SyntaxCatagory
    {
        Expression,
        Statement,
        Declaration,
        Document
    }

    public abstract class SyntaxNode
    {
        public abstract SyntaxCatagory Catagory { get; }

        public abstract SyntaxKind Kind { get; }

        public SourceSpan Span { get; }

        protected SyntaxNode(SourceSpan sourceSpan)
        {
            Span = sourceSpan;
        }
    }
}
