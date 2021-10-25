using System.Security.AccessControl;
namespace DropScript.Parsing
{
    /// <summary>
    /// DropScript 字句を表します。
    /// </summary>
    /// <param name="type">字句のタイプ。</param>
    /// <param name="value">字句の実際の値。</param>
    public record Token(TokenType type, string? value);
}
