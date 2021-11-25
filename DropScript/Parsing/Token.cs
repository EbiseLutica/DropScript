using System.Security.AccessControl;
namespace DropScript.Parsing
{
    /// <summary>
    /// DropScript 字句を表します。
    /// </summary>
    /// <param name="Type">字句のタイプ。</param>
    /// <param name="Value">字句の実際の値。</param>
    public record Token(TokenType Type, string? Value);
}
