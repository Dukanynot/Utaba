using System;
using Utaba.Implementations.Pieces;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.ObjectFactories
{
    class QueenFactory : IChessPieceFactory
    {
        private static readonly Lazy<QueenFactory> _queenFactory = new Lazy<QueenFactory>(() => new QueenFactory());
        private QueenFactory() { }

        public static QueenFactory Singleton = _queenFactory.Value;
        public IPiece GetChessPiece(ISquare square, Teams team)
        {
            return new Queen(square, team);
        }
    }
}
