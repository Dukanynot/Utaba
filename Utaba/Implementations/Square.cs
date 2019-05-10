using Utaba.Interfaces;

namespace Utaba.Implementations
{
    /// <summary>
    /// An instance of the square on a chess board
    /// </summary>
    internal class Square : ISquare
    {
        private static readonly char[] ColumnLetters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

        public Square(byte columnIndex, byte row, SquareColors myColor)
        {
            ColumnIndex = columnIndex;
            RowIndex = row;
            MyColor = myColor;
            Occupied = false;            
        }

        public static int ColumnIndexByChar(char chr)
        {
            int idx = -1;
            bool isFound = false;
            for (int i = 0; i < ColumnLetters.Length && !isFound; i++)
            {
                if (ColumnLetters[i] == chr)
                {
                    idx = i;
                    isFound = true;
                }
            }
            return idx;
        }

        public static char ColumnCharByIndex(int index)
        {
            return ColumnLetters[index];
        }
        #region Properties

        public bool Occupied { get; set; }

        public char ColumnLetter => ColumnCharByIndex(ColumnIndex);

        public byte ColumnIndex { get; }

        public SquareColors MyColor { get; }

        public byte RowIndex { get; }

        #endregion
    }
}
