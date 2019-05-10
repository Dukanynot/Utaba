

namespace Utaba.Interfaces.IFactories
{
    interface IChessPieceFactory
    {
        IPiece GetChessPiece(ISquare location, Teams team);
    }
}
