using System;
using Utaba.Implementations.Pieces;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.ObjectFactories
{
    class BishopFactory : IChessPieceFactory
    {
        private static readonly Lazy<BishopFactory> _bishopFactory = new Lazy<BishopFactory>(() => new BishopFactory());
        private BishopFactory() { }

        public static BishopFactory Singleton = _bishopFactory.Value;
        public IPiece GetChessPiece(ISquare square, Teams team)
        {
            return new Bishop(square, team);
        }
    }
}
