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
    public class CartController:ControllerBase
    {
        private readonly ICartRepository repository;
        public CartController(ICartRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult GetCarts(int skip = 0, int count = 10)
        {
            var carts = this.repository.Retrieve(skip, count);
            return Ok(carts);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var cart = this.repository.Retrieve(id);
            return Ok(cart);
        }

        //POST api/values
        [HttpPut("{id}/items")]
        public IActionResult PutCartItemsToCart([FromBody] CartItem newCartItem, int id)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }
            var cart = this.repository.Retrieve(id);
            if (cart == null)
            {
                return NotFound();
            }
            cart.AddCartItem(newCartItem);
            repository.Update(id, cart);
            return Ok(cart);
        }

        [HttpGet("{id}/items")]
        public IActionResult GetCartItemsByCart(int id)
        {
            var parameter = new SqlParameter("@cartId", id);
            var connectionString = "Server=.;Database=QuickReachDb;Integrated Security=true;";
            var connection = new SqlConnection(connectionString);
            var sql = @"SELECT Cart.ID AS Expr1, 
                                          CartItem.Id, 
                                          CartItem.ProductId, 
                                          CartItem.ProductName, 
                                          CartItem.UnitPrice, 
                                          CartItem.OldUnitPrice, 
                                          CartItem.Quantity, 
                                          CartItem.CartID
                    FROM Cart INNER JOIN CartItem ON Cart.ID = CartItem.CartID
                    Where CartItem.CartID = @cartId";
            var cartItems = connection.Query<SearchItemViewModel>(sql, new { cartId = id }).ToList();
            return Ok(cartItems);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Cart newCart)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }
            this.repository.Create(newCart);
            return CreatedAtAction(nameof(this.GetCarts), new { id = newCart.ID }, newCart);
        }

        ////PUT api/values
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] Category category)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    var entity = this.repository.Retrieve(id);
        //    if (entity == null)
        //    {
        //        return NotFound();
        //    }
        //    this.repository.Update(id, category);
        //    return Ok(category);
        //}


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

        //[HttpPut("{id}/products/{productId}")]
        //public IActionResult DeleteCategoryProduct(int id, int productId)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    var category = repository.Retrieve(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    if (productRepository.Retrieve(productId) == null)
        //    {
        //        return NotFound();
        //    }
        //    category.RemoveProduct(productId);
        //    repository.Update(id, category);
        //    return Ok();
        //}

        //[HttpGet("{id}/products")]
        //public IActionResult GetProductsByCategory(int id)
        //{
        //    var parameter = new SqlParameter("@categoryId", id);
        //    var connectionString = "Server=.;Database=QuickReachDb;Integrated Security=true;";
        //    var connection = new SqlConnection(connectionString);
        //    var sql = @"SELECT p.ID,
        //                       pc.ProductID, 
        //                       pc.CategoryID,
        //                       p.Name, 
        //                       p.Description,
        //                       p.Price,
        //                       p.ImageUrl
        //            FROM Product p INNER JOIN ProductCategory pc ON p.ID = pc.ProductID
        //            Where pc.CategoryID = @categoryId";
        //    var categories = connection.Query<SearchItemViewModel>(sql, new { categoryId = id }).ToList();
        //    return Ok(categories);
        //}
    }
}
