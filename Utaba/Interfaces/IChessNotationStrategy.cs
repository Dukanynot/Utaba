namespace Utaba.Interfaces
{
    interface IChessNotationStrategy
    {
        IChessCommand CreateCommand( Teams whosMove);
        string Notation { get; set; }
    }
}
