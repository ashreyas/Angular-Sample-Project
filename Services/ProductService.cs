using WeatherForecast_Proj.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Threading.Tasks;

namespace WeatherForecast_Proj.Services
{
    public class ProductService : IProductService
    {
        private static RestClient client;
        private static RestClient clientFilter;
        private readonly IConfiguration _config;
        public ProductService(IConfiguration config)
        {
            _config = config;

            client = new RestClient(_config.GetValue<string>("ProductServiceConfig:DataPathBase"));
            clientFilter = new RestClient(_config.GetValue<string>("ProductServiceConfig:FilterPathBase"));

            client.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable);
            clientFilter.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.CacheIfAvailable);
        }


        public IEnumerable<Product> FilterProductsFromCategories(IEnumerable<Product> products, Filter filter)
        {
            var filterCategoriesList = string.IsNullOrEmpty(filter.categories) ? Array.Empty<string>() : filter.categories.Split('|');
            float minDiscountPc = string.IsNullOrEmpty(filter.min_discount_pc) ? 0 : float.Parse(filter.min_discount_pc.Split('%')[0]);


            var filteredProducts = products.Where(x =>

                    (filterCategoriesList.Length == 0 || x.categories.Any(y => filterCategoriesList.Any(y.name.Contains)))  // filter categories
                    && (filter.min_price == null || (x.price >= filter.min_price))  // filter min_price
                    && (filter.max_price == null || (x.price <= filter.max_price))  // filter max_price
                    && (minDiscountPc <= 0 || (x.discount_amount / x.price * 100) >= minDiscountPc)  // filter by calculating minimum percent discount

                );
            return filteredProducts;
        }
        public (IEnumerable<Product>, JToken) FetchProductsData(int? page = null)
        {
            RestRequest request = new RestRequest("public-api/products/?page=" + page ?? "", Method.GET);
            IRestResponse<List<string>> response = client.Execute<List<string>>(request);
            var products = JObject.Parse(response.Data[0])["data"].ToObject<IEnumerable<Product>>();
            var metadata = JObject.Parse(response.Data[0])["meta"];
            return (products, metadata);
        }
        public Filter FetchFilterData()
        {
            RestRequest request = new RestRequest("www.creditenable.com/files/test/product-criteria.json", Method.GET);
            IRestResponse<List<string>> response = clientFilter.Execute<List<string>>(request);
            var filter = JObject.Parse(response.Data[0]).ToObject<Filter>();
            return filter;
        }
    }
}
