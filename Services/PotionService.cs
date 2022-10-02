using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Data;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Services;

public class PotionService : IPotionService
{
    private const int MaxIngredientsForPotions = 5;
    private readonly HogwartsContext _context;

    public PotionService(HogwartsContext context)
    {
        _context = context;
    }

    public async Task<Potion> AddPotion(Potion newPotion)
    {
        newPotion.Brewer = await _context.Students.FirstOrDefaultAsync(student => student.Id == newPotion.Brewer.Id);

        var isReplica = PotionIsReplica(newPotion);
        newPotion.BrewingStatus = isReplica ? BrewingStatus.Replica : BrewingStatus.Discovery;

        if (isReplica)
        {
            newPotion.Recipe = GetRecipeByIngredients(newPotion.Ingredients);
        }
        else
        {
            AddDiscoveryRecipeToPotion(newPotion);
        }

        var ingredients = new HashSet<Ingredient>();
        foreach (var newPotionIngredient in newPotion.Ingredients)
        {
            var ingredient =
                _context.Ingredients.AsNoTracking()
                    .FirstOrDefault(ingredient1 => ingredient1.Name == newPotionIngredient.Name);
            ingredients.Add(ingredient ?? newPotionIngredient);
        }

        newPotion.Ingredients = ingredients;
        var potion = _context.Potions.Add(newPotion).Entity;
        await _context.SaveChangesAsync();

        return potion;
    }

    public async Task<Potion> GetPotion(long potionId)
    {
        return await _context.Potions.FirstOrDefaultAsync(potion => potion.Id == potionId);
    }

    public async Task DeletePotion(long id)
    {
        var potion = await GetPotion(id);
        _context.Potions.Remove(potion);
    }

    public async Task UpdatePotion(Potion potion)
    {
        _context.Potions.Update(potion);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Potion>> GetAllPotion()
    {
        return await _context.Potions
            .Include(potion => potion.Ingredients)
            .Include(potion => potion.Brewer)
            .Include(potion => potion.Recipe)
            .AsSplitQuery()
            .Include(potion => potion.Recipe.Ingredients)
            .Include(potion => potion.Recipe.Brewer)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Potion>> GetAllPotionByStudent(long id)
    {
        return await _context.Potions
            .Where(potion => potion.Brewer.Id == id)
            .Include(potion => potion.Ingredients)
            .Include(potion => potion.Brewer)
            .Include(potion => potion.Recipe)
            .AsSplitQuery()
            .Include(potion => potion.Recipe.Brewer)
            .Include(potion => potion.Recipe.Ingredients)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Potion> AddBrewPotion(Student student)
    {
        var potion = new Potion()
        {
            Brewer = _context.Students.FirstOrDefault(student1 => student1.Id == student.Id),
            BrewingStatus = BrewingStatus.Brew,
            Ingredients = new HashSet<Ingredient>()
        };

        var potionEntity = _context.Potions.Add(potion).Entity;
        await _context.SaveChangesAsync();

        return potionEntity;
    }

    public async Task<Potion> BrewPotion(long potionId, Ingredient ingredient)
    {
        var potion = await _context.Potions
            .Include(potion1 => potion1.Brewer)
            .Include(potion1 => potion1.Ingredients)
            .FirstOrDefaultAsync(potion1 => potion1.Id == potionId);

        if (potion is null || potion.Ingredients.Count >= 5)
        {
            return null;
        }

        var newIngredient = _context.Ingredients
            .FirstOrDefault(ingredient1 => ingredient1.Name == ingredient.Name);
        
        potion.Ingredients.Add(newIngredient ?? ingredient);

        if (potion.Ingredients.Count >= MaxIngredientsForPotions)
        {
            var isReplica = PotionIsReplica(potion);
            potion.BrewingStatus = isReplica ? BrewingStatus.Replica : BrewingStatus.Discovery;

            if (isReplica)
            {
                potion.Recipe = GetRecipeByIngredients(potion.Ingredients);
            }
            else
            {
                AddDiscoveryRecipeToPotion(potion);
            }
        }

        var updatedPotion = _context.Potions.Update(potion).Entity;
        await _context.SaveChangesAsync();

        return updatedPotion;
    }

    public async Task<List<Recipe>> GetSimilarRecipes(long potionId)
    {
        var potion = await _context.Potions
            .Include(potion1 => potion1.Ingredients)
            .AsNoTracking()
            .FirstOrDefaultAsync(potion1 => potion1.Id == potionId);
        var ingredientNames = potion!.Ingredients.Select(ingredient => ingredient.Name);
        return await _context.Recipes
            .Include(recipe => recipe.Ingredients)
            .Where(recipe => recipe.Ingredients
                .Select(ingredient => ingredient.Name)
                .Any(s => ingredientNames.Contains(s)))
            .ToListAsync();
    }

    private bool PotionIsReplica(Potion newPotion)
    {
        return _context.Recipes
            .Include(recipe => recipe.Ingredients)
            .AsEnumerable()
            .Any(recipe => recipe.Ingredients
                .Select(ingredient => ingredient.Name)
                .OrderBy(x => x)
                .SequenceEqual(newPotion.Ingredients.Select(ingredient => ingredient.Name).OrderBy(y => y)));
    }

}