﻿using System;
using System.Collections.Generic;


namespace Utaba.Interfaces
{
    interface IChessBoardProxy
    {
        List<ISquare> GetValidSquares(IPiece piece);
        IPiece WhosOnThisSquare(ISquare square);
        IPiece WhosOnThisSquare(ISquare square, Teams enemyColor);
        bool IsKingInCheck(Teams team);
        bool IsKingCheckMate(Teams team);
        IEnumerable<ISquare> GetSquares(Func<ISquare, bool> query);
        IEnumerable<IPiece> GetPieces(Func<IPiece, bool> query);
        bool CanAPawnAttackSquare(ISquare square, Teams team);
    }
}
