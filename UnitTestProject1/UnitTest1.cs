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
        public void TestPawnMoves()
        {
            var board = new Board();
            using (var reader = new StreamReader(@"..\..\..\EnpassantMoves.txt"))
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
                    }

                    Console.WriteLine();

                }
            }
        }
    }
}