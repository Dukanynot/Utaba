using System;
using Utaba.Implementations.Pieces;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.ObjectFactories
{
    class KnightFactory : IChessPieceFactory
    {
        private static readonly Lazy<KnightFactory> LazyKnightFactory = new Lazy<KnightFactory>(() => new KnightFactory());
        private KnightFactory() { }

        public static KnightFactory Singleton = LazyKnightFactory.Value;
        public IPiece GetChessPiece(ISquare square, Teams team)
        {
            return new Knight(square, team);
        }
    }
}
