using System;
using System.Collections.Generic;
using System.Linq;
using Utaba.Interfaces;
using Utaba.Factories.AbstractFactories;
using Utaba.Factories.ObjectFactories;
using Utaba.Interfaces.IFactories;

namespace Utaba.Implementations
{
    public sealed class ChessBoard 
    {
        private Teams _whosMove;

        private const byte NumberOfSquares = 64;
        private const byte MaxNumberOfPieces = 32;

        private readonly IChessPieceAbstractFactory _chesssPieceAbstractFactory;
        private readonly ISquareFactory _squareFactory;

        private static readonly Lazy<ChessBoard> Board = new Lazy<ChessBoard>(() =>
            new ChessBoard(ChessPieceAbstractFactory.Singleton, SquareFactory.Singleton));

        #region Constructor
        private ChessBoard(IChessPieceAbstractFactory chesssPieceAbstractFactory, ISquareFactory squareFactory)
        {
            // team white starts first in a standard game of chess
            _whosMove = Teams.White;
            _chesssPieceAbstractFactory = chesssPieceAbstractFactory;
            _squareFactory = squareFactory;

            InitializeSquares();
            InitializePieces();
        }
        #endregion

        #region Public Methods
        public IMoveResponse RequestMove(string notation)
        {
            IMoveResponse imr;

            try
            {
                // TODO: Really have to integrate with a GUI that uses standard notation / PGN
                var strategy = ChessNotationAbstractFactory.Singleton.GetChessNotationStrategy(notation);
                var cmd = strategy.CreateCommand(notation, _whosMove);
                imr = cmd.Execute();
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
        public List<ISquare> ListOfSquares { get; private set; }

        public List<IPiece> ListOfPieces { get; private set; }

        public static ChessBoard Instance => Board.Value;

        #endregion

        #region Private Methods

        public IEnumerable<ISquare> GetSquares(Func<ISquare, bool> query)
        {
            return ListOfSquares.Where(query);
        }
        #region Initialization Code
        /// <summary>
        /// Initializes all the pieces onto their starting squares
        /// </summary>
        private void InitializePieces()
        {
            ListOfPieces = new List<IPiece>(MaxNumberOfPieces);
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
                ListOfPieces.Add(piece);
            }
        }

        /// <summary>
        /// Initializes all the squares on the board to a standard Chess board
        /// </summary>
        private void InitializeSquares()
        {
            ListOfSquares = new List<ISquare>(NumberOfSquares);
            bool colorDark = true;

            // initialize all 64 squares
            for (byte col = 0; col <= 7; col++)
            {
                for (byte row = 0; row <= 7; row++)
                {
                    var sqClr = colorDark ? (SquareColors)Enum.Parse(typeof(SquareColors), "1") :
                    (SquareColors)Enum.Parse(typeof(SquareColors), "0");

                    var sq = _squareFactory.GetSquare(col, row, sqClr);
                    ListOfSquares.Add(sq);

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
