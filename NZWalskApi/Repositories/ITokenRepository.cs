

using Microsoft.AspNetCore.Identity;

namespace NZWalskApi.Repositories
{
    public interface ITokenRepository
    {
       string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
