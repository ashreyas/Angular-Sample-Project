using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherForecast_Proj.Models
{
    public class Product
    {
        //  "id": 82,
        //"name": "Heavy Duty Plastic Keyboard",
        //"description": "Tepesco caelum viscus tamisium coniuratio vester traho asperiores canonicus ara inventore acceptus dens casus nobis corrigo decerno acervus amitto sequi tantillus administratio umquam adhuc arbustum stella.",
        //"image": "https://loremflickr.com/250/250",
        //"price": "8157.89",
        //"discount_amount": "340.08",
        //"status": true,
        //"categories": [{"id":2,"name":"Automotive"},{"id":1,"name":"Shoes"}]

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public float? price { get; set; }
        public float? discount_amount { get; set; }
        public string status { get; set; }
        public List<Category> categories { get; set; }
    }
}

public class Category
{
    //"id": 10,
    //"name": "Sports"
    public int id { get; set; }
    public string name { get; set; }
}

public class Filter
{
    //"categories":"Games|Electronics",
    //"min_discount_pc": "10%",
    //"min_price":25000,
    //"max_price":30000
    public string categories { get; set; }
    public string min_discount_pc { get; set; }
    public float? min_price { get; set; }
    public float? max_price { get; set; }
}
