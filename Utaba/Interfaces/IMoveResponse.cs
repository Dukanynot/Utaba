namespace Utaba.Interfaces
{
    public interface IMoveResponse
    {
        string ErrorMessage { get; }
        bool SuccessfulMove { get; }
        IPiece PieceCaptured { get; }

    }
}
