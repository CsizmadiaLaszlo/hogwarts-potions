using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsPotions.Controllers
{
    [ApiController, Route("/potions")]
    public class PotionController : ControllerBase
    {
        private readonly IPotionService _potionService;

        public PotionController(IPotionService potionService)
        {
            _potionService = potionService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Potion>>> GetAllPotion()
        {
            var potions = await _potionService.GetAllPotion();
            if (potions == null)
            {
                return NotFound();
            }
            return Ok(potions);
        }
        
    }
}
