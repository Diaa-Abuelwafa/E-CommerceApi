using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceDomain.Interfaces.Caching_Service
{
    public interface ICachedService
    {
        public void Set(string Key, object Value, int ExpireMinutes);
        public string Get(string Key);
    }
}
