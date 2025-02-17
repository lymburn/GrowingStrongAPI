﻿using System;
using System.Linq;
using System.Collections.Generic;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Helpers.Extensions;
using GrowingStrongAPI.Helpers;
using GrowingStrongAPI.DataAccess;
using GrowingStrongAPI.Models;
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

        public CreateFoodEntryResponse CreateFoodEntry(FoodEntryCreateModel createModel)
        {
            CreateFoodEntryResponse response = new CreateFoodEntryResponse();

            try
            {
                _foodEntryRepository.CreateFoodEntry(createModel);

                response.ResponseStatus.SetOk();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 e.ToString());
            }

            return response;
        }

        public UpdateFoodEntryResponse UpdateFoodEntry(Guid foodEntryId, FoodEntryUpdateModel updateModel)
        {
            //TODO: Handle food entry with non existent id
            UpdateFoodEntryResponse response = new UpdateFoodEntryResponse();

            try
            {
                _foodEntryRepository.UpdateFoodEntry(foodEntryId, updateModel);

                response.ResponseStatus.SetOk();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 e.ToString());
            }

            return response;
        }

        public DeleteFoodEntryResponse DeleteFoodEntry(Guid foodEntryId)
        {
            //TODO: Handle food entry with non existent id

            DeleteFoodEntryResponse response = new DeleteFoodEntryResponse();

            try
            {
                _foodEntryRepository.DeleteFoodEntry(foodEntryId);

                response.ResponseStatus.SetOk();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                response.ResponseStatus.SetError(ResponseStatusCode.INTERNAL_SERVER_ERROR,
                                                 e.ToString());
            }

            return response;
        }
    }
}
