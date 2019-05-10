
namespace Utaba.Interfaces
{
    /// <summary>
    /// Implemented by every square on the Chess ChessBoard
    /// </summary>
    public interface ISquare
    {
        bool Occupied { get; set; }
        SquareColors MyColor { get;}

        char ColumnLetter { get; }
        byte ColumnIndex { get; }
        byte RowIndex { get; }

    }
}
