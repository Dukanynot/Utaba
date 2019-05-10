using System;
using System.Linq;
using Utaba.Implementations.Pieces;
using Utaba.Implementations.Proxies;
using Utaba.Interfaces;

namespace Utaba.Implementations.Strategies.MoveStrategies
{
    sealed class  PawnMoveStrategy : IChessMoveStrategy
    {
        private readonly IChessBoardProxy _chessBoardProxy;
        private static readonly Lazy<PawnMoveStrategy> LazyPawnMoveHandler = new Lazy<PawnMoveStrategy>(() => 
            new PawnMoveStrategy(ChessBoardProxy.Singleton));
        public static PawnMoveStrategy Instance => LazyPawnMoveHandler.Value;

        private PawnMoveStrategy(IChessBoardProxy chessBoardProxy)
        {
            _chessBoardProxy = chessBoardProxy;
        }
        public  IMoveResponse HandleMove(IPiece piece, ISquare srcSquare, ISquare destSquare)
        {
            var imr = new MoveResponse();
            if (piece is Pawn pawn)
            {
                // TODO: look into making this a private variable that is assigned when a move request is received

                Teams enemyColor = pawn.MyTeam == Teams.White ? Teams.Black : Teams.White;
                
                // gets list of squares this pawn can legally move to
                var listOfValidPawnSquares = _chessBoardProxy.GetValidSquares(pawn);

                if (listOfValidPawnSquares.Contains(destSquare))
                {
                    // TODO: En passant, Capture, promotion, destination square occupant status
                    // TODO: Make sure the team making the move is not   in check

                    var startSquare = pawn.MyLocation;

                    // only adjust the occupied status of the destination square if there is no enemy piece on it
                    var enemyPiece = _chessBoardProxy.WhosOnThisSquare(destSquare, enemyColor);
                    if (enemyPiece == null)
                    {
                        destSquare.Occupied = true;
                    }

                    pawn.MyLocation.Occupied = false;
                    pawn.MyLocation = destSquare;

                    // Will this move result in the King being under check
                    if (_chessBoardProxy.IsKingInCheck(pawn.MyTeam))
                    {
                        // ...if the move would result in an exposed check of its own king
                        // ...cancel the move
                        pawn.MyLocation = startSquare;
                        pawn.MyLocation.Occupied = true;
                        if (enemyPiece == null)
                        {
                            destSquare.Occupied = false;
                        }
                        imr.Message = "Invalid move: Exposed Check!";
                        imr.SuccessfulMove = false;
                    }
                    else // not an exposed check move
                    {
                        // is this a regular move or a capture...find out if there is an enemy on this square
                        if (enemyPiece != null) // capture
                        {
                            // deactivate the piece that is about to be captured
                            enemyPiece.MyStatus = PieceStatus.Captured;
                            enemyPiece.MyLocation = null;

                            var isKingInCheck = _chessBoardProxy.IsKingInCheck(enemyColor);

                            // see if this move will result in a checkmate...check or stalemate
                            if (isKingInCheck && _chessBoardProxy.IsKingCheckMate(enemyColor))
                            {
                                imr.Message = $"{pawn.MyTeam} has won the game";
                            }
                            else if (isKingInCheck)
                            {
                                imr.Message = $"{enemyColor} is in check";
                            }
                            else // TODO check for stalemate
                            {
                                imr .Message = $"{destSquare.ColumnLetter}{destSquare.RowIndex + 1} capture was successful";
                                // stalemate involves checking other pieces to make sure they can't be moved

                            }
                        }
                        else // regular move / En Passant
                        {
                            // En Passant
                            if (Math.Abs(startSquare.RowIndex - destSquare.RowIndex) == 2)
                            {
                                var enPassantSquare = _chessBoardProxy.GetSquares(s => s.ColumnIndex == startSquare.ColumnIndex &&
                                                                      Math.Abs(s.RowIndex - startSquare.RowIndex) == 1 &&
                                                                      Math.Abs(s.RowIndex - destSquare.RowIndex) == 1).Single();
                                if (_chessBoardProxy.CanAPawnAttackSquare(enPassantSquare, pawn.MyTeam))
                                {
                                    imr.Message = $"{enPassantSquare.ColumnLetter}{enPassantSquare.RowIndex + 1} is an en passant square";
                                }
                                else
                                {
                                    imr.Message = $"{destSquare.ColumnLetter}{destSquare.RowIndex + 1} move was successful";

                                }
                            }
                            else // regular move
                            {
                                imr.Message = $"{destSquare.ColumnLetter}{destSquare.RowIndex + 1} move was successful";

                            }
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
                    imr.Message = "This move is not a valid move";
                    imr.SuccessfulMove = false;
                }
            }
            else
            {
                imr.Message = "Must be a Pawn";
            }
            return imr;
        }
    }
}
