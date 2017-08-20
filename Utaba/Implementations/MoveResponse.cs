using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utaba.Interfaces;

namespace Utaba.Implementations
{
    class MoveResponse : IMoveResponse
    {
        public string ErrorMessage { get; set; }

        public IPiece PieceCaptured
        {
            get { throw new NotImplementedException(); }
        }

        public bool SuccessfulMove { get; set; }
    }
}
