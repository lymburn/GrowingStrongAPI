using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public void UpdateFoodEntry(int id, [FromBody] FoodEntryUpdateModel updateModel)
        {
            _foodEntryService.UpdateFoodEntry(id, updateModel);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
