using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SoftwareMind.Core.Helpers;

public interface IJwtAuthenticationManager
{
    IDictionary<string, string> UsersRefreshTokens { get; set; }
    AuthenticationResponse Authenticate(string username, string password);
    AuthenticationResponse Authenticate(string username, Claim[] claims);
}

public class JwtAuthenticationManager : IJwtAuthenticationManager
{
    private readonly IRefreshTokenGenerator refreshTokenGenerator;

    private readonly string tokenKey;

    private readonly IDictionary<string, string> userRoles = new Dictionary<string, string>
    {
        {"Kuba", "Administrator"},
        {"Karol", "Employee"}
    };


    private readonly IDictionary<string, string> users = new Dictionary<string, string>
    {
        {"Kuba", "Kuba"},
        {"Karol", "Karol"}
    };

    public JwtAuthenticationManager(string tokenKey, IRefreshTokenGenerator refreshTokenGenerator)
    {
        this.tokenKey = tokenKey;
        this.refreshTokenGenerator = refreshTokenGenerator;
        UsersRefreshTokens = new Dictionary<string, string>();
    }

    public IDictionary<string, string> UsersRefreshTokens { get; set; }

    public AuthenticationResponse Authenticate(string username, Claim[] claims)
    {
        var token = GenerateTokenString(username, DateTime.UtcNow, claims);
        var refreshToken = refreshTokenGenerator.GenerateToken();

        if (UsersRefreshTokens.ContainsKey(username))
            UsersRefreshTokens[username] = refreshToken;
        else
            UsersRefreshTokens.Add(username, refreshToken);

        return new AuthenticationResponse
        {
            JwtToken = token,
            RefreshToken = refreshToken
        };
    }

    public AuthenticationResponse Authenticate(string username, string password)
    {
        if (!users.Any(u => u.Key == username && u.Value == password)) return null;

        var token = GenerateTokenString(username, DateTime.UtcNow);
        var refreshToken = refreshTokenGenerator.GenerateToken();

        if (UsersRefreshTokens.ContainsKey(username))
            UsersRefreshTokens[username] = refreshToken;
        else
            UsersRefreshTokens.Add(username, refreshToken);

        return new AuthenticationResponse
        {
            JwtToken = token,
            RefreshToken = refreshToken
        };
    }

    private string GenerateTokenString(string username, DateTime expires, Claim[] claims = null)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(tokenKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new(ClaimTypes.Name, username),

                    new(ClaimTypes.Role, userRoles.First(x => x.Key == username).Value)
                }),
            //NotBefore = expires,
            Expires = expires.AddMinutes(15),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
}