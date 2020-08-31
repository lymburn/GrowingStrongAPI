using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrowingStrongAPI.Helpers.Extensions;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GrowingStrongAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class FoodController : Controller
    {
        private readonly IFoodService _foodService;

        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }

        //TODO: Add pagination support
        [HttpGet]
        public IActionResult GetFoodsByFullTextSearch([FromQuery] string query)
        {
            GetFoodsByFullTextSearchResponse response = _foodService.GetFoodsByFullTextSearch(query);

            if (!response.ResponseStatus.HasError())
            {
                return Ok(response.FoodDtos);
            }

            return StatusCode(response.ResponseStatus.Status, response.ResponseStatus.Message);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateFood([FromBody] FoodDto foodDto)
        {
            CreateFoodResponse response = _foodService.CreateFood(foodDto);

            if (!response.ResponseStatus.HasError())
            {
                return Ok();
            }

            return StatusCode(response.ResponseStatus.Status, response.ResponseStatus.Message);
        }
    }
}
