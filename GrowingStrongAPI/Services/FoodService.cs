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

        public GetFoodsByFullTextSearchResponse GetFoodsByFullTextSearch(string query)
        {
            GetFoodsByFullTextSearchResponse response = new GetFoodsByFullTextSearchResponse();

            if (string.IsNullOrEmpty(query))
            {
                response.ResponseStatus.SetError(ResponseStatusCode.BAD_REQUEST,
                                                 Constants.GetFoodsStartingWithPatternMessages.InvalidQuery);

                return response;
            }

            try
            {
                List<Food> foods = _foodRepository.GetFoodsByFullTextSearch(query);

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

        public CreateFoodResponse CreateFood(FoodDto foodDto)
        {
            CreateFoodResponse response = new CreateFoodResponse();

            try
            {
                _foodRepository.CreateFood(foodDto);

                response.ResponseStatus.SetOk();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 Constants.CreateFoodMessages.FailedToCreateFood);
            }

            return response;
        }
    }
}
