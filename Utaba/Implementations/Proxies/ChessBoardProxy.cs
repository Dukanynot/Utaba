using System;
using System.Collections.Generic;
using System.Linq;
using Utaba.Factories.AbstractFactories;
using Utaba.Implementations.Pieces;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Implementations.Proxies
{
    /// <summary>
    /// Provides methods to interface with the ChessBoard
    /// </summary>
    class ChessBoardProxy : IChessBoardProxy
    {
        private readonly IValidSquaresStrategyAbstractFactory _validSquaresStrategyAbstractFactory;
        private readonly ChessBoard _chessBoard;
        private static readonly Lazy<ChessBoardProxy> LazyChessBoardProxy = new Lazy<ChessBoardProxy>(() => 
            new ChessBoardProxy(ChessBoard.Instance, ValidSquaresStrategyAbstractFactory.Singleton));

        private ChessBoardProxy(ChessBoard chessBoard, IValidSquaresStrategyAbstractFactory validSquaresStrategyAbstractFactory)
        {
            _chessBoard = chessBoard;
            _validSquaresStrategyAbstractFactory = validSquaresStrategyAbstractFactory;
        }

        public static ChessBoardProxy Singleton => LazyChessBoardProxy.Value;

        /// <summary>
        /// Checks to see whether the specified team's King is in Check
        /// </summary>
        /// <param name="team">The team to check for check :)</param>
        /// <returns></returns>
        public bool IsKingInCheck(Teams team)
        {
            // get the King's current location
            var kingLocation = GetPieces(p => p.WhoAmI == PieceType.King
                                              && p.MyTeam == team).Single().MyLocation;

            // can an enemy pawn attack this square?
            var isInCheck = CanAPawnAttackSquare(kingLocation, team);

            // if no pawn can attack..check for a Rook
            if (!isInCheck)
            {
                isInCheck = CanARookAttackSquare(kingLocation, team);
            }
            if (!isInCheck) // if no rook then check for Knight
            {
                isInCheck = CanAKnightAttackSquare(kingLocation, team);
            }
            if (!isInCheck) // if no knight can attack then check for bishop
            {
                isInCheck = CanABishopAttackSquare(kingLocation, team);
            }
            if (!isInCheck) // if no bishop can attack then check for queen
            {
                isInCheck = CanAQueenAttackSquare(kingLocation, team);
            }
            if (!isInCheck) // if no queen can attack then check for king
            {
                isInCheck = CanAKingAttackSquare(kingLocation, team);
            }

            return isInCheck;
        }

        private bool CanAKingAttackSquare(ISquare square, Teams team)
        {
            Teams enemyColor = team == Teams.White ? Teams.Black : Teams.White;


            // get all the squares around this square that a king can attack from
            // gets all the diagonal squares to the specified square
            // gets all adjacent squares
            var listOfSquares = GetSquares(s =>
                                           (Math.Abs(s.ColumnIndex - square.ColumnIndex) == 1 &&
                                           Math.Abs(s.RowIndex - square.RowIndex) == 1)
                                           ||
                                           (s.ColumnIndex == square.ColumnIndex && Math.Abs(s.RowIndex - square.RowIndex) == 1)
                                           ||
                                           (s.RowIndex == square.RowIndex && Math.Abs(s.ColumnIndex - square.ColumnIndex) == 1)
                                           );

            // get the enemy king on this square...should be only one or none
            var canAttack = GetPieces(p => p.WhoAmI == PieceType.King &&
                                            p.MyTeam == enemyColor &&
                                            listOfSquares.Contains(p.MyLocation)).Any();


            return canAttack;
        }

        private bool CanAQueenAttackSquare(ISquare square, Teams team)
        {
            // A queen's move is a cross between a bishop and a rook.
            var canAttack = CanADiagonalAttack(square, team, PieceType.Queen);

            // if the queen is not able to attack as a bishop...then check to see if it can attack as a rook
            if (!canAttack)
            {
                canAttack = CanAStraightPieceAttack(square, team, PieceType.Queen);
            }

            return canAttack;
        }

        /// <summary>
        /// Returns a list of squares that a king can move to 
        /// </summary>
        /// <param name="king"></param>
        /// <returns></returns>
        private List<ISquare> GetValidKingSquares(King king)
        {
            var listofValidSquares = new List<ISquare>();

            // get squares that are empty or occupied by the enemy
            var validSquares = GetSquares(s =>
                Math.Abs(s.ColumnIndex - king.MyLocation.ColumnIndex) == 1 &&
                Math.Abs(s.RowIndex - king.MyLocation.RowIndex) == 1 && (!s.Occupied || WhosOnThisSquare(s)?.MyTeam != king.MyTeam)
                ||
                s.ColumnIndex == king.MyLocation.ColumnIndex && Math.Abs(s.RowIndex - king.MyLocation.RowIndex) == 1 && (!s.Occupied || WhosOnThisSquare(s)?.MyTeam != king.MyTeam)
                ||
                s.RowIndex == king.MyLocation.RowIndex && Math.Abs(s.ColumnIndex - king.MyLocation.ColumnIndex) == 1 && (!s.Occupied || WhosOnThisSquare(s)?.MyTeam != king.MyTeam)
            );


            listofValidSquares.AddRange(validSquares);
            return listofValidSquares;
        }

        /// <summary>
        /// Checks if a Queen or Bishop can launch a diagonal attack
        /// </summary>
        /// <param name="square">The square that is to be attacked</param>
        /// <param name="team">The team looking to attack</param>
        /// <param name="piece">Queen or Bishop</param>
        /// <returns></returns>
        private bool CanADiagonalAttack(ISquare square, Teams team, PieceType piece)
        {
            bool canAttack = false;
            Teams enemyColor = team == Teams.White ? Teams.Black : Teams.White;

            // get all the squares a diagonal piece could launch an attack from eliminating the source square
            // all diagonal squares are row/column equi-distant from the source square
            var diagonalSquares = GetSquares(s => (square.ColumnIndex != s.ColumnIndex && square.RowIndex != s.RowIndex)
                                                && Math.Abs(square.ColumnIndex - s.ColumnIndex) ==
                                                Math.Abs(square.RowIndex - s.RowIndex)).ToList();

            // get all the enemy dialgonals on these squares
            var enemyDiagonals = GetPieces(p => p.MyTeam == enemyColor && p.WhoAmI == piece
                                            && diagonalSquares.Contains(p.MyLocation)).ToList();
            // if there are any enemy diagonals...then determine if there is open real estate between the 
            // enemy diagonal and the given square
            foreach (var enemyDiagonal in enemyDiagonals)
            {
                int upperColumn = enemyDiagonal.MyLocation.ColumnIndex > square.ColumnIndex ?
                                    enemyDiagonal.MyLocation.ColumnIndex : square.ColumnIndex;

                int lowerColumn = enemyDiagonal.MyLocation.ColumnIndex < square.ColumnIndex ?
                                    enemyDiagonal.MyLocation.ColumnIndex : square.ColumnIndex;

                int upperRow = enemyDiagonal.MyLocation.RowIndex > square.RowIndex ?
                                    enemyDiagonal.MyLocation.RowIndex : square.RowIndex;

                int lowerRow = enemyDiagonal.MyLocation.RowIndex < square.RowIndex ?
                                    enemyDiagonal.MyLocation.RowIndex : square.RowIndex;

                // returns all the empty squares between the two diagonal squares
                var emptySquares = GetSquares(s => (s.ColumnIndex > lowerColumn && s.ColumnIndex < upperColumn)
                                    && (s.RowIndex > lowerRow && s.RowIndex < upperRow)
                                    && (Math.Abs(s.ColumnIndex - enemyDiagonal.MyLocation.ColumnIndex) ==
                                        Math.Abs(s.RowIndex - enemyDiagonal.MyLocation.RowIndex))
                                    && !s.Occupied).Count();

                if (emptySquares == 0)
                {
                    // now check if the diagonal is right next to the square 
                    int columnDelta = Math.Abs(enemyDiagonal.MyLocation.ColumnIndex - square.ColumnIndex);
                    int rowDelta = Math.Abs(enemyDiagonal.MyLocation.RowIndex - square.RowIndex);
                    if (columnDelta == 1 && rowDelta == 1)
                    {
                        canAttack = true;
                        break;
                    }
                }
                else
                {
                    // the number of empty spaces needed for a diagonal to attack this square
                    int numOfEmptySpaces = Math.Abs(enemyDiagonal.MyLocation.RowIndex - square.RowIndex) - 1;
                    if (emptySquares == numOfEmptySpaces)
                    {
                        canAttack = true;
                        break;
                    }
                }
            }
            return canAttack;
        }

        private bool CanABishopAttackSquare(ISquare square, Teams team)
        {
            return CanADiagonalAttack(square, team, PieceType.Bishop);
        }
        private bool CanAKnightAttackSquare(ISquare square, Teams team)
        {
            Teams enemyColor = team == Teams.White ? Teams.Black : Teams.White;

            // get the squares from this square that a knight can launch an attack from
            var knightSquares = GetSquares(s =>
                                (s.ColumnIndex == square.ColumnIndex - 1
                                && (s.RowIndex == square.RowIndex + 2
                                || s.RowIndex == square.RowIndex - 2))
                                ||
                                (s.ColumnIndex == square.ColumnIndex + 1
                                && (s.RowIndex == square.RowIndex + 2
                                || s.RowIndex == square.RowIndex - 2))
                                ||
                                (s.RowIndex == square.RowIndex + 1
                                && (s.ColumnIndex == square.ColumnIndex + 2
                                || s.ColumnIndex == square.ColumnIndex - 2))
                                ||
                                (s.RowIndex == square.RowIndex - 1
                                && (s.ColumnIndex == square.ColumnIndex + 2
                                || s.ColumnIndex == square.ColumnIndex - 2))
                                ).ToList();

            // get all the enemy Knights in these squares
            var canAttack = GetPieces(p => p.MyTeam == enemyColor
                                            && p.WhoAmI == PieceType.Knight
                                            && knightSquares.Contains(p.MyLocation)).Any();
            return canAttack;
        }
        public bool CanAPawnAttackSquare(ISquare square, Teams team)
        {
            int forwardToken = team == Teams.White ? 1 : -1;
            Teams enemyColor = team == Teams.White ? Teams.Black : Teams.White;

            var canAttack = GetPieces(p => p.WhoAmI == PieceType.Pawn && p.MyTeam == enemyColor && p.MyStatus == PieceStatus.Active
                                                                       && p.MyLocation.RowIndex == square.RowIndex + forwardToken
                                                                       && ((p.MyLocation.ColumnIndex == square.ColumnIndex - 1)
                                                                           || (p.MyLocation.ColumnIndex == square.ColumnIndex + 1))).Any();
            return canAttack;
        }

        /// <summary>
        /// Checks whether a Queen or Rook can attack a given square
        /// </summary>
        /// <param name="square"></param>
        /// <param name="team"></param>
        /// <param name="piece"></param>
        /// <returns></returns>
        private bool CanAStraightPieceAttack(ISquare square, Teams team, PieceType piece)
        {
            bool canAttack = false;
            Teams enemyColor = team == Teams.White ? Teams.Black : Teams.White;

            // get the enemy pieces on the same column or row as the provided square
            var enemyPieces = GetPieces(p => p.WhoAmI == piece
                                        && p.MyTeam == enemyColor
                                        && (p.MyLocation.ColumnIndex == square.ColumnIndex
                                            || p.MyLocation.RowIndex == square.RowIndex));

            foreach (var enemyPiece in enemyPieces)
            {
                // get the # of spaces between the square and the enemy rook
                // on the same column...the rows separates them
                // on the same row ...the columns separates them
                int rowSpaces = Math.Abs(enemyPiece.MyLocation.RowIndex - square.RowIndex) - 1;
                int colSpaces = Math.Abs(enemyPiece.MyLocation.ColumnIndex - square.ColumnIndex) - 1;

                canAttack = GetSquares(s => s.RowIndex > square.RowIndex
                                        && s.RowIndex < enemyPiece.MyLocation.RowIndex
                                        && !s.Occupied).Count() == rowSpaces
                            ||
                            GetSquares(s => s.RowIndex < square.RowIndex
                                        && s.RowIndex > enemyPiece.MyLocation.RowIndex
                                        && !s.Occupied).Count() == rowSpaces
                            ||
                            GetSquares(s => s.ColumnIndex > square.ColumnIndex
                                        && s.ColumnIndex < enemyPiece.MyLocation.ColumnIndex
                                        && !s.Occupied).Count() == colSpaces
                            ||
                            GetSquares(s => s.ColumnIndex < square.ColumnIndex
                                        && s.ColumnIndex > enemyPiece.MyLocation.ColumnIndex
                                        && !s.Occupied).Count() == colSpaces

                            // this means these pieces are right next to each other
                            || (rowSpaces == 0 || colSpaces == 0);

                // if one rook can attack...then break out of loop
                if (canAttack) { break; }
            }
            return canAttack;
        }

        private bool CanARookAttackSquare(ISquare square, Teams team)
        {
            return CanAStraightPieceAttack(square, team, PieceType.Rook);
        }

        /// <summary>
        /// Checks to see whether the specified team's king is in checkmate
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public bool IsKingCheckMate(Teams team)
        {
            // TODO check for Run, Block & Capture
            bool isCheckMate = false;

            if (GetPieces(p => p.WhoAmI == PieceType.King && p.MyTeam == team).Single() is King king)
            {
                // RUN
                // Get all the valid squares a king can run to
                var kingSquares = GetValidKingSquares(king);

                // check if all these squares are attackable 
                isCheckMate = CheckAllLocations(kingSquares, team);
            }
            return isCheckMate;
        }

        public IPiece GetPieceForThisSquare(ISquare square, Teams team, PieceType pieceType)
        {
            IPiece piece = null;

            // TODO strategy pattern
            switch (pieceType)
            {
                case PieceType.King:
                    break;
                case PieceType.Queen:
                    break;
                case PieceType.Bishop:
                    break;
                case PieceType.Knight:
                    break;
                case PieceType.Rook:
                    break;
                case PieceType.Pawn:
                    piece = GetPawnForThisSquare(square, team);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pieceType), pieceType, null);
            }

            return piece;
        }

        /// <summary>
        /// Returns a pawn that can move into the square given
        /// </summary>
        /// <param name="square"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        private IPiece GetPawnForThisSquare(ISquare square, Teams team)
        {
            // get the list of pawns for this team
            var listOfPawns = ChessBoardProxy.Singleton.GetPieces(p => p.MyTeam == team &&
                                                                       p.WhoAmI == PieceType.Pawn &&
                                                                       p.MyStatus == PieceStatus.Active).ToList();
            bool isOk = true;
            IPiece pawnToMove = null;

            // for every pawn returned...
            for (int i = 0; isOk && i < listOfPawns.Count; i++)
            {
                if (listOfPawns[i] is Pawn pawn)
                {
                    // ... get all the valid squares it can go to
                    var listofSquares = ChessBoardProxy.Singleton.GetValidSquares(pawn);
                    if (listofSquares.Contains(square))
                    {
                        isOk = false;
                        pawnToMove = pawn;
                    }
                }
            }
            return pawnToMove;
        }

        /// <summary>
        /// Checks the locations specified to see if all squares can be attacked
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="team"></param>
        /// <returns>Returns true if all locations in the list can be attacked</returns>
        private bool CheckAllLocations(IEnumerable<ISquare> locations, Teams team)
        {
            bool canAttack = true;

            foreach (var item in locations)
            {
                canAttack &= CheckPiece(item, team);
            }

            return canAttack;
        }

        private bool CheckPiece(ISquare square, Teams team)
        {
            return  CanAPawnAttackSquare(square, team) ||
                    CanARookAttackSquare(square, team) ||
                    CanAKnightAttackSquare(square, team) ||
                    CanABishopAttackSquare(square, team) ||
                    CanAQueenAttackSquare(square, team) ||
                    CanAKingAttackSquare(square, team);
        }

        public IEnumerable<IPiece> GetPieces(Func<IPiece, bool> query)
        {
            return _chessBoard.ListOfPieces.Where(query);
        }
        public List<ISquare> GetValidSquares(IPiece piece)
        {
            var strategy = _validSquaresStrategyAbstractFactory.GetValidSquaresStrategy(piece);
            return strategy.GetValidSquares(piece);
        }

        public IEnumerable<ISquare> GetSquares(Func<ISquare, bool> query)
        {
            return _chessBoard.ListOfSquares.Where(query);
        }

        /// <summary>
        /// Returns any chess piece on the square provided
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        public IPiece WhosOnThisSquare(ISquare square)
        {
            var piece = _chessBoard.ListOfPieces.SingleOrDefault(p => p.MyLocation == square);
            return piece;
        }

        /// <summary>
        /// Returns an enemy chess piece on the square provided
        /// </summary>
        /// <param name="square"></param>
        /// <param name="enemyColor"></param>
        /// <returns></returns>
        public IPiece WhosOnThisSquare(ISquare square, Teams enemyColor)
        {
            var piece = _chessBoard.ListOfPieces.SingleOrDefault(p => p.MyLocation == square && p.MyTeam == enemyColor);
            return piece;
        }
    }
}
