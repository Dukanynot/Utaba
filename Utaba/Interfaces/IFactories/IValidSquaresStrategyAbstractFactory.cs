namespace Utaba.Interfaces.IFactories
{
    interface IValidSquaresStrategyAbstractFactory
    {
        IValidSquaresStrategy GetValidSquaresStrategy(IPiece piece);
    }
}