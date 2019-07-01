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
    public class CartController:ControllerBase
    {
        private readonly ICartRepository _repository;
        public CartController(ICartRepository repository)
        {
            this._repository = repository;
        }
    }
}
