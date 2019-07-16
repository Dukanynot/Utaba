using Utaba.Implementations.Proxies;
using Utaba.Interfaces;

namespace Utaba.Implementations.Strategies.MoveStrategies
{
    sealed class ChessPieceMoveStrategy : IHandleChessCommand
    {
        private readonly IChessBoardProxy _chessBoardProxy;
        private readonly IPiece _piece;
        private readonly ISquare _destSquare;

        public ChessPieceMoveStrategy(IPiece piece, ISquare destSquare)
        {
            _chessBoardProxy = ChessBoardProxy.Singleton;
            _piece = piece;
            _destSquare = destSquare;
        }
        public IMoveResponse HandleCommand()
        {
            var imr = new MoveResponse();
            if (_chessBoardProxy.IsItAValidSquare(_piece, _destSquare))
            {
                _piece.MyLocation.Occupied = false;
                _piece.MyLocation = _destSquare;
                _piece.MyLocation.Occupied = true;
                imr.Message = $"{_piece.MyTeam} {_piece.WhoAmI} to {_destSquare.ColumnLetter}{_destSquare.RowIndex + 1} move was successful";
                imr.SuccessfulMove = true;
            }
            else
            {
                imr.Message = "This move is not a valid move";
                imr.SuccessfulMove = false;
            }
            return imr;
        }
    }
}
