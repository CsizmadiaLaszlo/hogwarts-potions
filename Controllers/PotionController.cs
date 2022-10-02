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
        
        [HttpPost]
        public async Task<ActionResult> AddPotion([FromBody] Potion potion)
        {
            var createdPotion = await _potionService.AddPotion(potion);
            if (createdPotion == null)
            {
                return NotFound();
            }
            return Created("Potion", createdPotion);
        }
        
        [HttpGet("{id:long}")]
        public async Task<ActionResult<Room>> GetPotionsByStudent(long id)
        {
            var potions = await _potionService.GetAllPotionByStudent(id);
            if (potions == null)
            {
                return NotFound();
            }
            return Ok(potions);
        }
        
        [HttpPost]
        [Route("brew")]
        public async Task<ActionResult> AddBrewPotion([FromBody] Student student)
        {
            var potion = await _potionService.AddBrewPotion(student);
            if (potion == null)
            {
                return BadRequest();
            }
            return Created("Potion", potion);
        }
        
        [HttpPut]
        [Route("brew/{id:long}")]
        public async Task<ActionResult> BrewPotion(long id, [FromBody] Ingredient ingredient)
        {
            var potion = await _potionService.BrewPotion(id, ingredient);
            if (potion == null)
            {
                return BadRequest();
            }
            return Created("Potion", potion);
        }
        
    }
}
