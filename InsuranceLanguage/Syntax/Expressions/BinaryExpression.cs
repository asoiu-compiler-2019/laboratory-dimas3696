using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage.Syntax.Expressions
{
    public class BinaryExpression : Expression
    {
        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
        public Expression Left { get; }
        public Expression Right { get; }
        public BinaryOperator Operator { get; }

        public BinaryExpression(SourceSpan sourceSpan, Expression left, Expression right, BinaryOperator binaryOperator) : base(sourceSpan)
        {
            Left = left;
            Right = right;
            Operator = binaryOperator;
        }

    }
    public enum BinaryOperator
    {
        #region Assignment

        Assign,
        AddAssign,
        SubAssign,
        MulAssign,
        DivAssign,
        AndAssign,
        OrAssign,

        #endregion Assignment

        #region Logical

        LogicalOr,
        LogicalAnd,

        #endregion Logical

        #region Equality

        Equal,
        NotEqual,

        #endregion Equality

        #region Relational

        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,

        #endregion Relational

        #region Bitwise

        BitwiseAnd,
        BitwiseOr,

        #endregion Bitwise

        #region Additive

        Add,
        Sub,

        #endregion Additive

        #region Multiplicative

        Mul,
        Div,

        #endregion Multiplicative
    }
}
