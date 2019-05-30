using Utaba.Implementations.Strategies.MoveStrategies;
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

        public override IMoveResponse Move(ISquare destSquare, CommandType cmdType)
        {
            return PawnMoveStrategy.Singleton.HandleMove(this, this.Location, destSquare);
        }

        public bool HasMoved { get; set; }
    }
}
