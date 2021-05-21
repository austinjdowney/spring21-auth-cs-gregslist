using System.ComponentModel.DataAnnotations;

namespace auth_cs_gregslist.Models
{
  public class Car
  {
    public int Id { get; set; }
    public string CreatorId { get; set; }
    // TODO[epic=Populate]
    public Account Creator { get; set; }

    [Required]
    public string Make { get; set; }
    [Required]
    public string Model { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    [Range(100, float.MaxValue)]
    public float Price { get; set; }
    public string Description { get; set; } = "No Description";
    public string ImgUrl { get; set; } = "https://placehold.it/200x200";
  }
}