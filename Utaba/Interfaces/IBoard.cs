using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utaba.Interfaces
{
    interface IBoard
    {
      //  List<ISquare> ListOfSquares { get; }
        List<IPiece> ListOfPieces { get; }
    }
}
