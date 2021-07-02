using WeatherForecast_Proj.Models;
using WeatherForecast_Proj.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WeatherForecast_Proj.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        private readonly IProductService _productService;
        private readonly ILogger _logger;
        [HttpGet]
        [Route("GetFilteredProducts")]
        public IEnumerable<Product> GetFilteredProducts()
        {

            #region INIT_API_CALL

            var init_api_data = _productService.FetchProductsData();
            var products = init_api_data.Item1;

            #endregion
            try
            {

                List<Product> filteredProducts = new List<Product>();
                int pages = init_api_data.Item2["pagination"]["pages"].ToObject<int>();
                Filter filter = _productService.FetchFilterData();
                if (pages > 0)
                {
                    // skip fetching data again
                    filteredProducts.AddRange(_productService.FilterProductsFromCategories(products, filter));
                    for (int i = 2; i <= pages; i++)
                    {
                        // fetch data from next page

                        filteredProducts.AddRange(_productService.FilterProductsFromCategories(_productService.FetchProductsData(i).Item1, filter));
                    }
                }
                return filteredProducts;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in " + GetType().FullName + MethodBase.GetCurrentMethod() + " Exception :" + ex.ToString());
                return Enumerable.Empty<Product>();
            }
        }

    }
}
