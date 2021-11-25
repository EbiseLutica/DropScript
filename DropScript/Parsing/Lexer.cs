using System.Collections.Generic;
using System;
using System.Collections;

namespace DropScript.Parsing
{
    /// <summary>
    /// DropScriptの字句解析器。
    /// </summary>
    public static class Lexer
    {
        public static List<Token> Analyze(string script)
        {
            // 改行コード正規化
            script = script.Replace("\r\n", "\n").Replace("\r", "\n");

            var tokens = new List<Token>();
            var buffer = "";
            var isQuote = false;

            void addToken(TokenType token, string? value = null)
            {
                tokens?.Add(new Token(token, value));
            }

            void addBuffer(bool force = false)
            {
                if (buffer?.Length > 0 || force)
                {
                    addToken(TokenType.String, buffer);
                    clearBuffer();
                }
            }

            void addBufferAndToken(TokenType token, string? value = null)
            {
                addBuffer();
                addToken(token, value);
            }

            void pushBuffer(char value) => buffer += value;

            void clearBuffer() => buffer = "";

            for (var i = 0; i < script.Length; i++)
            {
                var current = script[i];
                if (!isQuote)
                {
                    switch (current)
                    {
                        case ' ': addBufferAndToken(TokenType.WhiteSpace); break;
                        case '+': addBufferAndToken(TokenType.Plus); break;
                        case '#':
                            while (i < script.Length - 1 && script[i + 1] != '\n') i++;
                            break;
                        case '\n': addBufferAndToken(TokenType.Newline); break;
                        case '$': addBufferAndToken(TokenType.DollarSign); break;
                        case '{': addBufferAndToken(TokenType.LeftCurlyBrace); break;
                        case '}': addBufferAndToken(TokenType.RightCurlyBrace); break;
                        case ',': addBufferAndToken(TokenType.Comma); break;
                        case '@': addBufferAndToken(TokenType.At); break;
                        case '%': addBufferAndToken(TokenType.Percent); break;
                        case '=': addBufferAndToken(TokenType.Equal); break;
                        case '"':
                            isQuote = true;
                            addBuffer();
                            break;
                        default:
                            pushBuffer(current);
                            break;
                    }
                }
                else
                {
                    switch (current)
                    {
                        case '"':
                            isQuote = false;
                            addBuffer(true);
                            break;
                        case '\n':
                            throw new ParserException("Unexpected EOL");
                        default:
                            pushBuffer(current);
                            break;
                    }
                }
            }
            if (isQuote) throw new ParserException("Unexpected EOF");
            addBuffer();

            return tokens;
        }
    }
}
