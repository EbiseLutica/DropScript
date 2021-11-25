using System.Collections.Generic;
namespace DropScript.Parsing
{
    /// <summary>
    /// リストを1つずつ読み取る機能と、巻き戻し機能を提供します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListReader<T>
    {
        /// <summary>
        /// 現在の要素を取得します。
        /// </summary>
        /// <value>現在のポインターが指し示す要素。存在しなければ <see langword="null"/>。</value>
        public T? Current => pointer < List.Count ? List[pointer] : default;

        /// <summary>
        /// 対象とするリストを取得します。
        /// </summary>
        public List<T> List { get; }

        /// <summary>
        /// <see cref="ListReader"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="list">対象とするリスト。</param>
        public ListReader(List<T> list)
        {
            List = list;
        }

        /// <summary>
        /// ポインターを次に進めます。
        /// </summary>
        public T? Next()
        {
            if (pointer < List.Count)
            {
                pointer++;
            }
            return Current;
        }

        /// <summary>
        /// ポインターを前に戻します。
        /// </summary>
        /// <returns>戻した後のポインターが指し示す要素。存在しなければ <see langword="null"/>。</returns>
        public T? Previous()
        {
            if (0 < pointer)
            {
                pointer--;
            }
            return Current;
        }

        /// <summary>
        /// ポインターをコミットします。次に <see cref="Rollback"/> メソッドを呼び出すときにこのポインターまで戻します。
        /// </summary>
        public void Commit()
        {
            committedPointer = pointer;
        }

        /// <summary>
        /// 前回コミットしたポインターまで戻します。
        /// </summary>
        public void Rollback()
        {
            committedPointer = pointer;
        }

        private int pointer;
        private int committedPointer;
    }
}
