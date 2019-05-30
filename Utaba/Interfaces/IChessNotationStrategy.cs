namespace Utaba.Interfaces
{
    interface IChessNotationStrategy
    {
        IChessCommand CreateCommand(string notation, Teams whosMove);
    }
}
