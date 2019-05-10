﻿
namespace Utaba.Interfaces
{
    /// <summary>
    /// Implemented by any Chess piece on the Chess board
    /// </summary>
    public interface IPiece
    {
        void Move();

        Teams MyTeam { get;}

        ISquare MyLocation { get; set; }

        PieceStatus MyStatus { get; set; }

        PieceType WhoAmI { get;}
    }
}
