using System;
using Utaba.Interfaces;

namespace Utaba.Implementations
{
    class MoveResponse : IMoveResponse
    {
        public string Message { get; set; }

        public IPiece PieceCaptured
        {
            get { throw new NotImplementedException(); }
        }

        public bool SuccessfulMove { get; set; }
    }
}
