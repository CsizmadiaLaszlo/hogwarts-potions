using System.Collections.Generic;
using System.Linq;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;

namespace HogwartsPotions.Data;

public static class DbInitializer
{
    public static void Initialize(HogwartsContext context)
    {
        context.Database.EnsureCreated();

        if (context.Students.Any() || context.Rooms.Any() || context.Potions.Any() || context.Ingredients.Any() ||
            context.Recipes.Any())
        {
            return;
        }

        var harry = new Student
            { Name = "Harry Potter", HouseType = HouseType.Gryffindor, PetType = PetType.Owl };
        context.Students.Add(harry);
        context.SaveChanges();
        
        var herminone = new Student
            { Name = "Hermione Granger", HouseType = HouseType.Gryffindor, PetType = PetType.Cat };
        context.Students.Add(herminone);
        context.SaveChanges();
        
        var rooms = new Room[]
        {
            new() { Capacity = 5, Residents = new HashSet<Student> { harry } },
            new() { Capacity = 5, Residents = new HashSet<Student> { herminone } },
            new() { Capacity = 5, Residents = new HashSet<Student>() }
        };

        context.Rooms.AddRange(rooms);
        context.SaveChanges();

        var newtSpleens = new Ingredient { Name = "newt spleens" };
        var banana = new Ingredient { Name = "banana" };
        var orangeSnake = new Ingredient { Name = "orange snake" };
        var greenLeaf = new Ingredient { Name = "green leaf" };
        var purpleOrange = new Ingredient { Name = "purple orange" };
        context.Ingredients.Add(newtSpleens);
        context.Ingredients.Add(banana);
        context.Ingredients.Add(orangeSnake);
        context.Ingredients.Add(greenLeaf);
        context.Ingredients.Add(purpleOrange);
        context.SaveChanges();
        
        var ageingRecipe = new Recipe
        {
            Name = "Harry Potter's discovery #1",
            Ingredients = new HashSet<Ingredient>
                { newtSpleens, banana, orangeSnake, greenLeaf, purpleOrange },
            Brewer = harry
        };
        context.Recipes.Add(ageingRecipe);
        context.SaveChanges();

        var potions = new Potion[]
        {
            new()
            {
                BrewingStatus = BrewingStatus.Discovery,
                Brewer = harry,
                Recipe = ageingRecipe,
                Ingredients = new HashSet<Ingredient>
                    { newtSpleens, banana, orangeSnake, greenLeaf, purpleOrange },
            }
        };
        
        context.Potions.AddRange(potions);
        context.SaveChanges();
    }
}