using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
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
    public class ManufacturersController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly IManufacturerRepository repository;
        public ManufacturersController(IManufacturerRepository repository, IProductRepository productRepository)
        {
            this.repository = repository;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Get(string search = " ", int skip = 0, int count = 10)
        {
            var manufacturers = this.repository.Retrieve(search, skip, count);
            return Ok(manufacturers);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var manufacturer = this.repository.Retrieve(id);
            return Ok(manufacturer);
        }

        //POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Manufacturer newManufacturer)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }
            this.repository.Create(newManufacturer);
            return CreatedAtAction(nameof(this.Get), new { id = newManufacturer.ID }, newManufacturer);
        }

        //POST api/values
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Manufacturer manufacturer)
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
            this.repository.Update(id, manufacturer);
            return Ok(manufacturer);
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

        [HttpPut("{manufacturerid}/products")]
        public IActionResult PutProductManufacturer(int manufacturerid, [FromBody] ProductManufacturer entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (entity.ManufacturerID != manufacturerid)
            {
                return BadRequest();
            }

            var manufacturer = repository.Retrieve(manufacturerid);
            if (manufacturer == null)
            {
                return NotFound();
            }

            if (productRepository.Retrieve(entity.ProductID) == null)
            {
                return NotFound();
            }
            manufacturer.AddProduct(entity);
            repository.Update(manufacturerid, manufacturer);
            return Ok(manufacturer);
        }

        [HttpPut("{id}/products/{productId}")]
        public IActionResult DeleteProductManufacturer(int id, int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var manufacturer = repository.Retrieve(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            if (productRepository.Retrieve(productId) == null)
            {
                return NotFound();
            }
            manufacturer.RemoveProduct(productId);
            repository.Update(id, manufacturer);
            return Ok();
        }

        [HttpGet("{id}/products")]
        public IActionResult GetProductsByManufacturer(int id)
        {
            var parameter = new SqlParameter("@manufacturerId", id);
            var connectionString = "Server=.;Database=QuickReachDb;Integrated Security=true;";
            var connection = new SqlConnection(connectionString);
            var sql = @"SELECT p.ID,
                               pc.ProductID, 
                               pc.ManufacturerID,
                               p.Name, 
                               p.Description,
                               p.Price,
                               p.ImageUrl
                    FROM Product p INNER JOIN ProductManufacturer pc ON p.ID = pc.ProductID
                    Where pc.ManufacturerID = @manufacturerId";
            var manufacturers = connection.Query<SearchItemViewModel>(sql, new { manufacturerId = id }).ToList();
            return Ok(manufacturers);
        }
    }
}