using System.IdentityModel.Tokens.Jwt;

namespace TestApi.Helpers
{
    public class JwtService
    {
        private string secureKey = "this is a very big secure key to have";
        public string Generate(int id)
        {
            var SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);

            var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.Today.AddDays(1));

            var SecurityToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(SecurityToken);
        }

        public JwtSecurityToken Verify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secureKey);

            // Validate the token and retrieve the validated token
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            // Cast the validated token to JwtSecurityToken
            var jwtToken = (JwtSecurityToken)validatedToken;

            // Return the validated JwtSecurityToken
            return jwtToken;
        }

    }
}