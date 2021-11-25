using System.Collections.Generic;

namespace DropScript.Parsing
{
    public class CommandNode : NodeBase, IStatementNode
    {
        public CommandNode(string commandName)
        {
            CommandName = commandName;
            Arguments = new();
        }
        public CommandNode(string commandName, List<IExpressionNode> arguments)
        {
            CommandName = commandName;
            Arguments = arguments;
        }

        public string CommandName { get; set; }
        public List<IExpressionNode> Arguments { get; set; }
    }
}