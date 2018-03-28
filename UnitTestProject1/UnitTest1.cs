using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utaba.Implementations;
using Utaba.Interfaces;
using Utaba;
using System.Collections.Generic;
using System.Linq;
using Moq.Language.Flow;
using Moq;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        List<IPiece> _listOfPieces;

        [TestMethod]
        public void TestPawnMoves()
        {
            
            var board = new Board();
            _listOfPieces = board.ListOfPieces;

            var wp1 = GetPiece(4, 1);
            var bp1 = GetPiece(0, 6);

            board.RequestMove(wp1, "e4");
            var mr = board.RequestMove(bp1, "a6");
            board.RequestMove(bp1, "a5");
            //board.RequestMove(bp1, "g4");
            //board.RequestMove(bp1, "g3");
            //board.RequestMove(bp1, "f2");
            // board.RequestMove(bp1, "b1");



            //  board.RequestMove(bp1, "b5");


            Assert.IsTrue(mr != null && mr.SuccessfulMove);
            
        }

        private IPiece GetPiece(byte col,byte row)
        {
            return _listOfPieces
                .Where(p => p.MyLocation.ColumnIndex == col && p.MyLocation.RowIndex == row).Single();
        }
    }
}
