using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrowingStrongAPI.Helpers.Extensions;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace GrowingStrongAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class FoodEntryController : Controller
    {

        private readonly IFoodEntryService _foodEntryService;
        private readonly ILogger _logger;

        public FoodEntryController (IFoodEntryService foodEntryService,
                                    ILogger<FoodEntryController> logger)
        {
            _foodEntryService = foodEntryService;
            _logger = logger;
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFoodEntry(int id, [FromBody] FoodEntryUpdateModel updateModel)
        {
            UpdateFoodEntryResponse response = _foodEntryService.UpdateFoodEntry(id, updateModel);

            if (!response.ResponseStatus.HasError())
            {
                return Ok();
            }

            return StatusCode(response.ResponseStatus.Status, response.ResponseStatus.Message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFoodEntry(int id)
        {
            DeleteFoodEntryResponse response = _foodEntryService.DeleteFoodEntry(id);

            if (!response.ResponseStatus.HasError())
            {
                return Ok();
            }

            return StatusCode(response.ResponseStatus.Status, response.ResponseStatus.Message);
        }
    }
}
