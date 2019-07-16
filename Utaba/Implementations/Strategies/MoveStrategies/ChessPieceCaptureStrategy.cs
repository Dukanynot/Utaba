using Utaba.Implementations.Pieces;
using Utaba.Implementations.Proxies;
using Utaba.Interfaces;

namespace Utaba.Implementations.Strategies.MoveStrategies
{
    sealed class ChessPieceCaptureStrategy : IHandleChessCommand
    {
        private readonly IPiece _attacker;
        private readonly IPiece _captured;
        private readonly ISquare _capturedSquare;
        private readonly IChessBoardProxy _chessBoardProxy;

        public ChessPieceCaptureStrategy(IPiece attacker, IPiece captured, ISquare capturedSquare)
        {
            _chessBoardProxy = ChessBoardProxy.Singleton;
            _attacker = attacker;
            _captured = captured;
            _capturedSquare = capturedSquare;
        }
        public IMoveResponse HandleCommand()
        {
            var imr = new MoveResponse();
            if (_chessBoardProxy.IsItAValidSquare(_attacker,_capturedSquare))
            {
                _attacker.MyLocation.Occupied = false;
                _attacker.MyLocation = _capturedSquare;
                _attacker.MyLocation.Occupied = true;

                _captured.MyStatus = PieceStatus.Captured;

                //  handles en passant captures
                if (_capturedSquare != _captured.MyLocation)
                {
                    _captured.MyLocation.Occupied = false;
                }
                _captured.MyLocation = null;

                imr.Message = $"{_captured.MyTeam} {_captured.WhoAmI} was captured";
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
