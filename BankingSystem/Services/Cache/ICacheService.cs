namespace BankingSystem.Services.Cache
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task<bool> SetAsync<T>(string key, T value, int duration);
        Task ClearAsync(string key);
        void ClearAll();
    }
}
