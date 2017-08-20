using System;
using Utaba.Interfaces;

namespace Utaba.Implementations.Pieces
{
    internal class King : ChessPiece
    {
        public King(ISquare location, Teams team)
            :base(team, location)
        {
            _whoAmI = PieceType.King;
        }

        public override void Move()
        {
            throw new NotImplementedException();
        }

        public bool HasMoved { get; set; }
    }
}
