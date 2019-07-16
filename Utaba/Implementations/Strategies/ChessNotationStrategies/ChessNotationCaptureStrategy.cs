using System;
using System.Linq;
using Utaba.Implementations.Commands;
using Utaba.Implementations.Pieces;
using Utaba.Implementations.Proxies;
using Utaba.Interfaces;

namespace Utaba.Implementations.Strategies.ChessNotationStrategies
{
    class ChessNotationCaptureStrategy : IChessNotationStrategy
    {
        private const string ColumnLetters = "abcdefgh";
        private const string OfficeLetters = "RNBQK";

        public ChessNotationCaptureStrategy(string notation)
        {
            Notation = notation;
        }
        public IChessCommand CreateCommand(Teams whosMove)
        {
            IChessCommand command = null;
            Teams enemyColor = whosMove == Teams.White ? Teams.Black : Teams.White;

            // separate the notation by attacker and the square getting attacked
            var separate = Notation.Split(new[] {'x'}, StringSplitOptions.RemoveEmptyEntries);

            // check if pawn making a capture e.g axb4
            if (separate[0].Length == 1 && ColumnLetters.Contains(separate[0][0]))
            {
                var location = separate[1];
                var destinationRow = int.Parse(separate[1].Substring(1,1));
                var destSquare =
                    ChessBoardProxy.Singleton.GetSquares(
                        s => s.ColumnLetter == location[0] && s.RowIndex  == destinationRow - 1).Single();

                var attacker = ChessBoardProxy.Singleton.GetPawnThatCanAttack(destSquare, whosMove, separate[0][0]);

                if (attacker != null)
                {
                    var capturedPiece = ChessBoardProxy.Singleton.WhosOnThisSquare(destSquare, enemyColor) ??
                                        ChessBoardProxy.Singleton.GetPieces(p => p.WhoAmI == PieceType.Pawn &&  ((Pawn)p).EnPassantSquare == destSquare && p.MyTeam == enemyColor)
                                            .SingleOrDefault();
                    if (capturedPiece != null)
                    {
                        command = new CaptureCommand(attacker, capturedPiece,destSquare);
                    }
                }
            }
            return command;
        }

        public string Notation { get; set; }
    }
}
