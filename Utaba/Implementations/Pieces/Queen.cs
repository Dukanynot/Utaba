using System;
using Utaba.Interfaces;

namespace Utaba.Implementations.Pieces
{
    internal class Queen : ChessPiece
    {
        public Queen(ISquare location, Teams team)
            :base(team, location)
        {
            whoAmI = PieceType.Queen;
        }

        public override IMoveResponse Move(ISquare destSquare, CommandType cmdType)
        {
            throw new NotImplementedException();
        }

    }
}
