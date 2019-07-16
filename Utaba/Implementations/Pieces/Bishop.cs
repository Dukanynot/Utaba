using System;
using Utaba.Interfaces;

namespace Utaba.Implementations.Pieces
{
    internal class Bishop : ChessPiece
    {
        public Bishop(ISquare location, Teams team)
            :base(team, location)
        {
            whoAmI = PieceType.Bishop;
        }

        public override IMoveResponse Move(ISquare destSquare)
        {
            throw new NotImplementedException();
        }
    }
}
