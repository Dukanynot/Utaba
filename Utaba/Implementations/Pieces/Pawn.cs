using System;
using System.Linq;
using Utaba.Implementations.Proxies;
using Utaba.Interfaces;

namespace Utaba.Implementations.Pieces
{
    internal class Pawn : ChessPiece
    {
        public Pawn(ISquare location, Teams team)
            :base(team, location)
        {
            whoAmI = PieceType.Pawn;
        }

        public override IMoveResponse Move(ISquare destSquare)
        {
            // TODO: promotion

            var startSquare = this.MyLocation;

            // En Passant
            if (Math.Abs(startSquare.RowIndex - destSquare.RowIndex) == 2)
            {
                var enPassantSquare = ChessBoardProxy.Singleton.GetSquares(s => s.ColumnIndex == startSquare.ColumnIndex &&
                                                                       Math.Abs(s.RowIndex - startSquare.RowIndex) == 1 &&
                                                                       Math.Abs(s.RowIndex - destSquare.RowIndex) == 1).Single();

                if (ChessBoardProxy.Singleton.CanAPawnAttackSquare(enPassantSquare, this.MyTeam))
                {
                    EnPassantSquare = enPassantSquare;
                    Console.WriteLine("About to En passant");
                }
            }

            var response = base.Move(destSquare);
            if (response.SuccessfulMove)
            {
                UpdateHasMoved();
            }
            return response;
        }

        public override IMoveResponse Capture(IPiece capturedPiece, ISquare capturedSquare)
        {
            var response = base.Capture(capturedPiece,capturedSquare);
            if (response.SuccessfulMove)
            {
                UpdateHasMoved();
            }
            return response;
        }

        public bool HasMoved { get; set; }
        public ISquare EnPassantSquare { get; set; }

        private void UpdateHasMoved()
        {
            if (!HasMoved)
            {
                HasMoved = true;
            }
        }
    }
}
