using System;
using Utaba.Implementations.Strategies.ValidSquareStrategies;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.AbstractFactories
{
    /// <summary>
    /// Abstract factory for returning valid squares strategies
    /// </summary>
    class ValidSquaresStrategyAbstractFactory : IValidSquaresStrategyAbstractFactory
    {
        private static readonly Lazy<ValidSquaresStrategyAbstractFactory> LazyValidSquaresStrategyAbstractFactory = new Lazy<ValidSquaresStrategyAbstractFactory>(() =>
            new ValidSquaresStrategyAbstractFactory());
        private ValidSquaresStrategyAbstractFactory() { }

        public static ValidSquaresStrategyAbstractFactory Singleton => LazyValidSquaresStrategyAbstractFactory.Value;

        public IValidSquaresStrategy GetValidSquaresStrategy(IPiece piece)
        {
            // TODO: move these into factories
            IValidSquaresStrategy strategy = null;
            switch (piece.WhoAmI)
            {
                case PieceType.King:
                    break;
                case PieceType.Queen:
                    break;
                case PieceType.Bishop:
                    break;
                case PieceType.Knight:
                    break;
                case PieceType.Rook:
                    break;
                case PieceType.Pawn:
                    strategy = ValidPawnSquaresStrategy.Singleton;
                    break;
            }
            return strategy;
        }
    }
}
