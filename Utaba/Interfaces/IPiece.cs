
namespace Utaba.Interfaces
{
    /// <summary>
    /// Implemented by any Chess piece on the Chess board
    /// </summary>
    public interface IPiece
    {
        IMoveResponse Move(ISquare destSquare, CommandType cmdType);

        Teams MyTeam { get;}

        ISquare MyLocation { get; set; }

        PieceStatus MyStatus { get; set; }

        PieceType WhoAmI { get;}
    }
}
