namespace Utaba.Interfaces
{
    public interface IMoveResponse
    {
        string Message { get; }
        bool SuccessfulMove { get; }
        IPiece PieceCaptured { get; }
    }
}
