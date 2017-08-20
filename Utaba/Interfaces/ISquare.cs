
namespace Utaba.Interfaces
{
    /// <summary>
    /// Implemented by every square on the Chess Board
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
