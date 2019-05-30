using System;
using Utaba.Implementations;
using Utaba.Interfaces;
using Utaba.Interfaces.IFactories;

namespace Utaba.Factories.ObjectFactories
{
    class SquareFactory : ISquareFactory
    {
        private static readonly Lazy<SquareFactory> LazySquareFactory = new Lazy<SquareFactory>( () => new SquareFactory());
        private SquareFactory() { }
        public static SquareFactory Singleton = LazySquareFactory.Value;

        public ISquare GetSquare(byte column, byte row, SquareColors color)
        {
            return new Square(column, row, color);
        }
    }
}
