using Utaba.Implementations.Pieces;
using Utaba.Interfaces;

namespace Utaba.Factories
{
    static class CreateChessPiece
    {
        public static IPiece GetChessPiece(Teams team, ISquare square, PieceType pieceType)
        {
            IPiece chessPiece;
            switch (pieceType)
            {
                case PieceType.King:
                    chessPiece = new King(square, team);
                    break;
                case PieceType.Queen:
                    chessPiece = new Queen(square, team);
                    break;
                case PieceType.Bishop:
                    chessPiece = new Bishop(square, team);
                    break;
                case PieceType.Knight:
                    chessPiece = new Knight(square, team);
                    break;
                case PieceType.Rook:
                    chessPiece = new Rook(square, team);
                    break;
                case PieceType.Pawn:
                    chessPiece = new Pawn(square, team);
                    break;
                default:
                    chessPiece = null;
                    break;
            }
            return chessPiece;
        }
    }
}
