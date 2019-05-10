
namespace Utaba.Interfaces.IFactories
{
    interface IChessMoveStrategyAbstractFactory
    {
        IChessMoveStrategy GetChessMoveStrategy(IPiece piece);
    }
}
