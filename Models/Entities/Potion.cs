using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HogwartsPotions.Models.Enums;

namespace HogwartsPotions.Models.Entities;

public class Potion
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [Required]
    public Student Brewer { get; set; }
    
    public string Name => $"{Brewer.Name}'s Potions";
    
    public HashSet<Ingredient> Ingredients { get; set; } = new();
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BrewingStatus BrewingStatus { get; set; } = BrewingStatus.Brew;
    public Recipe Recipe { get; set; }
}