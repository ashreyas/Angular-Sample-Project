using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherForecast_Proj.Models;
using Newtonsoft.Json.Linq;

namespace WeatherForecast_Proj.Services
{
    public interface IProductService
    {
        public IEnumerable<Product> FilterProductsFromCategories(IEnumerable<Product> products, Filter filter);
        public (IEnumerable<Product>, JToken) FetchProductsData(int? page = null);
        public Filter FetchFilterData();
    }
}
