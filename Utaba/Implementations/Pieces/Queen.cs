using System;
using Utaba.Interfaces;

namespace Utaba.Implementations.Pieces
{
    internal class Queen : ChessPiece
    {
        public Queen(ISquare location, Teams team)
            :base(team, location)
        {
            _whoAmI = PieceType.Queen;
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

    }
}
