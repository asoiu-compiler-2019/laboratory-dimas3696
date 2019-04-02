using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage
{
    public enum TokenCatagory
    {
        Unknown,
        WhiteSpace,
        
        Identifier,
        Grouping,
        Punctuation,
        Operator,
        Constant,

        Invalid
    }

    public sealed class Token : IEquatable<Token>
    {
        public Lazy<TokenCatagory> _Catagory;
        public TokenCatagory Catagory => _Catagory.Value;
        public TokenKind TokenKind { get; }
        public SourceSpan SourceSpan { get; }
        public string Value { get; }

        public Token(TokenKind tokenKind, string contents, SourceLocation start, SourceLocation end)
        {
            TokenKind = tokenKind;
            Value = contents;
            SourceSpan = new SourceSpan(start, end);

            _Catagory = new Lazy<TokenCatagory>(GetTokenCatagory);
        }
        public static bool operator !=(Token left, string right)
        {
            return left?.Value != right;
        }

        public static bool operator !=(string left, Token right)
        {
            return right?.Value != left;
        }

        public static bool operator !=(Token left, TokenKind right)
        {
            return left?.TokenKind != right;
        }

        public static bool operator !=(TokenKind left, Token right)
        {
            return right?.TokenKind != left;
        }

        public static bool operator ==(Token left, string right)
        {
            return left?.Value == right;
        }

        public static bool operator ==(string left, Token right)
        {
            return right?.Value == left;
        }

        public static bool operator ==(Token left, TokenKind right)
        {
            return left?.TokenKind == right;
        }

        public static bool operator ==(TokenKind left, Token right)
        {
            return right?.TokenKind == left;
        }

        public override bool Equals(object obj)
        {
            if (obj is Token)
            {
                return Equals((Token)obj);
            }
            return base.Equals(obj);
        }

        public bool Equals(Token other)
        {
            if (other == null)
            {
                return false;
            }
            return other.Value == Value &&
                   other.SourceSpan == SourceSpan &&
                   other.TokenKind == TokenKind;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ SourceSpan.GetHashCode() ^ TokenKind.GetHashCode();
        }

        public bool IsTrivia()
        {
            return Catagory == TokenCatagory.WhiteSpace;
        }

        private TokenCatagory GetTokenCatagory()
        {
            switch (TokenKind)
            {
                case TokenKind.Colon:
                case TokenKind.Semicolon:
                case TokenKind.Comma:
                case TokenKind.Dot:
                    return TokenCatagory.Punctuation;

                case TokenKind.Equal:
                case TokenKind.NotEqual:
                case TokenKind.Not:
                case TokenKind.LessThan:
                case TokenKind.LessThanOrEqual:
                case TokenKind.GreaterThan:
                case TokenKind.GreaterThanOrEqual:
                case TokenKind.Minus:
                case TokenKind.MinusEqual:
                case TokenKind.MinusMinus:
                case TokenKind.Mul:
                case TokenKind.MulEqual:
                case TokenKind.Plus:
                case TokenKind.PlusEqual:
                case TokenKind.PlusPlus:
                case TokenKind.Question:
                case TokenKind.DoubleQuestion:
                case TokenKind.DivEqual:
                case TokenKind.Div:
                case TokenKind.BooleanOr:
                case TokenKind.BooleanAnd:
                case TokenKind.BitwiseOrEqual:
                case TokenKind.BitwiseOr:
                case TokenKind.BitwiseAndEqual:
                case TokenKind.BitwiseAnd:
                case TokenKind.Assignment:
                    return TokenCatagory.Operator;

                case TokenKind.NewLine:
                case TokenKind.WhiteSpace:
                    return TokenCatagory.WhiteSpace;

                case TokenKind.LeftBrace:
                case TokenKind.LeftBracket:
                case TokenKind.LeftParenthesis:
                case TokenKind.RightBrace:
                case TokenKind.RightBracket:
                case TokenKind.RightParenthesis:
                    return TokenCatagory.Grouping;

                case TokenKind.Identifier:
                case TokenKind.Keyword:
                    return TokenCatagory.Identifier;

                case TokenKind.StringLiteral:
                case TokenKind.IntegerLiteral:
                case TokenKind.FloatLiteral:
                    return TokenCatagory.Constant;

                case TokenKind.Error:
                    return TokenCatagory.Invalid;

                default: return TokenCatagory.Unknown;
            }
        }
    }

    public enum TokenKind
    {
        EndOfFile,
        Error,

        #region WhiteSpace

        WhiteSpace,
        NewLine,

        #endregion WhiteSpace


        #region Constants

        IntegerLiteral,
        StringLiteral,
        FloatLiteral,

        #endregion Constants

        #region Identifiers

        Identifier,
        Keyword,

        #endregion Identifiers

        #region Groupings

        LeftBracket, // {
        RightBracket, // }
        RightBrace, // ]
        LeftBrace, // [
        LeftParenthesis, // (
        RightParenthesis, // )

        #endregion Groupings

        #region Operators

        GreaterThanOrEqual, // >=
        GreaterThan, // >

        LessThan, // <
        LessThanOrEqual, // <=

        PlusEqual, // +=
        PlusPlus, // ++
        Plus, // +

        MinusEqual, // -=
        MinusMinus, // --
        Minus, // -

        Assignment, // =

        Not, // !
        NotEqual, // !=

        Mul, // *
        MulEqual, // *=

        Div, // /
        DivEqual, // /=

        BooleanAnd, // &&
        BooleanOr, // ||

        BitwiseAnd, // &
        BitwiseOr, // |

        BitwiseAndEqual, // &=
        BitwiseOrEqual, // |=

        DoubleQuestion, // ??
        Question, // ?

        Equal, // ==

        #endregion Operators



        #region Punctuation

        Dot,
        Comma,
        Semicolon,
        Colon,

        #endregion Punctuation
    }
}
