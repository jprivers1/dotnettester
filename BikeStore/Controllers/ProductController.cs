using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using BikeDataAccess;
using System.IO;
using System.Data.SqlClient;

namespace BikeStore.Controllers
{

    public class Book
    {
        //int userId;
        public int UserId  { get; set; }
        //int id;
        public int Id { get; set; }
        //string title { get; set; }
        public string Title { get; set; }
        //string body { get; set; }
        public string Body { get; set; }
    }

    public class ProductController : ApiController
    {
        //public IEnumerable<product> Get()
        public IHttpActionResult Get()
        {
            //List<product> prods = new List<product>();
            //product p = new product();
            //p.brand_id = 1;
            //prods.Add(p);
            //return prods;
            using (BikeStoresEntities entities = new BikeStoresEntities())
            {
                //return entities.products
                //    .Include(x => x.stocks)
                //    .Include(x => x.order_items)
                //    .ToList();
            }
            HttpClient client = new HttpClient();
            HttpResponseMessage res = client.GetAsync("https://jsonplaceholder.typicode.com/posts").Result;
            IEnumerable<Book> books = null;
            if (res.IsSuccessStatusCode)
            {
                books = res.Content.ReadAsAsync<IEnumerable<Book>>().Result;
            }

            string filename = "tester.txt";
            if (books != null)
            {
                List<string> bookdetails = new List<string>();
                foreach (Book b in books) 
                {
                    bookdetails.Add($"{b.Id} {b.Title}");
                }
                File.WriteAllText(Path.GetTempPath() + filename, String.Join("\n", bookdetails));
            }
            
            string text = File.ReadAllText(Path.GetTempPath() + $"{filename}");
            List<product> prods = new List<product>();
            prods.Add(new product());
            return Ok(prods);
        }

        [HttpGet]
        public product GetProduct(int id)
        {
            using (BikeStoresEntities entities = new BikeStoresEntities())
            {
                string connectionString = @"data source = JasonASUS; initial catalog = BikeStores; Trusted_Connection = true";
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand command = new SqlCommand("select * from production.products;", con);
                SqlDataReader reader = command.ExecuteReader();
                string output;
                while (reader.Read())
                {
                    output = reader.GetValue(1) + "";
                }




                return entities.products
                    .SqlQuery("select * from production.products where product_id = 1;").ToList()[0];
                    //.Include(x => x.stocks)
                    //.Include(x => x.order_items)
                    //.FirstOrDefault(e => e.product_id == id);
            }
        }

        [HttpPut]
        public IHttpActionResult Put([FromBody]product prod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Form your request properly please!");
            }

            using (BikeStoresEntities entities = new BikeStoresEntities())
            {
                product product = entities.products.Where(e => e.product_id == prod.product_id).FirstOrDefault<product>();

                if (product != null)
                {
                    product.product_id = prod.product_id;
                    product.product_name = prod.product_name;
                    entities.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            return Ok("Saved");
        } 

        [HttpPost]
        public IHttpActionResult AddProduct([FromBody]product prod)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Form your request properly please!");
            }

            using (BikeStoresEntities entities = new BikeStoresEntities())
            {
                product product = entities.products.Where(e => e.product_id == prod.product_id).FirstOrDefault<product>();

                if (product != null)
                {
                    product.product_id = prod.product_id;
                    entities.SaveChanges();
                }
                else
                {
                    entities.products.Add(new product()
                    {
                        product_id = prod.product_id
                    });
                    entities.SaveChanges();
                }
            }
            return Ok();


        }
    }
}
