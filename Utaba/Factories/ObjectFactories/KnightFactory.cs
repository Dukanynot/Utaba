using System;
using Utaba.Implementations.Pieces;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.ObjectFactories
{
    class KnightFactory : IChessPieceFactory
    {
        private static readonly Lazy<KnightFactory> _knightFactory = new Lazy<KnightFactory>(() => new KnightFactory());
        private KnightFactory() { }

        public static KnightFactory Singleton = _knightFactory.Value;
        public IPiece GetChessPiece(ISquare square, Teams team)
        {
            return new Knight(square, team);
        }
    }
}
