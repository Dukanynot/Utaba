
namespace Utaba.Interfaces
{
    interface IChessMoveStrategy
    {
        IMoveResponse HandleMove(IPiece piece, ISquare srcSquare, ISquare destSquare);
    }
}
