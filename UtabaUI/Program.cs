using System;
using System.Collections.Generic;
using Utaba.Interfaces;
using Utaba.Implementations;
using Utaba;


namespace UtabaUI
{
    class Program
    {
       // static private List<ISquare> _listofSquares;
        static private List<IPiece> _listOfPieces;
        static void Main(string[] args)
        {
            var board = new Board();
            _listOfPieces = board.ListOfPieces;
        }
    }
}
