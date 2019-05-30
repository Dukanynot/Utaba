using System;
using Utaba.Implementations.Pieces;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.ObjectFactories
{
    class KingFactory : IChessPieceFactory
    {
        private static readonly Lazy<KingFactory> LazyKingFactory = new Lazy<KingFactory>(() => new KingFactory());
        private KingFactory() { }

        public static KingFactory Singleton = LazyKingFactory.Value;
        public IPiece GetChessPiece(ISquare square, Teams team)
        {
            return new King(square, team);
        }
    }
}
