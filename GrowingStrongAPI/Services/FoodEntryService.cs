using System;
using System.Linq;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.DataAccess;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Helpers;
using GrowingStrongAPI.Helpers.Extensions;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace GrowingStrongAPI.Services
{
    public class FoodEntryService: IFoodEntryService
    {
        private IFoodEntryRepository _foodEntryRepository;
        private IMapper _mapper;
        private readonly ILogger _logger;

        public FoodEntryService(IFoodEntryRepository foodEntryRepository,
                                IMapper mapper,
                                ILogger<IFoodEntryService> logger)
        {
            _foodEntryRepository = foodEntryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public void UpdateFoodEntry(int foodEntryId, FoodEntryUpdateModel updateModel)
        {
            _foodEntryRepository.UpdateFoodEntry(foodEntryId, updateModel);
        }
    }
}
