using System;
using Utaba.Implementations.Pieces;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.ObjectFactories
{
    class RookFactory : IChessPieceFactory
    {
        private static readonly Lazy<RookFactory> _rookFactory = new Lazy<RookFactory>(() => new RookFactory());
        private RookFactory() { }

        public static RookFactory Singleton = _rookFactory.Value;
        public IPiece GetChessPiece(ISquare square, Teams team)
        {
            return new Rook(square, team);
        }
    }
}
