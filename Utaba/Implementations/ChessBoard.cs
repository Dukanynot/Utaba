using System;
using System.Collections.Generic;
using System.Linq;
using Utaba.Interfaces;
using Utaba.Implementations.Pieces;
using System.Text.RegularExpressions;
using Utaba.Factories.AbstractFactories;
using Utaba.Implementations.Proxies;
using Utaba.Interfaces.IFactories;

namespace Utaba.Implementations
{
    public sealed class ChessBoard 
    {
        private List<ISquare> _listofSquares;
        private List<IPiece> _listofPieces;

        private Teams _whosMove;

        private const byte NumberOfSquares = 64;
        private const byte MaxNumberOfPieces = 32;
        private readonly IChessMoveStrategyAbstractFactory _chessMoveStrategyAbstractFactory;
        private readonly IChessPieceAbstractFactory _chesssPieceAbstractFactory;

        private static readonly Lazy<ChessBoard> Board = new Lazy<ChessBoard>(() =>
            new ChessBoard(ChessMoveStrategyAbstractFactory.Singleton, ChessPieceAbstractFactory.Singleton));

        #region Constructor
        private ChessBoard(IChessMoveStrategyAbstractFactory chessMoveStrategyAbstractFactory, IChessPieceAbstractFactory chesssPieceAbstractFactory)
        {
            // team white starts first in a standard game of chess
            _whosMove = Teams.White;
            _chessMoveStrategyAbstractFactory = chessMoveStrategyAbstractFactory;
            _chesssPieceAbstractFactory = chesssPieceAbstractFactory;

            InitializeSquares();
            InitializePieces();
        }
        #endregion

        #region Public Methods
        public IMoveResponse RequestMove(string destination)
        {
            IMoveResponse imr = null;

            try
            {
                // only make a move if it is the right team's turn

                // checks to make sure the destination is in the right 
                // chess notation
                // TODO: Really have to integrate with a GUI that uses standard notation / PGN
                if (destination.Length == 2
                    && Regex.IsMatch(destination, @"[a-hA-H]{1}[1-8]{1}"))
                {
                    int destinationColumn = Square.ColumnIndexByChar(destination[0]);
                    int destinationRow = int.Parse(destination.Substring(1, 1));

                    // get the actual square this string destination represents
                    var destSquare = GetSquares(s => s.ColumnIndex == destinationColumn
                                                     && s.RowIndex == destinationRow - 1).Single();

                    // TODO convert this to strategy pattern
                    var piece = GetPawnForThisSquare(destSquare, _whosMove);

                    if (piece?.MyTeam == _whosMove)
                    {
                        var moveStrategy = _chessMoveStrategyAbstractFactory.GetChessMoveStrategy(piece);
                        imr = moveStrategy.HandleMove(piece, piece.MyLocation, destSquare);
                    }
                    else
                    {
                        imr = new MoveResponse
                        {
                            SuccessfulMove = false,
                            Message = "Invalid Move!!!"
                        };
                    }
                }
                else
                {
                    throw new Exception("Can't pass me a null Chess Piece\n Also check the destination notation");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            _whosMove = _whosMove == Teams.White ? Teams.Black : Teams.White;
            return imr;
        }
        #endregion

        #region Properties
        public List<ISquare> ListOfSquares => _listofSquares;

        public List<IPiece> ListOfPieces => _listofPieces;

        public static ChessBoard Instance => Board.Value;

        #endregion

        #region Private Methods

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

        public IEnumerable<ISquare> GetSquares(Func<ISquare, bool> query)
        {
            return _listofSquares.Where(query);
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
            CreateChessPieces(Teams.White, GetSquares(s => (s.ColumnIndex == 4 && s.RowIndex == 0)), PieceType.King);

            // create Black King
            CreateChessPieces(Teams.Black, GetSquares(s => (s.ColumnIndex == 4 && s.RowIndex == 7)), PieceType.King);
        }
        private void InitializeQueens()
        {
            // create White Queen 
            CreateChessPieces(Teams.White, GetSquares(s => (s.ColumnIndex == 3 && s.RowIndex == 0)), PieceType.Queen);

            // create Black Queen
            CreateChessPieces(Teams.Black, GetSquares(s => (s.ColumnIndex == 3 && s.RowIndex == 7)), PieceType.Queen);
        }

        private void InitializeBishops()
        {
            // create White Bishops
            var whiteBishopSquares = GetSquares(s => (s.ColumnIndex == 2 && s.RowIndex == 0) ||
                                        (s.ColumnIndex == 5 && s.RowIndex == 0));
            CreateChessPieces(Teams.White, whiteBishopSquares, PieceType.Bishop);

            // create Black Bishops
            var blackBishopSquares = GetSquares(s => (s.ColumnIndex == 2 && s.RowIndex == 7) ||
                                        (s.ColumnIndex == 5 && s.RowIndex == 7));
            CreateChessPieces(Teams.Black, blackBishopSquares, PieceType.Bishop);
        }

        private void InitializeKnights()
        {
            // create White Knights
            var whiteKnightSquares = GetSquares(s => (s.ColumnIndex == 1 && s.RowIndex == 0) ||
                                        (s.ColumnIndex == 6 && s.RowIndex == 0));
            CreateChessPieces(Teams.White, whiteKnightSquares, PieceType.Knight);

            // create Black Knights
            var blackKnightSquares = GetSquares(s => (s.ColumnIndex == 1 && s.RowIndex == 7) ||
                                        (s.ColumnIndex == 6 && s.RowIndex == 7));
            CreateChessPieces(Teams.Black, blackKnightSquares, PieceType.Knight);
        }
        private void InitializeRooks()
        {
            // Create White Rooks
            var whiteRookSquares = GetSquares(s => (s.ColumnIndex == 0 && s.RowIndex == 0) ||
                         (s.ColumnIndex == 7 && s.RowIndex == 0));
            CreateChessPieces(Teams.White, whiteRookSquares, PieceType.Rook);

            // Create Black Rooks
            var blackRookSquares = GetSquares(s => (s.ColumnIndex == 0 && s.RowIndex == 7) ||
                                   (s.ColumnIndex == 7 && s.RowIndex == 7));

            CreateChessPieces(Teams.Black, blackRookSquares, PieceType.Rook);
        }

        private void InitializePawns()
        {
            // initialize all the white pawns...they start on row index 1 (rank 2...row 2)
            CreateChessPieces(Teams.White, GetSquares(s => s.RowIndex == 1), PieceType.Pawn);

            // initialize all the black pawns...they start on row index 6 (rank 7...row 7)
            CreateChessPieces(Teams.Black, GetSquares(s => s.RowIndex == 6), PieceType.Pawn);
        }

        private void CreateChessPieces(Teams team, IEnumerable<ISquare> squares, PieceType pieceType)
        {
            foreach (var square in squares)
            {
                square.Occupied = true;
                IPiece piece = _chesssPieceAbstractFactory.GetChessPiece(team, square, pieceType);
                _listofPieces.Add(piece);
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

                    // TODO: add a square factory
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
