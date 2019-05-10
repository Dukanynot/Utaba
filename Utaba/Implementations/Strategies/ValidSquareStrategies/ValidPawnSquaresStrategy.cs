using System;
using System.Collections.Generic;
using System.Linq;
using Utaba.Implementations.Pieces;
using Utaba.Implementations.Proxies;
using Utaba.Interfaces;

namespace Utaba.Implementations.Strategies.ValidSquareStrategies
{
    /// <summary>
    /// Returns valid squares a pawn can go to
    /// </summary>
    class ValidPawnSquaresStrategy : IValidSquaresStrategy
    {
        private readonly IChessBoardProxy _chessBoardProxy;
        private static readonly Lazy<ValidPawnSquaresStrategy> LazyValidPawnSquaresStrategy = new Lazy<ValidPawnSquaresStrategy>(() =>
            new ValidPawnSquaresStrategy(ChessBoardProxy.Singleton));

        private ValidPawnSquaresStrategy(IChessBoardProxy chessBoardProxy)
        {
            _chessBoardProxy = chessBoardProxy;
        }

        public static ValidPawnSquaresStrategy Singleton => LazyValidPawnSquaresStrategy.Value;

        public List<ISquare> GetValidSquares(IPiece piece)
        {
            var listofValidSquares = new List<ISquare>();
            if (piece is Pawn pawn)
            {
                // the concept of moving forward is opposite for Black and White...HA!!!
                int forwardToken = pawn.MyTeam == Teams.White ? 1 : -1;
                Teams enemyColor = pawn.MyTeam == Teams.White ? Teams.Black : Teams.White;

                // check if the square in front of the pawn is empty
                var frontSquare = _chessBoardProxy.GetSquares(s => s.ColumnIndex == pawn.MyLocation.ColumnIndex
                                    && s.RowIndex == pawn.MyLocation.RowIndex + forwardToken
                                    && !s.Occupied).SingleOrDefault();
                if (frontSquare != null)
                {
                    listofValidSquares.Add(frontSquare);

                    // since the front square is empty...check if the pawn is allowed to take two steps
                    if (!pawn.HasMoved)
                    {
                        var twoSquares = _chessBoardProxy.GetSquares(s => s.ColumnIndex == pawn.MyLocation.ColumnIndex
                        && s.RowIndex == pawn.MyLocation.RowIndex + (2 * forwardToken)
                        && !s.Occupied).SingleOrDefault();
                        if (twoSquares != null)
                        {
                            listofValidSquares.Add(twoSquares);
                        }
                    }
                }

                // now check for capture squares occupied by enemy forces
                var captureSquares = _chessBoardProxy.GetSquares(s => (s.ColumnIndex == pawn.MyLocation.ColumnIndex - 1
                                                && s.RowIndex == pawn.MyLocation.RowIndex + forwardToken
                                                && s.Occupied && _chessBoardProxy.WhosOnThisSquare(s)?.MyTeam == enemyColor)
                                                || (s.ColumnIndex == pawn.MyLocation.ColumnIndex + 1
                                                && s.RowIndex == pawn.MyLocation.RowIndex + forwardToken
                                                && s.Occupied && _chessBoardProxy.WhosOnThisSquare(s)?.MyTeam == enemyColor));
                listofValidSquares.AddRange(captureSquares);

                // TODO: Check for en pessant squares

            }
            return listofValidSquares;
        }
    }
}
