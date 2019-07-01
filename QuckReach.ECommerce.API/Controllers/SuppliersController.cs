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
    public class SuppliersController : ControllerBase
    {
        private readonly IProductRepository productRepository;
        private readonly ISupplierRepository repository;
        public SuppliersController(ISupplierRepository repository, IProductRepository productRepository)
        {
            this.repository = repository;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Get(string search = " ", int skip = 0, int count = 10)
        {
            var suppliers = this.repository.Retrieve(search, skip, count);
            return Ok(suppliers);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var supplier = this.repository.Retrieve(id);
            return Ok(supplier);
        }

        //POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Supplier newSupplier)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }
            this.repository.Create(newSupplier);
            return CreatedAtAction(nameof(this.Get), new { id = newSupplier.ID }, newSupplier);
        }

        //POST api/values
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Supplier supplier)
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
            this.repository.Update(id, supplier);
            return Ok(supplier);
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

        [HttpPut("{supplierid}/products")]
        public IActionResult PutProductSupplier(int supplierid, [FromBody] ProductSupplier entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (entity.SupplierID != supplierid)
            {
                return BadRequest();
            }

            var supplier = repository.Retrieve(supplierid);
            if (supplier == null)
            {
                return NotFound();
            }

            if (productRepository.Retrieve(entity.ProductID) == null)
            {
                return NotFound();
            }
            supplier.AddProduct(entity);
            repository.Update(supplierid, supplier);
            return Ok(supplier);
        }

        [HttpPut("{id}/products/{productId}")]
        public IActionResult DeleteProductSupplier(int id, int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var category = repository.Retrieve(id);
            if (category == null)
            {
                return NotFound();
            }
            if (productRepository.Retrieve(productId) == null)
            {
                return NotFound();
            }
            category.RemoveProduct(productId);
            repository.Update(id, category);
            return Ok();
        }

        [HttpGet("{id}/products")]
        public IActionResult GetProductsBySupplier(int id)
        {
            var parameter = new SqlParameter("@supplierId", id);
            var connectionString = "Server=.;Database=QuickReachDb;Integrated Security=true;";
            var connection = new SqlConnection(connectionString);
            var sql = @"SELECT p.ID,
                               pc.ProductID, 
                               pc.SupplierID,
                               p.Name, 
                               p.Description,
                               p.Price,
                               p.ImageUrl
                    FROM Product p INNER JOIN ProductSupplier pc ON p.ID = pc.ProductID
                    Where pc.SupplierID = @supplierId";
            var suppliers = connection.Query<SearchItemViewModel>(sql, new { supplierId = id }).ToList();
            return Ok(suppliers);
        }
    }
}