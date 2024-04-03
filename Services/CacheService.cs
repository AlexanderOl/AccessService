using AccessService.Models;
using AccessService.Models.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace AccessService.Services
{
    public class CacheService(IMemoryCache cache)
    {
        private const string ApiTokenDetailsKey = "ApiTokenDetails_{0}";
        private const string UserApiTokenKey = "UserApiToken_{0}";

        private static MemoryCacheEntryOptions OneMonthOptions =>
            new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(31));
        public void AddApiKey(Guid userId, ApiTokenDetails apiTokenDetails)
        {
            var tokenDetailsKey = string.Format(ApiTokenDetailsKey, userId);
            if (cache.TryGetValue(tokenDetailsKey, out List<ApiTokenDetails>? cachedTokens))
            {
                cachedTokens!.Add(apiTokenDetails);
                cache.Set(tokenDetailsKey, cachedTokens, OneMonthOptions);
            }
            else
            {
                cache.Set(tokenDetailsKey, new List<ApiTokenDetails> { apiTokenDetails }, OneMonthOptions);
            }

            var tokenUserKey = string.Format(UserApiTokenKey, apiTokenDetails.Token);
            cache.Set(tokenUserKey, userId, OneMonthOptions);
        }

        public ApiTokenDetails? GetApiTokenDetails(Guid apiToken)
        {
            var userApiTokenKey = string.Format(UserApiTokenKey, apiToken);
            if (!cache.TryGetValue(userApiTokenKey, out Guid userId)) return null;

            var tokenDetailsKey = string.Format(ApiTokenDetailsKey, userId);
            if (!cache.TryGetValue(tokenDetailsKey, out List<ApiTokenDetails>? cachedTokens)) return null;

            var tokenDetails = cachedTokens?.FirstOrDefault(f => f.Token == apiToken);
            if (tokenDetails == null) return null;

            tokenDetails.LastUsage = DateTime.Now;
            cache.Set(tokenDetailsKey, cachedTokens, OneMonthOptions);
            return tokenDetails;
        }

        public bool DisableApiToken(Guid userId, Guid apiToken)
        {
            var tokenDetailsKey = string.Format(ApiTokenDetailsKey, userId);
            if (!cache.TryGetValue(tokenDetailsKey, out List<ApiTokenDetails>? cachedTokens)) return false;

            var tokenDetails = cachedTokens?.FirstOrDefault(f => f.Token == apiToken);
            if (tokenDetails == null) return false;

            tokenDetails.LastUsage = DateTime.Now;
            tokenDetails.Status = ApiTokenStatus.Inactive;
            cache.Set(tokenDetailsKey, cachedTokens, OneMonthOptions);
            return true;
        }


        public List<ApiTokenDetails>? GetAllApiTokenDetails(Guid userId)
        {
            var tokenDetailsKey = string.Format(ApiTokenDetailsKey, userId);

            return cache.TryGetValue(tokenDetailsKey, out List<ApiTokenDetails>? cachedTokens) ? cachedTokens : [];
        }
    }
}
