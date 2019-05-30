using System;
using Utaba.Implementations.Pieces;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.ObjectFactories
{
    class BishopFactory : IChessPieceFactory
    {
        private static readonly Lazy<BishopFactory> LazyBishopFactory = new Lazy<BishopFactory>(() => new BishopFactory());
        private BishopFactory() { }

        public static BishopFactory Singleton = LazyBishopFactory.Value;
        public IPiece GetChessPiece(ISquare square, Teams team)
        {
            return new Bishop(square, team);
        }
    }
}
