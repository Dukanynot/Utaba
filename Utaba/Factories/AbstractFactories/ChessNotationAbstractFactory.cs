using System;
using System.Text.RegularExpressions;
using Utaba.Implementations.Strategies.ChessNotationStrategies;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.AbstractFactories
{
    class ChessNotationAbstractFactory : IChessNotationStrategyAbstractFactory
    {
        private const string MovePattern = "^(([RNBQK]([a-h]|[1-8]){0,1})*[a-h][1-8])$";
        private const string CapturePattern = "^((([RNBQK]([a-h]|[1-8]){0,1})*|[a-h]){0,1}x[a-h][1-8])$";

        private static readonly Lazy<ChessNotationAbstractFactory> LazyChessNotationAbstractFactory = 
            new Lazy<ChessNotationAbstractFactory>(() => new ChessNotationAbstractFactory());

        private ChessNotationAbstractFactory() { }

        public static readonly ChessNotationAbstractFactory Singleton = LazyChessNotationAbstractFactory.Value;
        public IChessNotationStrategy GetChessNotationStrategy(string notation)
        {
            IChessNotationStrategy strategy = null;
            // TODO: Move these to Factories
            if (Regex.IsMatch(notation,MovePattern))
            {
                strategy = new ChessNotationMoveStrategy();
            }
            else if (Regex.IsMatch(notation,CapturePattern))
            {
                strategy = new ChessNotationCaptureStrategy();
            }
            // TODO: Castling and Promotion strategy

            return strategy;
        }
    }
}
