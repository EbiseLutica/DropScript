namespace DropScript.Parsing
{
    public abstract class ValueNodeBase : NodeBase, IExpressionNode
    {
        public string Value { get; set; } = "";
        public ValueNodeBase(string value)
        {
            Value = value;
        }
    }
}