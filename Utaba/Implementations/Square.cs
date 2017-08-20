using Utaba.Interfaces;

namespace Utaba.Implementations
{
    internal class Square : ISquare
    {
        readonly private byte _columnIndex;
        readonly private byte _row;
        readonly private SquareColors _myColor;
        private bool _occupied;
        private static readonly char[] ColumnLetters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

        public Square(byte columnIndex, byte row, SquareColors myColor)
        {
            _columnIndex = columnIndex;
            _row = row;
            _myColor = myColor;
            _occupied = false;            
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

        public bool Occupied
        {
            get { return _occupied; }
            set { _occupied = value; }
        }
        public char ColumnLetter
        {
            get { return ColumnCharByIndex(_columnIndex); }
        }
        public byte ColumnIndex
        {
            get { return _columnIndex; }
        }

        public SquareColors MyColor
        {
            get { return _myColor; }
        }

        public byte RowIndex
        {
            get { return _row; }
        }
        #endregion
    }
}
