using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage
{
    public sealed class LexerAnaliser
    {
        static readonly string[] _Keywords = { "true", "false", "null", "str", "no_integer", "number", "delimetr", "op", "upper_symbol", "lower_symbol", "digital",
                                                        "document_type", "variable", "client_code", "client_document", "boolean", "client_is_entity", "client_is_car_owner",
                                                        "client_is_beneficiary", "client", "company_code", "company_coefficients", "company", "engine_capacity", "car_type",
                                                        "car_registration", "car", "conditions", "document", "if", "else", "for"};
        StringBuilder builder;
        int column;
        ErrorSink errorSink;
        int index;
        int line;
        SourceCode sourceCode;
        SourceLocation tokenStart;

        public ErrorSink ErrorSink => errorSink;
        char ch => sourceCode[index];
        char last => Peek(-1);
        char next => Peek(1);

        public LexerAnaliser(ErrorSink errorSink)
        {
            this.errorSink = errorSink;
            builder = new StringBuilder();
            sourceCode = null;
        }

        public LexerAnaliser() : this(new ErrorSink()) { }

        public LexerAnaliser(SourceCode sourceCode)
        {
            this.sourceCode = sourceCode;
            builder.Clear();
            line = 1;
            index = 0;
            column = 0;
            
        }

        private Token CreateToken(TokenKind kind)
        {
            string contents = builder.ToString();
            SourceLocation end = new SourceLocation(index, line, column);
            SourceLocation start = tokenStart;

            tokenStart = end;
            builder.Clear();

            return new Token(kind, contents, start, end);
        }

        private IEnumerable<Token> LexContents()
        {
            while (!IsEOF())
            {
                yield return LexToken();
            }

            yield return CreateToken(TokenKind.EndOfFile);
        }

        private void DoNewLine()
        {
            line++;
            column = 0;
        }

        private Token ScanNewLine()
        {
            Consume();
            DoNewLine();

            return CreateToken(TokenKind.NewLine);
        }

        private Token ScanWhiteSpace()
        {
            while (IsWhiteSpace())
                Consume();

            return CreateToken(TokenKind.WhiteSpace);
        }

        private void AddError(string message, Severity severity)
        {
            var span = new SourceSpan(tokenStart, new SourceLocation(index, line, column));
            errorSink.AddError(message, sourceCode, severity, span);
        }

        private Token ScanWord(Severity severity = Severity.Error, string message = "Unexpected token '{0}'")
        {
            while (!IsWhiteSpace() && !IsPuctuation() && !IsEOF())
                Consume();
            AddError(string.Format(message, builder.ToString()), severity);
            return CreateToken(TokenKind.Error);
            
        }

        private Token ScanIntager()
        {
            while (IsDigit())
                Consume();

            if (ch == '.' || ch == 'e' || ch == 'f')
                return ScanFloat();
            if (!IsWhiteSpace() && !IsPuctuation() && !IsEOF())
                return ScanWord();

            return CreateToken(TokenKind.IntegerLiteral);
        }

        private bool IsLetterOrDigit()
        {
            return Char.IsLetterOrDigit(ch);
        }

        private bool IsIdentifire()
        {
            return IsLetterOrDigit() || ch == '_';
        }

        private bool IsKeyword()
        {
            return _Keywords.Contains(builder.ToString());
        }

        private Token ScanIdentifire()
        {
            while (IsIdentifire())
                Consume();

            if (!IsWhiteSpace() && !IsPuctuation() && !IsEOF())
                ScanWord();
            if (IsKeyword())
                return CreateToken(TokenKind.Keyword);

            return CreateToken(TokenKind.Identifier);
        }

        private Token ScanStringLiteral()
        {
            Advance();
            
            while(ch != '"')
            {
                if (IsEOF())
                {
                    AddError("Unepected EOF", Severity.Fatal);
                    return CreateToken(TokenKind.Error);
                }
                Consume();
            }

            Advance();

            return CreateToken(TokenKind.Error);
        }

        private Token ScanPunctuation()
        {
            switch (ch)
            {
                case ';':
                    Consume();
                    return CreateToken(TokenKind.Semicolon);
                case ':':
                    Consume();
                    return CreateToken(TokenKind.Colon);
                case '{':
                    Consume();
                    return CreateToken(TokenKind.LeftBracket);
                case '}':
                    Consume();
                    return CreateToken(TokenKind.RightBracket);
                case '[':
                    Consume();
                    return CreateToken(TokenKind.LeftBrace);
                case ']':
                    Consume();
                    return CreateToken(TokenKind.RightBrace);
                case '(':
                    Consume();
                    return CreateToken(TokenKind.LeftParenthesis);
                case ')':
                    Consume();
                    return CreateToken(TokenKind.RightParenthesis);
                case '>':
                    Consume();
                    if(ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.GreaterThanOrEqual);
                    }
                    return CreateToken(TokenKind.GreaterThan);
                case '<':
                    Consume();
                    if(ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.LessThanOrEqual);
                    }
                    return CreateToken(TokenKind.LessThan);
                case '+':
                    Consume();
                    if(ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.PlusEqual);
                    }
                    else if(ch == '+')
                    {
                        Consume();
                        return CreateToken(TokenKind.PlusPlus);
                    }
                    return CreateToken(TokenKind.Plus);
                case '-':
                    Consume();
                    if(ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.MinusEqual);
                    }
                    else if(ch == '-')
                    {
                        Consume();
                        return CreateToken(TokenKind.MinusMinus);
                    }
                    return CreateToken(TokenKind.Minus);
                case '=':
                    Consume();
                    if(ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.Equal);
                    }
                    return CreateToken(TokenKind.Assignment);
                case '!':
                    Consume();
                    if(ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.NotEqual);
                    }
                    return CreateToken(TokenKind.Not);
                case '*':
                    Consume();
                    if(ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.MulEqual);
                    }
                    return CreateToken(TokenKind.Mul);
                case '/':
                    Consume();
                    if(ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.DivEqual);
                    }
                    return CreateToken(TokenKind.Div);
                case '.':
                    Consume();
                    return CreateToken(TokenKind.Dot);
                case ',':
                    Consume();
                    return CreateToken(TokenKind.Comma);
                case '&':
                    Consume();
                    if(ch == '&')
                    {
                        Consume();
                        return CreateToken(TokenKind.BooleanAnd);
                    }
                    else if(ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.BitwiseAndEqual);
                    }
                    return CreateToken(TokenKind.BitwiseAnd);
                case '|':
                    Consume();
                    if (ch == '|')
                    {
                        Consume();
                        return CreateToken(TokenKind.BooleanOr);
                    }
                    else if(ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.BitwiseOrEqual);
                    }
                    return CreateToken(TokenKind.BitwiseOr);

                default: return ScanWord();
            }
        }

        private Token LexToken()
        {
            if (IsEOF())
                return CreateToken(TokenKind.EndOfFile);
            else if (IsNewLine())
                return ScanNewLine();
            else if (IsWhiteSpace())
                return ScanWhiteSpace();
            else if (IsDigit())
                return ScanIntager();
            else if (IsLetter() || ch == '_')
                return ScanIdentifire();
            else if (ch == '"')
                return ScanStringLiteral();
            else if (ch == '.' && Char.IsDigit(next))
                return ScanFloat();
            else if (IsPuctuation())
                return ScanPunctuation();
            else
                return ScanWord();
        }

        public IEnumerable<Token> LexFile(SourceCode sourceCode)
        {
            this.sourceCode = sourceCode;
            builder.Clear();
            line = 1;
            index = 0;
            column = 0;
            CreateToken(TokenKind.EndOfFile);

            return LexContents();
        }

        public IEnumerable<Token> LexFile(string sourceCode)
        {
            return LexFile(new SourceCode(sourceCode));
        }

        private char Peek(int ahead)
        {
            return sourceCode[index + ahead];
        }

        public void Advance()
        {
            index++;
            column++;
        }

        public void Consume()
        {
            builder.Append(ch);
            Advance();
        }

        private bool IsEOF()
        {
            return ch == '\0';
        }

        private bool IsNewLine()
        {
            return ch == '\n';
        }

        private bool IsDigit()
        {
            return Char.IsDigit(ch);
        }

        private bool IsLetter()
        {
            return Char.IsLetter(ch);
        }

        private bool IsWhiteSpace()
        {
            return (char.IsWhiteSpace(ch) || IsEOF()) && !IsNewLine();
        }

        private bool IsPuctuation()
        {
            return "<>{}()[]!%^&*+-=/.,?;:|".Contains(ch);
        }

        private Token ScanFloat()
        {
            if(ch == 'f')
            {
                Advance();
                if((!IsWhiteSpace() && !IsPuctuation() && !IsEOF()) || ch == '.')
                {
                    return ScanWord(message: "Remove 'f' in floating point number.");
                }
                return CreateToken(TokenKind.FloatLiteral);
            }

            int preDotLength = index - tokenStart.Index;

            if(ch == '.')
            {
                Consume();
            }

            while (IsDigit())
            {
                Consume();
            }

            if (last == '.')
                return ScanWord(message: "Must contains digits after '.'");

            if(ch == 'e')
            {
                Consume();
                if (preDotLength > 1)
                    return ScanWord(message: "First number must be less than 10.");

                if (ch == '+' || ch == '-')
                    Consume();

                while (IsDigit())
                    Consume();
            }

            if (ch == 'f')
                Consume();

            if(!IsWhiteSpace() && !IsPuctuation() && !IsEOF())
            {
                if (IsLetter())
                    return ScanWord(message: "'{0}' is an invalid float value");
                return ScanWord();
            }

            return CreateToken(TokenKind.FloatLiteral);
        }
    }
}
