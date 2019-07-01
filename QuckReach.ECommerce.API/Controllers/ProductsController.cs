using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuckReach.ECommerce.API.ViewModel;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data;
using QuickReach.ECommerce.Infra.Data.Repositories;

namespace QuckReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository repository;
        private readonly ICategoryRepository categoryRepository;
        public ProductsController(IProductRepository repository, ICategoryRepository categoryRepository)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Get(string search = " ", int skip = 0, int count = 10)
        {
            var products = this.repository.Retrieve(search, skip, count);
            return Ok(products);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = this.repository.Retrieve(id);
            return Ok(product);
        }

        //POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Product newProduct)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }
            this.repository.Create(newProduct);
            return CreatedAtAction(nameof(this.Get), new { id = newProduct.ID }, newProduct);
        }

        //POST api/values
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {

            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }
            var entity = this.repository.Retrieve(id);
            if (entity == null)
            {
                return NotFound();
            }
            this.repository.Update(id, product);
            return Ok(product);
        }


        //DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = this.repository.Retrieve(id);
            if (entity == null)
            {
                return NotFound();
            }
            this.repository.Delete(id);
            return Ok();
        }

        
    }
}