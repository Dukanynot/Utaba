using System;
using Utaba.Interfaces;

namespace Utaba.Implementations.Pieces
{
    internal class King : ChessPiece
    {
        public King(ISquare location, Teams team)
            :base(team, location)
        {
            whoAmI = PieceType.King;
        }

        public override IMoveResponse Move(ISquare destSquare, CommandType cmdType)
        {
            throw new NotImplementedException();
        }

        public bool HasMoved { get; set; }
    }
}
