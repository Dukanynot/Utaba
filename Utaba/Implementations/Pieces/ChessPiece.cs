using Utaba.Implementations.Strategies.MoveStrategies;
using Utaba.Interfaces;

namespace Utaba.Implementations.Pieces
{
    internal abstract class ChessPiece : IPiece
    {
        protected ISquare Location;
        protected readonly Teams Team;
        protected PieceStatus Status;
        protected PieceType whoAmI;

        protected ChessPiece(Teams team, ISquare location)
        {
            Team = team;
            Location = location;
            Status = PieceStatus.Active;
        }

        public virtual IMoveResponse Move(ISquare destSquare)
        {
            var strategy = new ChessPieceMoveStrategy(this,destSquare);
            return strategy.HandleCommand();
        }

        public virtual IMoveResponse Capture(IPiece capturedPiece, ISquare capturedSquare)
        {
            var strategy = new ChessPieceCaptureStrategy(this, capturedPiece,capturedSquare);
            return strategy.HandleCommand();
        }

        #region Properties

        public PieceType WhoAmI => whoAmI;

        public ISquare MyLocation
        {
            get => Location;
            set => Location = value;
        }

        public Teams MyTeam => Team;

        public PieceStatus MyStatus
        {
            get => Status;
            set => Status = value;
        }
        #endregion
    }
}
