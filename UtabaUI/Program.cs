using System.Collections.Generic;
using Utaba.Interfaces;
using Utaba.Implementations;


namespace UtabaUI
{
    class Program
    {
       // static private List<ISquare> _listofSquares;
        private static List<IPiece> _listOfPieces;
        static void Main()
        {
            var board = new Board();
            _listOfPieces = board.ListOfPieces;
        }
    }
}
