using System;
using Utaba.Interfaces;

namespace Utaba.Implementations.Pieces
{
    internal class Knight : ChessPiece
    {
        public Knight(ISquare location, Teams team)
            :base(team, location)
        {
            whoAmI = PieceType.Knight;
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }
    }
}
