namespace Utaba
{
    /// <summary>
    /// The two sides in a Chess Game
    /// </summary>
    public enum Teams : byte
    {
        White = 0,
        Black = 1
    }

    /// <summary>
    /// The two shades of color on a Chess board
    /// </summary>
    public enum SquareColors : byte
    {
        Light = 0,
        Dark = 1
    }

    public enum PieceStatus : byte
    {
        Captured = 0,
        Active = 1
    }

    public enum PieceType : byte
    {
        King = 0,
        Queen,
        Bishop,
        Knight,
        Rook,
        Pawn
    }
}
