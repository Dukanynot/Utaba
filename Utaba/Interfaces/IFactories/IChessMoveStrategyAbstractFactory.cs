
namespace Utaba.Interfaces.IFactories
{
    interface IChessMoveStrategyAbstractFactory
    {
        IHandleChessCommand GetChessMoveStrategy(IPiece piece,ISquare destSquare);
    }
}
