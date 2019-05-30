using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utaba.Implementations;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestBlackPawnCheckMate()
        {
            var board =  ChessBoard.Instance;
            string result = null;
            using (var reader = new StreamReader(@"..\..\..\KingCheckMateTest_Black.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var moves = line.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);
                    Console.Write($"Sending {moves[0]}");
                    var imr = board.RequestMove(moves[0]);
                    Console.WriteLine($"\t{imr.Message}");
                   

                    if (moves.Length == 2)
                    {
                        Console.Write($"Sending {moves[1]}");
                        var ir = board.RequestMove(moves[1]);
                        Console.WriteLine($"\t{ir.Message}");
                        result = ir.Message;
                    }

                    Console.WriteLine();

                }
            }
            Assert.IsTrue(result != null && result.Contains("Black has won the game"));
        }

        [TestMethod]
        public void TestEnpassant()
        {
            var board = ChessBoard.Instance;
            string result = null;
            using (var reader = new StreamReader(@"..\..\..\EnpassantMoves.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var moves = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    Console.Write($"Sending {moves[0]}");
                    var imr = board.RequestMove(moves[0]);
                    Console.WriteLine($"\t{imr.Message}");
                    result = imr.Message;


                    if (moves.Length == 2)
                    {
                        Console.Write($"Sending {moves[1]}");
                        var ir = board.RequestMove(moves[1]);
                        Console.WriteLine($"\t{ir.Message}");
                        result = ir.Message;
                    }

                    Console.WriteLine();

                }
            }
            Assert.IsTrue(result != null && result.Contains("en passant square"));
        }

        [TestMethod]
        public void TestKingCheck()
        {
            var board = ChessBoard.Instance;
            string result = null;
            using (var reader = new StreamReader(@"..\..\..\KingCheckMoves.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var moves = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    Console.Write($"Sending {moves[0]}");
                    var imr = board.RequestMove(moves[0]);
                    Console.WriteLine($"\t{imr.Message}");
                    result = imr.Message;


                    if (moves.Length == 2)
                    {
                        Console.Write($"Sending {moves[1]}");
                        var ir = board.RequestMove(moves[1]);
                        Console.WriteLine($"\t{ir.Message}");
                        result = ir.Message;
                    }

                    Console.WriteLine();

                }
            }
            Assert.IsTrue(result != null && result.Contains("White is in check"));
        }
    }
}