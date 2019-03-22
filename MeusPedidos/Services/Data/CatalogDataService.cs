using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Akavache;
using MeusPedidos.Constants;
using MeusPedidos.Contracts;
using MeusPedidos.Contracts.Data;
using MeusPedidos.Models;
using MeusPedidos.Repository;
using MeusPedidos.Services.BaseCacheService;

namespace Pedidos.Services.Data
{
    public class CatalogDataService : BaseService, ICatalogDataService
    {
        private readonly IGenericRepository _genericRepository;

        public CatalogDataService()
        {
            _genericRepository = new GenericRepository();
        }


        public async Task<IEnumerable<Products>> GetAllCatalogAsync()
        {
            List<Products> producstFromCache = await
                 GetFromCache<List<Products>>(CacheNameConstants.AllProducts);

            if (producstFromCache != null)//loaded from cache
            {
                return producstFromCache;
            }
            else
            {
                UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
                {
                    Path = ApiConstants.ProducstEndPoint
                };

                var urlDefault = "https://pastebin.com/raw/eVqp7pfX";
                var products = await _genericRepository.GetAsync<List<Products>>(urlDefault);

                //Cache.InsertObject(CacheNameConstants.AllProducts, products, DateTimeOffset.Now.AddSeconds(1));

                return products;
            }
        }


        public async Task<IEnumerable<PriceOff>> GetProductsOfTheWeekAsync()
        {
            List<PriceOff> pricesFromCache = await GetFromCache<List<PriceOff>>(CacheNameConstants.PriceOffOfTheWeek);

            if (pricesFromCache != null)//loaded from cache
            {
                return pricesFromCache;
            }

            UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.PricesOffEndPoint
            };

            var urlDefault = "https://pastebin.com/raw/R9cJFBtG";
            var prices = await _genericRepository.GetAsync<List<PriceOff>>(urlDefault);

            //Cache.InsertObject(CacheNameConstants.PriceOffOfTheWeek, prices, DateTimeOffset.Now.AddSeconds(1));

            return prices;
        }


        public async Task<IEnumerable<Categories>> GetAllCategories()
        {
            List<Categories> categoriesFromCache = await GetFromCache<List<Categories>>(CacheNameConstants.Categories);

            if (categoriesFromCache != null)//loaded from cache
            {
                return categoriesFromCache;
            }

            UriBuilder builder = new UriBuilder(ApiConstants.BaseApiUrl)
            {
                Path = ApiConstants.CategoriesEndPoint
            };

            var urlDefault = "https://pastebin.com/raw/YNR2rsWe";
            var categories = await _genericRepository.GetAsync<List<Categories>>(urlDefault);

            //Cache.InsertObject(CacheNameConstants.PriceOffOfTheWeek, categories, DateTimeOffset.Now.AddSeconds(1));

            return categories;
        }       

    }
}
