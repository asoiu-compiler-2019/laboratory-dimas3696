using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceLanguage
{
    public sealed class LexerAnaliser
    {
        private static readonly string[] _Keywords = { "true", "false", "null", "str", "no_integer", "number", "delimetr", "op", "upper_symbol", "lower_symbol", "digital",
                                                        "document_type", "variable", "client_code", "client_document", "boolean", "client_is_entity", "client_is_car_owner",
                                                        "client_is_beneficiary", "client", "company_code", "company_coefficients", "company", "engine_capacity", "car_type",
                                                        "car_registration", "car", "conditions", "document", "if", "else", "for"};
        private StringBuilder _builder;
        private int _column;
        private ErrorSink _errorSink;
        private int _index;
        private int _line;
        private SourceCode _sourceCode;
        private SourceLocation _tokenStart;

        public ErrorSink ErrorSink => _errorSink;
        private char _ch => _sourceCode[_index];
        private char _last => Peek(-1);
        private char _next => Peek(1);

        public LexerAnaliser(ErrorSink errorSink)
        {
            _errorSink = errorSink;
            _builder = new StringBuilder();
            _sourceCode = null;
        }

        public LexerAnaliser() : this(new ErrorSink()) { }

        public LexerAnaliser(SourceCode sourceCode)
        {
            _sourceCode = sourceCode;
            _builder.Clear();
            _line = 1;
            _index = 0;
            _column = 0;
            
        }

        private Token CreateToken(TokenKind kind)
        {
            string contents = _builder.ToString();
            SourceLocation end = new SourceLocation(_index, _line, _column);
            SourceLocation start = _tokenStart;

            _tokenStart = end;
            _builder.Clear();

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
            _line++;
            _column = 0;
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
            var span = new SourceSpan(_tokenStart, new SourceLocation(_index, _line, _column));
            _errorSink.AddError(message, _sourceCode, severity, span);
        }

        private Token ScanWord(Severity severity = Severity.Error, string message = "Unexpected token '{0}'")
        {
            while (!IsWhiteSpace() && !IsPuctuation() && !IsEOF())
                Consume();
            AddError(string.Format(message, _builder.ToString()), severity);
            return CreateToken(TokenKind.Error);
            
        }

        private Token ScanIntager()
        {
            while (IsDigit())
                Consume();

            if (_ch == '.' || _ch == 'e' || _ch == 'f')
                return ScanFloat();
            if (!IsWhiteSpace() && !IsPuctuation() && !IsEOF())
                return ScanWord();

            return CreateToken(TokenKind.IntegerLiteral);
        }

        private bool IsLetterOrDigit()
        {
            return Char.IsLetterOrDigit(_ch);
        }

        private bool IsIdentifire()
        {
            return IsLetterOrDigit() || _ch == '_';
        }

        private bool IsKeyword()
        {
            return _Keywords.Contains(_builder.ToString());
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
            
            while(_ch != '"')
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
            switch (_ch)
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
                    if(_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.GreaterThanOrEqual);
                    }
                    return CreateToken(TokenKind.GreaterThan);
                case '<':
                    Consume();
                    if(_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.LessThanOrEqual);
                    }
                    return CreateToken(TokenKind.LessThan);
                case '+':
                    Consume();
                    if(_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.PlusEqual);
                    }
                    else if(_ch == '+')
                    {
                        Consume();
                        return CreateToken(TokenKind.PlusPlus);
                    }
                    return CreateToken(TokenKind.Plus);
                case '-':
                    Consume();
                    if(_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.MinusEqual);
                    }
                    else if(_ch == '-')
                    {
                        Consume();
                        return CreateToken(TokenKind.MinusMinus);
                    }
                    return CreateToken(TokenKind.Minus);
                case '=':
                    Consume();
                    if(_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.Equal);
                    }
                    return CreateToken(TokenKind.Assignment);
                case '!':
                    Consume();
                    if(_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.NotEqual);
                    }
                    return CreateToken(TokenKind.Not);
                case '*':
                    Consume();
                    if(_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.MulEqual);
                    }
                    return CreateToken(TokenKind.Mul);
                case '/':
                    Consume();
                    if(_ch == '=')
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
                    if(_ch == '&')
                    {
                        Consume();
                        return CreateToken(TokenKind.BooleanAnd);
                    }
                    else if(_ch == '=')
                    {
                        Consume();
                        return CreateToken(TokenKind.BitwiseAndEqual);
                    }
                    return CreateToken(TokenKind.BitwiseAnd);
                case '|':
                    Consume();
                    if (_ch == '|')
                    {
                        Consume();
                        return CreateToken(TokenKind.BooleanOr);
                    }
                    else if(_ch == '=')
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
            else if (IsLetter() || _ch == '_')
                return ScanIdentifire();
            else if (_ch == '"')
                return ScanStringLiteral();
            else if (_ch == '.' && Char.IsDigit(_next))
                return ScanFloat();
            else if (IsPuctuation())
                return ScanPunctuation();
            else
                return ScanWord();
        }

        public IEnumerable<Token> LexFile(SourceCode sourceCode)
        {
            _sourceCode = sourceCode;
            _builder.Clear();
            _line = 1;
            _index = 0;
            _column = 0;
            CreateToken(TokenKind.EndOfFile);

            return LexContents();
        }

        public IEnumerable<Token> LexFile(string sourceCode)
        {
            return LexFile(new SourceCode(sourceCode));
        }

        private char Peek(int ahead)
        {
            return _sourceCode[_index + ahead];
        }

        public void Advance()
        {
            _index++;
            _column++;
        }

        public void Consume()
        {
            _builder.Append(_ch);
            Advance();
        }

        private bool IsEOF()
        {
            return _ch == '\0';
        }

        private bool IsNewLine()
        {
            return _ch == '\n';
        }

        private bool IsDigit()
        {
            return Char.IsDigit(_ch);
        }

        private bool IsLetter()
        {
            return Char.IsLetter(_ch);
        }

        private bool IsWhiteSpace()
        {
            return (char.IsWhiteSpace(_ch) || IsEOF()) && !IsNewLine();
        }

        private bool IsPuctuation()
        {
            return "<>{}()[]!%^&*+-=/.,?;:|".Contains(_ch);
        }

        private Token ScanFloat()
        {
            if(_ch == 'f')
            {
                Advance();
                if((!IsWhiteSpace() && !IsPuctuation() && !IsEOF()) || _ch == '.')
                {
                    return ScanWord(message: "Remove 'f' in floating point number.");
                }
                return CreateToken(TokenKind.FloatLiteral);
            }

            int preDotLength = _index - _tokenStart.Index;

            if(_ch == '.')
            {
                Consume();
            }

            while (IsDigit())
            {
                Consume();
            }

            if (_last == '.')
                return ScanWord(message: "Must contains digits after '.'");

            if(_ch == 'e')
            {
                Consume();
                if (preDotLength > 1)
                    return ScanWord(message: "First number must be less than 10.");

                if (_ch == '+' || _ch == '-')
                    Consume();

                while (IsDigit())
                    Consume();
            }

            if (_ch == 'f')
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
