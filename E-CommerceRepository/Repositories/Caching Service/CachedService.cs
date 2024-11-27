using E_CommerceDomain.Interfaces.Caching_Service;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_CommerceRepository.Repositories.Caching_Service
{
    public class CachedService : ICachedService
    {
        private IDatabase Redis;
        public CachedService(IConnectionMultiplexer Redis)
        {
            this.Redis = Redis.GetDatabase();
        }
        public string Get(string Key)
        {
            return Redis.StringGet(Key);
        }

        public void Set(string Key, object Value, int ExpireMinutes)
        {
            if(Value is not null)
            {
                var SerializeOptions = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var JsonData = JsonSerializer.Serialize(Value, SerializeOptions);

                Redis.StringSet(Key, JsonData, TimeSpan.FromMinutes(ExpireMinutes));
            }
        }
    }
}
