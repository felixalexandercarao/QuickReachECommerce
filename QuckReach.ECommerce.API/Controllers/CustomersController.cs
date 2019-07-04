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
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository repository;
        public CustomersController(ICustomerRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Get(string search = " ", int skip = 0, int count = 10)
        {
            var customers = this.repository.Retrieve(search, skip, count);
            return Ok(customers);
        }


        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var customers = this.repository.Retrieve(id);
            return Ok(customers);
        }

        //POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Customer newCustomer)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }
            this.repository.Create(newCustomer);
            return CreatedAtAction(nameof(this.Get), new { id = newCustomer.ID }, newCustomer);
        }

        //POST api/values
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Customer customer)
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
            this.repository.Update(id, customer);
            return Ok(customer);
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