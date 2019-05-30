
namespace Utaba.Interfaces.IFactories
{
    interface ISquareFactory
    {
        ISquare GetSquare(byte column, byte row, SquareColors color);
    }
}
