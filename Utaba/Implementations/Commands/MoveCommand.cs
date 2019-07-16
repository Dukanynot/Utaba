using Utaba.Implementations.Proxies;
using Utaba.Interfaces;

namespace Utaba.Implementations.Commands
{
    class MoveCommand : IChessCommand
    {
        private readonly IPiece _primaryPiece;
        private readonly ISquare _destSquare;

        public MoveCommand(IPiece primaryPiece, ISquare destSquare)
        {
            _primaryPiece = primaryPiece;
            _destSquare = destSquare;
        }

        public IMoveResponse Execute()
        {
            ChessBoardProxy.Singleton.RemoveEnpassant(_primaryPiece);
            return _primaryPiece.Move(_destSquare);
        }
    }
}
