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
        ConstantExpression,
        ReferenceExpression,
        MethodCallExpression,
        ParameterDeclaration,
        BlockStatement,
        LambdaExpression,
        NewExpression,
        ArrayAccessExpression,
        WhileStatement,
        IfStatement,
        ElseStatement,
        SwitchStatement,
        CaseStatement,
        EmptyStatement,
        BreakStatement,
        ContinueStatement,
        ForStatement,
        VariableDeclaration,
        ClassDeclaration,
        FieldDeclaration,
        MethodDeclaration,
        ConstructorDeclaration,
        PropertyDeclaration,
        ReturnStatement,
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
