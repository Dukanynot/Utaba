using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utaba.Interfaces.IFactories
{
    interface IChessNotationStrategyAbstractFactory
    {
        IChessNotationStrategy GetChessNotationStrategy(string notation);
    }
}
