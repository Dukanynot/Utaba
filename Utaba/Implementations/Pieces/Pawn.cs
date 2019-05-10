using System;
using Utaba.Interfaces;

namespace Utaba.Implementations.Pieces
{
    internal class Pawn : ChessPiece
    {
        public Pawn(ISquare location, Teams team)
            :base(team, location)
        {
            whoAmI = PieceType.Pawn;
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public bool HasMoved { get; set; }
    }
}
