using System;
using Utaba.Interfaces;

namespace Utaba.Implementations.Pieces
{
    internal class Rook : ChessPiece
    {
        public Rook(ISquare location, Teams team)
            : base(team, location)
        {
            whoAmI = PieceType.Rook;
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public bool HasMoved{get; set;}
    }
}
