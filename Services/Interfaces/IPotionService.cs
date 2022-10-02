using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Services.Interfaces;

public interface IPotionService
{
    Task<Potion> AddPotion(Potion room);
    Task<Potion> GetPotion(long id);
    Task DeletePotion(long id);
    Task UpdatePotion(Potion room);
    Task<List<Potion>> GetAllPotion();
    Task<List<Potion>> GetAllPotionByStudent(long id);
    Task<Potion> AddBrewPotion(Student student);
    Task<Potion> BrewPotion(long potionId, Ingredient ingredient);
    Task<List<Recipe>> GetSimilarRecipes(long potionId);
}