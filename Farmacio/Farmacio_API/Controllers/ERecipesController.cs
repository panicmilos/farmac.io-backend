using AutoMapper;
using Farmacio_API.Contracts.Requests.ERecipes;
using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Farmacio_API.Controllers
{
    [Route("e-recipes")]
    [ApiController]
    [Produces("application/json")]
    public class ERecipesController : ControllerBase
    {
        private readonly IERecipeService _eRecipeService;
        private readonly IMapper _mapper;

        public ERecipesController(IERecipeService eRecipeService, IMapper mapper)
        {
            _eRecipeService = eRecipeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns list of pharmacies that containts all medicines from eRecipe in stock with total price.
        /// </summary>
        /// <response code="200">Returns list of pharmacies with total price.</response>
        /// <response code="404">Given eRecipe doesn't exist.</response>
        [HttpGet("{eRecipeId}/pharmacies")]
        public IActionResult GetPharmaciesWithMedicines(Guid eRecipeId)
        {
            return Ok(_eRecipeService.FindPharmaciesWithMedicinesFrom(eRecipeId));
        }

        /// <summary>
        /// Sorts pharmacies with medicines for eRecipe.
        /// </summary>
        /// <response code="200">Sorted pharmacies.</response>
        [HttpGet("{eRecipeId}/pharmacies/sort")]
        public IActionResult SortPharmaciesWithMedicines(Guid eRecipeId, string criteria, bool isAsc)
        {
            return Ok(_eRecipeService.SortPharmaciesWithMedicinesFrom(eRecipeId, criteria, isAsc));
        }

        /// <summary>
        /// Creates reservation from eRecipe.
        /// </summary>
        /// <response code="200">Created reservation.</response>
        /// <response code="400">ERecipe is already used or there is not enough medicines for reservation.</response>
        /// <response code="404">Given eRecipe or pharmacy or patient is not found.</response>
        [HttpPost("to-reservation")]
        public IActionResult CreateReservationFromERecipe(CreateReservationFromERecipeRequest request)
        {
            var createReservationDTO = _mapper.Map<CreateReservationFromERecipeDTO>(request);
            var createdReservation = _eRecipeService.CreateReservationFromERecipe(createReservationDTO);

            return Ok(createdReservation);
        }
    }
}