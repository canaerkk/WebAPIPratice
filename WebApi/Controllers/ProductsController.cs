using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private FabricsEntities db = new FabricsEntities();

        //ctor ==>constractor
        public ProductsController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }
        /// <summary>
        /// 取得所有產品
        /// </summary>
        /// <returns></returns>
        // GET: api/Products
        [Route("")]
        public IQueryable<Product> GetProduct()
        {
            return db.Product.OrderByDescending(p => p.ProductId).Take(10);
        }
        /// <summary>
        /// 取得單一商品
        /// </summary>
        /// <param name="id">商品id</param>
        /// <returns></returns>
        // GET: api/Products/5
        [Route("{id}", Name = "GetProductById")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/Products/5
        [Route("{id:int}/orderlines")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProductOrderlines(int id)
        {
            Product product = db.Product.Include("OrderLine").FirstOrDefault(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product.OrderLine.ToList());
        }

        [Route("{id}")]
        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Products
        [Route("")]
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Product.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("GetPRoductById", new { id = product.ProductId }, product);
        }
        [Route("{id}")]
        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Product.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Product.Count(e => e.ProductId == id) > 0;
        }
    }
}