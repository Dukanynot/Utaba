using Utaba.Implementations.Proxies;
using Utaba.Interfaces;

namespace Utaba.Implementations.Commands
{
    class CaptureCommand : IChessCommand
    {
        private readonly IPiece _attackerPiece;
        private readonly IPiece _capturedPiece;
        private readonly ISquare _capturedSquare;

        public CaptureCommand(IPiece attackerPiece, IPiece capturedPiece, ISquare capturedSquare)
        {
            _attackerPiece = attackerPiece;
            _capturedPiece = capturedPiece;
            _capturedSquare = capturedSquare;
        }
        public IMoveResponse Execute()
        {
            var imr =  _attackerPiece.Capture(_capturedPiece,_capturedSquare);
            ChessBoardProxy.Singleton.RemoveEnpassant(_attackerPiece);
            return imr;
        }
        // TODO Create Memento pattern and unexecute
    }
}
