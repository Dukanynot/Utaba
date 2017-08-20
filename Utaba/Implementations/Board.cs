using System;
using System.Collections.Generic;
using System.Linq;
using Utaba.Interfaces;
using Utaba.Implementations.Pieces;
using System.Text.RegularExpressions;

namespace Utaba.Implementations
{
    public class Board : IBoard
    {
        private List<ISquare> _listofSquares;
        private List<IPiece> _listofPieces;

        private Teams _whosMove;

        private const byte NumberOfSquares = 64;
        private const byte MaxNumberOfPieces = 32;
        #region Constructor
        public Board()
        {
            InitializeSquares();
            InitializePieces();
            // team white starts first in a standard game of chess
            _whosMove = Teams.White;
        }
        #endregion

        #region Public Methods
        public IMoveResponse RequestMove(IPiece piece, string destination)
        {
            IMoveResponse imr = null;

            // only make a move if it is the right team's turn
            if (piece.MyTeam == _whosMove)
            {
                // checks to make sure the destination is in the right 
                // chess notation
                if (piece != null && destination.Length == 2
                    && Regex.IsMatch(destination, @"[a-hA-H]{1}[1-8]{1}"))
                {
                    int destinationColumn = Square.ColumnIndexByChar(destination[0]);
                    int destinationRow = int.Parse(destination.Substring(1, 1));

                    // get the actual square this string destination represents
                    var destSquare = GetSquares(s => s.ColumnIndex == destinationColumn
                                       && s.RowIndex == destinationRow - 1).Single();

                    switch (piece.WhoAmI)
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
                            var pawn = piece as Pawn;
                            if (pawn != null) { imr = PawnMoveHandler(pawn, destSquare); }
                            break;
                        default:
                            imr = null;
                            break;
                    }
                }
                else
                {
                    throw new Exception("Can't pass me a null Chess Piece\n Also check the destination notation");
                }
            }
            else
            {

                var mr = new MoveResponse();
                mr.SuccessfulMove = false;
                mr.ErrorMessage = "It is not your turn to move!";
            }
            return imr;
        } 
        #endregion

        #region Properties
        //public List<ISquare> ListOfSquares
        //{
        //    get { return _listofSquares; }
        //}

        public List<IPiece> ListOfPieces
        {
            get { return _listofPieces; }
        }
        #endregion

        #region Private Methods


        private bool CanAQueenAttackSquare(ISquare square, Teams team)
        {
            bool canAttack = false;



            return canAttack;
        }

