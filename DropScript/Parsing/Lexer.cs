using System.Collections.Generic;
using System;
using System.Collections;

namespace DropScript.Parsing
{
    /// <summary>
    /// DropScriptの字句解析器。
    /// </summary>
    public sealed class Lexer : IEnumerable<Token>
    {
        private Lexer(List<Token> tokens) { this.tokens = tokens; }

        public static Lexer Analyze(string script)
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
                    addToken(TokenType.STRING, buffer);
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
                        case ' ': addBufferAndToken(TokenType.WHITE_SPACE); break;
                        case '+': addBufferAndToken(TokenType.PLUS); break;
                        case '\n': addBufferAndToken(TokenType.NEW_LINE); break;
                        case '\\':
                            addBufferAndToken(TokenType.BACKSLASH);
                            i++;
                            if (i >= script.Length) throw new ParserException("Unexpected EOF");
                            addToken(TokenType.STRING, script[i].ToString());
                            if (script[i] == 'C')
                            {
                                i++;
                                if (i >= script.Length) throw new ParserException("Unexpected EOF");
                                addToken(TokenType.STRING, script[i].ToString());
                            }
                            break;
                        case '$': addBufferAndToken(TokenType.DOLLAR); break;
                        case '{': addBufferAndToken(TokenType.LEFT_CURLY_BRACE); break;
                        case '}': addBufferAndToken(TokenType.RIGHT_CURLY_BRACE); break;
                        case ',': addBufferAndToken(TokenType.COMMA); break;
                        case '@': addBufferAndToken(TokenType.AT); break;
                        case '%': addBufferAndToken(TokenType.PERCENT); break;
                        case '=': addBufferAndToken(TokenType.EQUAL); break;
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

            return new Lexer(tokens);
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return tokens.GetEnumerator();
        }

        private List<Token> tokens;
    }

    public class ParserException : System.Exception
    {
        public ParserException() { }
        public ParserException(string message) : base(message) { }
        public ParserException(string message, System.Exception inner) : base(message, inner) { }
        public ParserException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
