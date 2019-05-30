using System;
using Utaba.Factories.ObjectFactories;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.AbstractFactories
{
    /// <summary>
    /// Returns a chess piece
    /// </summary>
    class ChessPieceAbstractFactory : IChessPieceAbstractFactory
    {
        private static readonly Lazy<ChessPieceAbstractFactory> LazyChessPieceAbstractFactory = new Lazy<ChessPieceAbstractFactory>(() => 
            new ChessPieceAbstractFactory());
        private ChessPieceAbstractFactory() { }

        public static ChessPieceAbstractFactory Singleton => LazyChessPieceAbstractFactory.Value;

        public IPiece GetChessPiece(Teams team, ISquare square, PieceType pieceType)
        {
            IPiece chessPiece;
            switch (pieceType)
            {
                case PieceType.King:
                    chessPiece = KingFactory.Singleton.GetChessPiece(square, team);
                    break;
                case PieceType.Queen:
                    chessPiece = QueenFactory.Singleton.GetChessPiece(square, team);
                    break;
                case PieceType.Bishop:
                    chessPiece = BishopFactory.Singleton.GetChessPiece(square, team);
                    break;
                case PieceType.Knight:
                    chessPiece = KnightFactory.Singleton.GetChessPiece(square, team);
                    break;
                case PieceType.Rook:
                    chessPiece = RookFactory.Singleton.GetChessPiece(square, team);
                    break;
                case PieceType.Pawn:
                    chessPiece = PawnFactory.Singleton.GetChessPiece(square, team);
                    break;
                default:
                    chessPiece = null;
                    break;
            }
            return chessPiece;
        }
    }
}