        private bool CanABishopAttackSquare(ISquare square, Teams team)
        {
            bool canAttack = false;
            Teams enemyColor = team == Teams.White ? Teams.Black : Teams.White;

            // get all the squares a bishop could launch an attack from eliminating the source square
            // all diagonal squares are row/column equi-distant from the source square
            var bishopSquares = GetSquares(s => (square.ColumnIndex != s.ColumnIndex && square.RowIndex != s.RowIndex) 
                                                && Math.Abs(square.ColumnIndex - s.ColumnIndex) ==
                                                Math.Abs(square.RowIndex - s.RowIndex)).ToList();

            // get all the enemy bishops on these squares
            var enemyBishops = GetPieces(p => p.MyTeam == enemyColor && p.WhoAmI == PieceType.Bishop
                                            && bishopSquares.Contains(p.MyLocation)).ToList();
            // if there are any enemy bishops...then determine if there is open real estate between the 
            // enemy bishop and the given square
            foreach (var enemyBishop in enemyBishops)
            {
                int upperColumn = enemyBishop.MyLocation.ColumnIndex > square.ColumnIndex ?
                                    enemyBishop.MyLocation.ColumnIndex : square.ColumnIndex;

                int lowerColumn = enemyBishop.MyLocation.ColumnIndex < square.ColumnIndex ?
                                    enemyBishop.MyLocation.ColumnIndex : square.ColumnIndex;

                int upperRow = enemyBishop.MyLocation.RowIndex > square.RowIndex ?
                                    enemyBishop.MyLocation.RowIndex : square.RowIndex;

                int lowerRow = enemyBishop.MyLocation.RowIndex < square.RowIndex ?
                                    enemyBishop.MyLocation.RowIndex : square.RowIndex;

                // returns all the empty squares between the two diagonal squares
                var emptySquares = GetSquares(s => (s.ColumnIndex > lowerColumn && s.ColumnIndex < upperColumn)
                                    && (s.RowIndex > lowerRow && s.RowIndex < upperRow)
                                    && (Math.Abs(s.ColumnIndex - enemyBishop.MyLocation.ColumnIndex ) == 
                                        Math.Abs(s.RowIndex - enemyBishop.MyLocation.RowIndex)) 
                                    && !s.Occupied).Count();

                if (emptySquares == 0)
                {
                    // now check if the bishop is right next to the square 
                    int columnDelta = Math.Abs(enemyBishop.MyLocation.ColumnIndex - square.ColumnIndex);
                    int rowDelta = Math.Abs(enemyBishop.MyLocation.RowIndex - square.RowIndex);
                    if (columnDelta == 1 && rowDelta == 1)
                    {
                        canAttack = true;
                        break;
                    }
                }
                else
                {
                    // the number of empty spaces needed for a bishop to attack this square
                    int numOfEmptySpaces = Math.Abs(enemyBishop.MyLocation.RowIndex - square.RowIndex) - 1;
                    if (emptySquares == numOfEmptySpaces)
                    {
                        canAttack = true;
                        break;
                    }
                }  
            }

            return canAttack;

        }
        private bool CanAKnightAttackSquare(ISquare square, Teams team)
        {
            bool canAttack = false;
            Teams enemyColor = team == Teams.White ? Teams.Black : Teams.White;

            // get the squares from this square that a knight can launch an attack from
            var knightSquares = GetSquares(s => 
                                (s.ColumnIndex == square.ColumnIndex -1 
                                && (s.RowIndex == square.RowIndex + 2
                                || s.RowIndex == square.RowIndex - 2))
                                ||
                                (s.ColumnIndex == square.ColumnIndex + 1
                                && (s.RowIndex == square.RowIndex + 2
                                || s.RowIndex == square.RowIndex -2))
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
            canAttack = GetPieces(p => p.MyTeam == enemyColor
                               && p.WhoAmI == PieceType.Knight
                               && knightSquares.Contains(p.MyLocation)).Any();
            return canAttack;
        }
        private bool CanAPawnAttackSquare(ISquare square, Teams team)
        {
            bool canAttack = false;
            int forwardToken = team == Teams.White ? 1 : -1;
            Teams enemyColor = team == Teams.White ? Teams.Black : Teams.White;

            canAttack = GetPieces(p => p.WhoAmI == PieceType.Pawn && p.MyTeam == enemyColor
                      && p.MyLocation.RowIndex == square.RowIndex + forwardToken
                      && ((p.MyLocation.ColumnIndex == square.ColumnIndex - 1)
                      || (p.MyLocation.ColumnIndex == square.ColumnIndex + 1))).Any();
            return canAttack;
        }

        private bool CanARookAttackSquare(ISquare square, Teams team)
        {
            bool canAttack = false;
            Teams enemyColor = team == Teams.White ? Teams.Black : Teams.White;

            // get the enemy rooks on the same column or row as the provided square
            var enemyRooks = GetPieces(p => p.WhoAmI == PieceType.Rook 
                                        && p.MyTeam == enemyColor
                                        && (p.MyLocation.ColumnIndex == square.ColumnIndex
                                            || p.MyLocation.RowIndex == square.RowIndex));

            foreach (var enemyrook in enemyRooks)
            {
                // get the # of spaces between the square and the enemy rook
                // on the same column...the rows separates them
                // on the same row ...the columns separates them
                int rowSpaces = Math.Abs(enemyrook.MyLocation.RowIndex - square.RowIndex) - 1;
                int colSpaces = Math.Abs(enemyrook.MyLocation.ColumnIndex - square.ColumnIndex) - 1;

                canAttack = GetSquares(s => s.RowIndex > square.RowIndex
                                        && s.RowIndex < enemyrook.MyLocation.RowIndex
                                        && !s.Occupied).Count() == rowSpaces
                            ||
                            GetSquares(s => s.RowIndex < square.RowIndex
                                        && s.RowIndex > enemyrook.MyLocation.RowIndex
                                        && !s.Occupied).Count() == rowSpaces
                            ||
                            GetSquares(s => s.ColumnIndex > square.ColumnIndex
                                        && s.ColumnIndex < enemyrook.MyLocation.ColumnIndex
                                        && !s.Occupied).Count() == colSpaces
                            ||
                            GetSquares(s => s.ColumnIndex < square.ColumnIndex
                                        && s.ColumnIndex > enemyrook.MyLocation.ColumnIndex
                                        && !s.Occupied).Count() == colSpaces

                            // this means this pieces are right next to each other
                            || (rowSpaces == 0 || colSpaces == 0);

                // if one rook can attack...then break out of loop
                if (canAttack) { break; }
            }
            return canAttack;

        }
        /// <summary>
        /// Checks to see whether the specified team's King is in Check
        /// </summary>
        /// <param name="team">The team to check for check :)</param>
        /// <returns></returns>
        private bool IsMyKingInCheck(Teams team)
        {
            bool isInCheck = false;

            // get the King's current location
            var kingLocation = GetPieces(p => p.WhoAmI == PieceType.King 
                                && p.MyTeam == team).Single().MyLocation;

            // can an enemy pawn attack this square?
            isInCheck = CanAPawnAttackSquare(kingLocation, team);
            
            // if no pawn can attack..check for a Rook
            if (!isInCheck )
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


            return isInCheck;
        }
        private List<ISquare> GetValidPawnSquares(Pawn pawn)
        {
            var listofValidSquares = new List<ISquare>();

            // the concept of moving forward is opposite for Black and White...HA!!!
            int forwardToken = pawn.MyTeam == Teams.White ? 1 : -1;
            Teams enemyColor = pawn.MyTeam == Teams.White ? Teams.Black : Teams.White;

            // check if the square in front of the pawn is empty
            var frontSquare = GetSquares(s => s.ColumnIndex == pawn.MyLocation.ColumnIndex
                                && s.RowIndex == pawn.MyLocation.RowIndex + forwardToken
                                && !s.Occupied).SingleOrDefault();
            if (frontSquare != null)
            {
                listofValidSquares.Add(frontSquare);

                // since the front square is empty...check if the pawn is allowed to take two steps
                if (!pawn.HasMoved)
                {
                    var twoSquares = GetSquares(s => s.ColumnIndex == pawn.MyLocation.ColumnIndex
                    && s.RowIndex == pawn.MyLocation.RowIndex + (2 * forwardToken)
                    && !s.Occupied).SingleOrDefault();
                    if (twoSquares != null)
                    {
                        listofValidSquares.Add(twoSquares);
                    }
                }
            }

            // now check for capture squares
            var captureSquares = GetSquares(s => (s.ColumnIndex == pawn.MyLocation.ColumnIndex - 1
                                            && s.RowIndex == pawn.MyLocation.RowIndex + forwardToken
                                            && s.Occupied && WhosOnThisSquare(s)?.MyTeam == enemyColor)
                                            || (s.ColumnIndex == pawn.MyLocation.ColumnIndex + 1
                                            && s.RowIndex == pawn.MyLocation.RowIndex + forwardToken
                                            && s.Occupied && WhosOnThisSquare(s)?.MyTeam == enemyColor));
            listofValidSquares.AddRange(captureSquares);
            return listofValidSquares;
        }
        private IMoveResponse PawnMoveHandler(Pawn pawn, ISquare destSquare)
        {
            Teams enemyColor = pawn.MyTeam == Teams.White ? Teams.Black : Teams.White;

            var imr = new MoveResponse();

            // gets list of squares this pawn can legally move to
            var lst = GetValidPawnSquares(pawn);

            if (lst.Contains(destSquare))
            {
              //  pawn.MyLocation.Occupied = false;
              //  var holdMySquare = pawn.MyLocation;

                // TODO: En passant, Capture , promotion, destination square occupant status
               // destSquare.Occupied = true;
             //   pawn.MyLocation = destSquare;               

                // Will this move result in the King being under check
                //if (IsMyKingInCheck(pawn.MyTeam))
                //{
                //    // ...if the move would result in an exposed check of its own king
                //    //...cancel the move
                //    pawn.MyLocation = holdMySquare;
                //    pawn.MyLocation.Occupied = true;
                    
                //}
                //else // not an exposed check move
                {
                    // is this a regular move or a capture
                    var piece = WhosOnThisSquare(destSquare);

                    if (piece != null) // capture
                    {
                        // deactivate piece that is about to be captured
                        pawn.MyLocation.Occupied = false;
                        destSquare.Occupied = true;
                        pawn.MyLocation = destSquare;
                        piece.MyStatus = PieceStatus.Captured;
    

                        // see if this move will result in a check...checkmate or stalemate
                        if (IsMyKingInCheck(enemyColor))
                        {
                            imr.ErrorMessage = $"{enemyColor} is in check";
                        }

                    }
                    else // regular move
                    {
                        pawn.MyLocation.Occupied = false;
                        destSquare.Occupied = true;
                        pawn.MyLocation = destSquare;
                        
                    }

                    if (!pawn.HasMoved)
                    {
                        pawn.HasMoved = true;
                    }
                    imr.SuccessfulMove = true;
                }
            }
            else
            {
                imr.ErrorMessage = "This move is not a valid move";
                imr.SuccessfulMove = false;
            }           

            return imr;
        }
        private ChessPiece WhosOnThisSquare(ISquare square)
        {
            var piece = _listofPieces.Where(p => p.MyLocation == square).FirstOrDefault();
            return piece as ChessPiece;
        }

        private IEnumerable<ISquare> GetSquares(Func<ISquare, bool> query)
        {
            return _listofSquares.Where(query);   
        }
        private IEnumerable<IPiece> GetPieces(Func<IPiece, bool> query)
        {
            return _listofPieces.Where(query);
        }
        #region Initialization Code
        /// <summary>
        /// Initializes all the pieces onto their starting squares
        /// </summary>
        private void InitializePieces()
        {
            _listofPieces = new List<IPiece>(MaxNumberOfPieces);
            InitializePawns();
            InitializeRooks();
            InitializeKnights();
            InitializeBishops();
            InitializeQueens();
            InitializeKings();
        }

        private void InitializeKings()
        {
            // create White King 
            CreateChessPiece(Teams.White, GetSquares(s => (s.ColumnIndex == 4 && s.RowIndex == 0)), PieceType.King);

            // create Black King
            CreateChessPiece(Teams.Black, GetSquares(s => (s.ColumnIndex == 4 && s.RowIndex == 7)), PieceType.King);
        }
        private void InitializeQueens()
        {
            // create White Queen 
            CreateChessPiece(Teams.White, GetSquares(s => (s.ColumnIndex == 3 && s.RowIndex == 0)), PieceType.Queen);

            // create Black Queen
            CreateChessPiece(Teams.Black, GetSquares(s => (s.ColumnIndex == 3 && s.RowIndex == 7)), PieceType.Queen);
        }

        private void InitializeBishops()
        {
            // create White Bishops
            var whiteBishopSquares = GetSquares(s => (s.ColumnIndex == 2 && s.RowIndex == 0) ||
                                        (s.ColumnIndex == 5 && s.RowIndex == 0));
            CreateChessPiece(Teams.White, whiteBishopSquares, PieceType.Bishop);

            // create Black Bishops
            var blackBishopSquares = GetSquares(s => (s.ColumnIndex == 2 && s.RowIndex == 7) ||
                                        (s.ColumnIndex == 5 && s.RowIndex == 7));
            CreateChessPiece(Teams.Black, blackBishopSquares, PieceType.Bishop);
        }

        private void InitializeKnights()
        {
            // create White Knights
            var whiteKnightSquares = GetSquares(s => (s.ColumnIndex == 1 && s.RowIndex == 0) ||
                                        (s.ColumnIndex == 6 && s.RowIndex == 0));
            CreateChessPiece(Teams.White, whiteKnightSquares, PieceType.Knight);

            // create Black Knights
            var blackKnightSquares = GetSquares(s => (s.ColumnIndex == 1 && s.RowIndex == 7) ||
                                        (s.ColumnIndex == 6 && s.RowIndex == 7));
            CreateChessPiece(Teams.Black, blackKnightSquares, PieceType.Knight);
        }
        private void InitializeRooks()
        {
            // Create White Rooks
            var whiteRookSquares = GetSquares(s => (s.ColumnIndex == 0 && s.RowIndex == 0) ||
                         (s.ColumnIndex == 7 && s.RowIndex == 0));
            CreateChessPiece(Teams.White, whiteRookSquares, PieceType.Rook);

            // Create Black Rooks
            var blackRookSquares = GetSquares(s => (s.ColumnIndex == 0 && s.RowIndex == 7) ||
                                   (s.ColumnIndex == 7 && s.RowIndex == 7));

            CreateChessPiece(Teams.Black, blackRookSquares, PieceType.Rook);
        }

        private void InitializePawns()
        {
            // initialize all the white pawns...they start on row index 1 (rank 2...row 2)
            CreateChessPiece(Teams.White, GetSquares(s => s.RowIndex == 1), PieceType.Pawn);

            // initialize all the black pawns...they start on row index 6 (rank 7...row 7)
            CreateChessPiece(Teams.Black, GetSquares(s => s.RowIndex == 6), PieceType.Pawn);
        }

        private void CreateChessPiece(Teams team, IEnumerable<ISquare> squares, PieceType pieceType)
        {
            foreach (var square in squares)
            {
                square.Occupied = true;
                switch (pieceType)
                {
                    case PieceType.King:
                        _listofPieces.Add(new King(square, team));
                        break;
                    case PieceType.Queen:
                        _listofPieces.Add(new Queen(square, team));
                        break;
                    case PieceType.Bishop:
                        _listofPieces.Add(new Bishop(square, team));
                        break;
                    case PieceType.Knight:
                        _listofPieces.Add(new Knight(square, team));
                        break;
                    case PieceType.Rook:
                        _listofPieces.Add(new Rook(square, team));
                        break;
                    case PieceType.Pawn:
                        _listofPieces.Add(new Pawn(square, team));
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Initializes all the squares on the board to a standard Chess board
        /// </summary>
        private void InitializeSquares()
        {
            _listofSquares = new List<ISquare>(NumberOfSquares);
            bool colorDark = true;

            // initialize all 64 squares
            for (byte col = 0; col <= 7; col++)
            {
                for (byte row = 0; row <= 7; row++)
                {
                    var sqClr = colorDark ? (SquareColors)Enum.Parse(typeof(SquareColors), "1") :
                    (SquareColors)Enum.Parse(typeof(SquareColors), "0");
                    var sq = new Square(col, row, sqClr);
                    _listofSquares.Add(sq);

                    // ensures every cell alternates in color 
                    if (row != 7)
                    {
                        colorDark = !colorDark;
                    }
                }
            }
        }  
        #endregion
        #endregion
    }
}
