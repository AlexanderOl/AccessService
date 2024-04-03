
using AccessService.Models;
using AutoMapper;

namespace AccessService.Services
{
    public class AuthorizeService(CacheService cacheService, IMapper mapper)
    {
        public List<UserApiTokenView> GetAllTokens(Guid userId)
        {
            var allTokenDetails = cacheService.GetAllApiTokenDetails(userId);

            var views = mapper.Map<List<UserApiTokenView>>(allTokenDetails);

            return views;
        }


        public bool RevokeApiToken(Guid userId, Guid apiToken)
        {
            var success = cacheService.DisableApiToken(userId, apiToken);

            return success;
        }
    }
}
