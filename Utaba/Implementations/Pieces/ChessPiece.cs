using Utaba.Interfaces;

namespace Utaba.Implementations.Pieces
{
    internal abstract class ChessPiece : IPiece
    {
        protected ISquare _location;
        protected readonly Teams _team;
        protected PieceStatus _status;
        protected PieceType _whoAmI;

        protected ChessPiece(Teams team, ISquare location)
        {
            _team = team;
            _location = location;
            _status = PieceStatus.Active;
        }
        // each piece should implement its move
        public abstract void Move();

        #region Properties

        public PieceType WhoAmI => _whoAmI;

        public ISquare MyLocation
        {
            get => _location;
            set => _location = value;
        }

        public Teams MyTeam => _team;

        public PieceStatus MyStatus
        {
            get => _status;
            set => _status = value;
        }
        #endregion
    }
}
