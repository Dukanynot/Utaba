
namespace Utaba.Interfaces.IFactories
{
    interface IChessPieceAbstractFactory
    {
        IPiece GetChessPiece(Teams team, ISquare square, PieceType pieceType);
    }
}
