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
using System.Net.Http;

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
        [Route("GetFilteredProducts/{pageNo}/{pageLimit}")]
        public ProductsResponse GetFilteredProducts(int pageNo, int pageLimit)
        {
            ProductsResponse response = new ProductsResponse();

            #region INIT_API_CALL

               var init_api_data = _productService.FetchProductsData();
            var products = init_api_data.Item1;

            #endregion
            try
            {

                List<Product> filteredProducts = new List<Product>();
                int pages = init_api_data.Item2["pagination"]["pages"].ToObject<int>();
                response.totalCount = init_api_data.Item2["pagination"]["total"].ToObject<int>();
                Filter filter = _productService.FetchFilterData();
                if (pages > 0)
                {
                    // skip fetching data again
                    filteredProducts.AddRange(_productService.FilterProductsFromCategories(products, filter));
                    List<int> pagesList = new List<int>();

                    for (int i = 2; i <= pages; i++)
                    {
                        pagesList.Add(i);
                        // fetch data from next page
                        //filteredProducts.AddRange(_productService.FilterProductsFromCategories(_productService.FetchProductsData(i).Item1, filter));
                    }

                    // Fetch records parallelly to improve load time
                    // twice performance improvement 6sec to 2.77sec
                    var tasks = pagesList.Select(i => Task<List<Product>>.Factory.StartNew(() => _productService.FilterProductsFromCategories(_productService.FetchProductsData(i).Item1, filter).ToList())).ToArray();

                    Task.WaitAll(tasks);
                    foreach (var task in tasks)
                    {
                        filteredProducts.AddRange(task.Result);
                    }
                }

                //filter data based on pageNo and pageLimit
                if (filteredProducts.Count() <= pageLimit)
                    response.products = filteredProducts;
                //new JSON(filteredProducts);
                else
                    response.products = filteredProducts.Skip(pageLimit * (pageNo - 1)).Take(pageLimit).ToList();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in " + GetType().FullName + MethodBase.GetCurrentMethod() + " Exception :" + ex.ToString());
                return response;
            }
        }

    }
}
