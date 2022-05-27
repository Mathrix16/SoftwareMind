namespace SoftwareMind.Core.Helpers
{
    public interface ITokenRefresher
    {
        AuthenticationResponse Refresh(RefreshCred refreshCred);
    }
}