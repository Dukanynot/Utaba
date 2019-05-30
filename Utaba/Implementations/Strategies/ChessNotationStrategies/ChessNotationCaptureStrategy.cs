using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utaba.Interfaces;

namespace Utaba.Implementations.Strategies.ChessNotationStrategies
{
    class ChessNotationCaptureStrategy : IChessNotationStrategy
    {
        public IChessCommand CreateCommand(string notation, Teams whosMove)
        {
            throw new NotImplementedException();
        }
    }
}
