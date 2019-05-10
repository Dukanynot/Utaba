using System;
using Utaba.Implementations.Strategies.MoveStrategies;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.AbstractFactories
{
    class ChessMoveStrategyAbstractFactory : IChessMoveStrategyAbstractFactory
    {
        private static readonly Lazy<ChessMoveStrategyAbstractFactory> LazyChessMoveStrategyAbstractFactory = 
            new Lazy<ChessMoveStrategyAbstractFactory>(() => new ChessMoveStrategyAbstractFactory());

        private ChessMoveStrategyAbstractFactory(){}

        public static ChessMoveStrategyAbstractFactory Singleton => LazyChessMoveStrategyAbstractFactory.Value;

        public IChessMoveStrategy GetChessMoveStrategy(IPiece piece)
        {
            IChessMoveStrategy moveStrategy = null;
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
                    moveStrategy = PawnMoveStrategy.Instance;
                    break;
            }
            return moveStrategy;
        }
    }
}
