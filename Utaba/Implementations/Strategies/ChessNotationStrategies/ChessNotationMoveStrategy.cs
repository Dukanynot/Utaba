using System.Linq;
using Utaba.Implementations.Commands;
using Utaba.Implementations.Proxies;
using Utaba.Interfaces;

namespace Utaba.Implementations.Strategies.ChessNotationStrategies
{
    class ChessNotationMoveStrategy : IChessNotationStrategy
    {
        public ChessNotationMoveStrategy(string notation)
        {
            Notation = notation;
        }
        public IChessCommand CreateCommand(Teams whosMove)
        {
            IChessCommand command = null;

            // pawn moves = 2
            if (Notation.Length ==  2)
            {
                int destinationColumn = Square.ColumnIndexByChar(Notation[0]);
                int destinationRow = int.Parse(Notation.Substring(1, 1));

                // get the actual square this string destination represents
                var destSquare = ChessBoardProxy.Singleton.GetSquares(s => s.ColumnIndex == destinationColumn
                                                 && s.RowIndex == destinationRow - 1).Single();
               var piece = ChessBoardProxy.Singleton.GetPieceForThisSquare(destSquare, whosMove, PieceType.Pawn);

                // TODO Factory pattern maybe????
                command = new MoveCommand(piece,destSquare);
            }
            return command;
        }


        public string Notation { get; set; }
    }
}
