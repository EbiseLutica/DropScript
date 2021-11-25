namespace DropScript.Parsing
{
    public class EscapeSequenceNode : NodeBase
    {
        public char EscapeChar { get; set; }
        public int? Argument { get; set; }
    }


}