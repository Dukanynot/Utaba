using System;
using Utaba.Implementations.Pieces;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.ObjectFactories
{
    class PawnFactory : IChessPieceFactory
    {
        private static readonly Lazy<PawnFactory> LazyPawnFactory = new Lazy<PawnFactory>(() => new PawnFactory());
        private PawnFactory() { }

        public static PawnFactory Singleton = LazyPawnFactory.Value;
        public IPiece GetChessPiece(ISquare square, Teams team)
        {
            return new Pawn(square, team);
        }
    }
}
