using System.Collections.Generic;

namespace DropScript.Parsing
{
    public class MessageNode : NodeBase, IStatementNode
    {
        public MessageNode(List<IExpressionNode> children)
        {
            Children = children;
        }

        public List<IExpressionNode> Children { get; set; }
    }
}