using System.Linq;
using Utaba.Implementations.Commands;
using Utaba.Implementations.Proxies;
using Utaba.Interfaces;

namespace Utaba.Implementations.Strategies.ChessNotationStrategies
{
    class ChessNotationMoveStrategy : IChessNotationStrategy
    {
        public IChessCommand CreateCommand(string notation, Teams whosMove)
        {
            IChessCommand command = null;

            // pawn moves = 2
            if (notation.Length ==  2)
            {
                int destinationColumn = Square.ColumnIndexByChar(notation[0]);
                int destinationRow = int.Parse(notation.Substring(1, 1));

                // get the actual square this string destination represents
                var destSquare = ChessBoardProxy.Singleton.GetSquares(s => s.ColumnIndex == destinationColumn
                                                 && s.RowIndex == destinationRow - 1).Single();
               var piece = ChessBoardProxy.Singleton.GetPieceForThisSquare(destSquare, whosMove, PieceType.Pawn);

                // TODO Abstarct Factory pattern
                command = new MoveCommand(piece,destSquare);
            }

            return command;


        }


    }
}
