using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuckReach.ECommerce.API.ViewModel;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data;
using QuickReach.ECommerce.Infra.Data.Repositories;
using Dapper;

namespace QuckReach.ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository repository;
        private readonly IProductRepository productRepository;
        private readonly ECommerceDbContext context;
        public CategoriesController(ICategoryRepository repository, IProductRepository productRepository,ECommerceDbContext context)
        {
            this.repository = repository;
            this.productRepository = productRepository;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get(string search=" ",int skip=0, int count=100)
        {
            var categories = this.repository.Retrieve(search,skip,count);
            return Ok(categories);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = this.repository.Retrieve(id);
            return Ok(category);
        }

        //POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Category newCategory)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }
            this.repository.Create(newCategory);
            return CreatedAtAction(nameof(this.Get),new { id = newCategory.ID }, newCategory);
        }

        //PUT api/values
        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] Category category)
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
            this.repository.Update(id, category);
            return Ok(category);
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

        [HttpPut("{categoryid}/products")]
        public IActionResult PutCategoryProduct(int categoryID, [FromBody] ProductCategory entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (entity.CategoryID!=categoryID)
            {
                return BadRequest();
            }

            var category = repository.Retrieve(categoryID);
            if (category == null)
            {
                return NotFound();
            }

            if (productRepository.Retrieve(entity.ProductID) == null)
            {
                return NotFound();
            }
            category.AddProduct(entity);
            repository.Update(categoryID, category);
            return Ok(category);
        }
        [HttpPut("{id}/products/{productId}")]
        public IActionResult DeleteCategoryProduct(int id, int productId)
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
        public IActionResult GetProductsByCategory(int id)
        {
            var parameter = new SqlParameter("@categoryId", id);
            var connectionString ="Server=.;Database=QuickReachDb;Integrated Security=true;";
            var connection = new SqlConnection(connectionString);
            var sql = @"SELECT p.ID,
                               pc.ProductID, 
                               pc.CategoryID,
                               p.Name, 
                               p.Description,
                               p.Price,
                               p.ImageUrl
                    FROM Product p INNER JOIN ProductCategory pc ON p.ID = pc.ProductID
                    Where pc.CategoryID = @categoryId";
            var categories = connection.Query<SearchItemViewModel>(sql, new { categoryId = id }).ToList();
            return Ok(categories);
        }
    }
}