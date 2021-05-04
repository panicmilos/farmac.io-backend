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

        public ERecipesController(IERecipeService eRecipeService)
        {
            _eRecipeService = eRecipeService;
        }

        /// <summary>
        /// Returns list of pharmacies that containt all medicines from eRecipe in stock with total price.
        /// </summary>
        /// <response code="200">Returns list of pharmacies with total price.</response>
        /// <response code="404">Given eRecipe doesn't exist.</response>
        [HttpGet("{eRecipeId}/pharmacies")]
        public IActionResult GetPharmaciesWithMedicines(Guid eRecipeId)
        {
            return Ok(_eRecipeService.FindPharmaciesWithMedicinesFrom(eRecipeId));
        }
    }
}