using System;
using System.Collections.Generic;
using System.Linq;

namespace DropScript.Parsing
{
    public static class Parser
    {
        public static StatementsNode Parse(string script)
        {
            return Parse(Lexer.Analyze(script));
        }

        public static StatementsNode Parse(List<Token> tokens)
        {
            var reader = new ListReader<Token>(tokens);

            return ReadAsStatements(reader);
        }

        private static StatementsNode ReadAsStatements(ListReader<Token> reader)
        {
            var nodes = new List<IStatementNode>();
            while (reader.Current != null)
            {
                nodes.Add(ReadAsStatement(reader));
            }
            return new StatementsNode(nodes);
        }

        private static IStatementNode ReadAsStatement(ListReader<Token> reader)
        {
            switch (reader.Current?.Type)
            {
                // Command
                case TokenType.Plus:
                {
                    var name = reader.Next();
                    name.Assert(TokenType.String);

                    reader.SkipWhiteSpace();
                    var delimiter = reader.Next();
                    var nameValue = name?.Value ?? throw new ParserException("BUG: name is unexpectly null.");
                    if (delimiter.Is(TokenType.Newline)) return new CommandNode(nameValue);
                    var parameters = new List<IExpressionNode>();
                    while (!reader.Current.IsEofOr(TokenType.Newline))
                    {
                        reader.SkipWhiteSpace();
                        switch (reader.Current.Type)
                        {
                            case TokenType.DollarSign:
                                // 変数
                                reader.Next().Assert(TokenType.LeftCurlyBrace);
                                var identifier = reader.Next().Assert(TokenType.String);
                                reader.Next().Assert(TokenType.RightCurlyBrace);
                                parameters.Add(new IdentifierNode(identifier?.Value ?? throw new ParserException("BUG: identifier is unexpectly null.")));
                                break;
                            case TokenType.String:
                                parameters.Add(new StringNode(reader.Current.Value ?? throw new ParserException("BUG: The string value is unexpectly null.")));
                                break;
                        }
                        if (reader.Next().Is(TokenType.Comma))
                        {
                            reader.Next();
                        }
                    }
                    return new CommandNode(nameValue, parameters);
                }
                default:
                    throw new NotImplementedException();
            }
        }

        private static void SkipWhiteSpace(this ListReader<Token> reader)
        {
            while (IsEofOr(reader.Current, TokenType.WhiteSpace))
            {
                reader.Next();
            }
        }

        private static Token? Assert(this Token? token, params TokenType[] expectedTypes)
        {
            if (!Is(token, expectedTypes))
            {
                throw new ParserException($"Unexpected token ${token?.Type.ToString() ?? "null"}. ${string.Join(", ", expectedTypes)} expected.");
            }
            return token;
        }

        private static Token? AssertWithEof(this Token? token, params TokenType[] expectedTypes)
        {
            if (!Is(token, expectedTypes))
            {
                throw new ParserException($"Unexpected token ${token?.Type.ToString() ?? "null"}. ${string.Join(", ", expectedTypes)}, or EOF expected.");
            }
            return token;
        }

        private static bool IsEofOr(this Token? token, params TokenType[] expectedTypes)
        {
            if (token == null) return true;

            return expectedTypes.Contains(token.Type);
        }

        private static bool Is(this Token? token, params TokenType[] expectedTypes)
        {
            if (token == null) return false;

            return expectedTypes.Contains(token.Type);
        }
    }

    public class StatementsNode : NodeBase
    {
        public StatementsNode(List<IStatementNode> children) => Children = children;
        public List<IStatementNode> Children { get; set; }
    }


}