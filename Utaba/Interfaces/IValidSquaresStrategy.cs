using System.Collections.Generic;

namespace Utaba.Interfaces
{
    /// <summary>
    /// Interface for any stratgey that returns valid squares a chess piece can go to
    /// </summary>
    interface IValidSquaresStrategy
    {
        List<ISquare> GetValidSquares(IPiece piece);
    }
}
