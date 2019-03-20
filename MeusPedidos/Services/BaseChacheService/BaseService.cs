using Akavache;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Linq;//importante essa Biblioteca cuidado ao atualizar o pacote akavache
using System;
using MeusPedidos.Models;

namespace MeusPedidos.Services.BaseCacheService
{
    public class BaseService
    {
        protected IBlobCache Cache;

        public BaseService()
        {
            Cache = BlobCache.LocalMachine;
        }

        public BaseService(IBlobCache cache)
        {
            Cache = cache ?? BlobCache.LocalMachine;
        }

        public async Task<T> GetFromCache<T>(string cacheName)
        {
            try
            {
                T t = await Cache.GetObject<T>(cacheName);
                return t;
            }
            catch (KeyNotFoundException)
            {
                return default(T);
            }
        }

        public IEnumerable<Categories> GetCategoriesMenu() 
        {

            return null;
        }

        public async Task InsertObject<T>(string key, T value, System.DateTimeOffset dateTimeOffset)
        {
            await Cache.InsertObject(key, value, dateTimeOffset);
        }

       
        public void InvalidateCache(string key)
        {
            Cache.Invalidate(key);
        }

      
        public async Task InsertObjectNoTimeCache<T>(string key, T value)
        {
            await Cache.InsertObject(key, value);
        }

        public async Task InsertObjecItemsCache(Dictionary<string, string> dicionarioTodo)
        {
            await Cache.InsertObjects(dicionarioTodo);
        }
    }
}
