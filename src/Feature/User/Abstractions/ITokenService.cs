namespace User.Abstractions
{
    public interface ITokenService
    {
        string GenerateToken(int size);
    }
}