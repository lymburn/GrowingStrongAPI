using System;
using System.Collections.Generic;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.DataAccess;
using GrowingStrongAPI.Helpers;
using GrowingStrongAPI.Helpers.Extensions;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace GrowingStrongAPI.Services
{
    public class FoodService : IFoodService
    {
        private IFoodRepository _foodRepository;
        private readonly ILogger _logger;
        private IMapper _mapper;

        public FoodService(IFoodRepository foodRepository,
                           ILogger<FoodService> logger,
                           IMapper mapper)
        {
            _foodRepository = foodRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public GetFoodsStartingWithPatternResponse GetFoodsStartingWithPattern(string pattern)
        {
            GetFoodsStartingWithPatternResponse response = new GetFoodsStartingWithPatternResponse();

            if (string.IsNullOrEmpty(pattern))
            {
                response.ResponseStatus.SetError(ResponseStatusCode.BAD_REQUEST,
                                                 Constants.GetFoodsStartingWithPatternMessages.InvalidPattern);

                return response;
            }

            try
            {
                List<Food> foods = _foodRepository.GetFoodsStartingWithPattern(pattern);

                List<FoodDto> foodDtos = _mapper.Map<List<FoodDto>>(foods);

                response.FoodDtos = foodDtos;

                response.ResponseStatus.SetOk();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.GetFoodsStartingWithPatternMessages.FailedToGetFoods);
            }

            return response;
        }
    }
}
