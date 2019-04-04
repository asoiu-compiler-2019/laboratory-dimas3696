using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuranceLanguage.Syntax.Declarations;

namespace InsuranceLanguage.Parser
{
    public sealed partial class SyntaxAnaliser
    {
        bool error;
        ErrorSink errorSink;
        int index;
        ParserOptions options;
        SourceCode sourceCode;
        IEnumerable<Token> tokens;

        Token currentToken => tokens.ElementAtOrDefault(index) != null ? tokens.ElementAtOrDefault(index) : tokens.Last();
        Token lastToken => Peek(-1);
        Token nextToken => Peek(1);        

        public SyntaxAnaliser(ErrorSink errorSink)
        {
            this.errorSink = errorSink;
        }

        public SyntaxAnaliser() : this(new ErrorSink()) { }


        private Token Peek(int shift)
        {
            return tokens.ElementAtOrDefault(index + shift) != null ? tokens.ElementAtOrDefault(index) : tokens.Last();
        }

        public void AddError(Severity severity, string mes, SourceSpan? span = null)
        {
            errorSink.AddError(mes, sourceCode, severity, span ?? CreateSpan(currentToken));
        }

        private SourceSpan CreateSpan(SourceLocation start, SourceLocation end)
        {
            return new SourceSpan(start, end);
        }

        private SourceSpan CreateSpan(SourceLocation start)
        {
            return CreateSpan(start, currentToken.SourceSpan.End);
        }

        private SourceSpan CreateSpan(Token start)
        {
            return CreateSpan(start.SourceSpan.Start, currentToken.SourceSpan.End);
        }

        private SyntaxError SyntaxError(Severity severity, string mes, SourceSpan? span = null)
        {
            error = true;
            AddError(severity, mes, span);
            return new SyntaxError(mes);
        }

        private void MoveNext()
        {
            index++;
        }

        private SyntaxError NotRecognizedToken(string name)
        {
            MoveNext();
            var val = String.IsNullOrEmpty(lastToken?.Value) ? lastToken.TokenKind.ToString() : lastToken.Value;
            string mes = $"Error: can`t recognition token : {val}. {name}";

            return SyntaxError(Severity.Error, mes, lastToken.SourceSpan);
        }

        private SyntaxError NotRecognizedToken(TokenKind token)
        {
            return NotRecognizedToken(token.ToString());
        }

        private Token TakeToken()
        {
            Token token = currentToken;
            MoveNext();
            return token;
        }

        private Token TakeToken(TokenKind tokenKind)
        {
            if (currentToken != tokenKind)
                throw NotRecognizedToken(tokenKind);
            return TakeToken();
        }

        private Token TakeToken(string contextReservKeyword)
        {
            if (currentToken != TokenKind.Identifier && currentToken != contextReservKeyword)
                throw NotRecognizedToken(contextReservKeyword);
            return TakeToken();
        }

        private Token TakeTokenKeyword(string keyword)
        {
            if (currentToken != TokenKind.Keyword && currentToken != keyword)
                throw NotRecognizedToken(keyword);
            return TakeToken();
        }

        private Token TakeTokenSemicolon()
        {
            if (options.UseSemicolons || currentToken == TokenKind.Semicolon)
                return TakeToken(TokenKind.Semicolon);
            return currentToken;
        }

        private void CreateStatement(Action action, TokenKind breakTokenKind = TokenKind.Semicolon)
        {
            try
            {
                while (currentToken != breakTokenKind && currentToken != TokenKind.EndOfFile)
                    action();
            }
            catch (SyntaxError){
                while (currentToken != breakTokenKind && currentToken != TokenKind.EndOfFile)
                    TakeToken();
            }
            finally
            {
                if (error)
                {
                    if (lastToken == breakTokenKind)
                        index--;
                    if(currentToken != breakTokenKind)
                    {
                        while (currentToken != breakTokenKind && currentToken != TokenKind.EndOfFile)
                            TakeToken();
                    }
                    error = false;
                }
                if (breakTokenKind == TokenKind.Semicolon)
                    TakeTokenSemicolon();
                else
                    TakeToken(breakTokenKind);
            }
        }

        private void ParserInit(SourceCode sourceCode, IEnumerable<Token> tokens, ParserOptions options)
        {
            this.sourceCode = sourceCode;
            this.tokens = tokens;
            this.options = options;
            index = 0;
        }

        private void CreateBlock(Action action, TokenKind beginTokenKind = TokenKind.LeftBracket, TokenKind endTokenKind = TokenKind.RightBracket)
        {
            TakeToken(beginTokenKind);
            CreateStatement(action, endTokenKind);
        }

        private ClassDeclaration SyntaxAnalizeClassDeclaration()
        {
            List<FieldDeclaration> field = new List<FieldDeclaration>();
            
        }

    }
}
