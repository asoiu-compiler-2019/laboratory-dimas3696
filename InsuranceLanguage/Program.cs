﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsuranceLanguage.Parser;

namespace InsuranceLanguage
{
    class Program
    {
        static void Main(string[] args)
        {
            LexerAnaliser lexer = new LexerAnaliser();
            SyntaxAnaliser parser = new SyntaxAnaliser(lexer.ErrorSink);

            while (true)
            {
                Console.Write("InsuranceLanguage> ");
                var program = Console.ReadLine();
                var sourceCode = new SourceCode(program);
                var tokens = lexer.LexFile(sourceCode).ToArray();

                foreach (var token in tokens)
                {
                    Console.WriteLine($"{token.TokenKind} ( \"{token.Value}\" ) ");
                }

                if (lexer.ErrorSink.Count() > 0)
                {
                    foreach (var error in lexer.ErrorSink)
                    {
                        Console.WriteLine(new string('-', Console.WindowWidth / 3));

                        WriteError(error);
                    }
                    lexer.ErrorSink.Clear();
                }
                else
                {
                    //var ast = parser.ParseFile(sourceCode, tokens, SyntaxAnalizer.OptionalSemicolons);
                    if (lexer.ErrorSink.Count() > 0)
                    {
                        foreach (var error in lexer.ErrorSink)
                        {
                            Console.WriteLine(new string('-', Console.WindowWidth / 3));

                            WriteError(error);
                        }
                        lexer.ErrorSink.Clear();
                    }
                }

                Console.WriteLine(new string('-', Console.WindowWidth / 2));
            }
        }

        private static void WriteError(ErrorEntry error)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (error.Lines.Length > 1)
            {
                Console.WriteLine(error.Lines.First());
                Console.CursorLeft = error.Span.Start.Column;
                Console.WriteLine(new string('^', error.Lines[0].Length - error.Span.Start.Column));
                for (int i = 1; i < error.Lines.Length - 1; i++)
                {
                    Console.WriteLine(error.Lines[i]);
                    Console.WriteLine(new string('^', error.Lines[i].Length));
                }
                Console.WriteLine(error.Lines.Last());
                Console.WriteLine(new string('^', error.Lines.Last().Length - error.Span.End.Column));
            }
            else
            {
                Console.WriteLine(error.Lines.First());
                Console.CursorLeft = error.Span.Start.Column;
                Console.WriteLine(new string('^', error.Span.Length));
                Console.WriteLine($"{error.Severity} {error.Span}: {error.Message}");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
