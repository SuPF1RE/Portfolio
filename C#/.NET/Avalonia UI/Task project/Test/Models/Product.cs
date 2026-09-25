using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
    public class Product 
    {
        [Required(ErrorMessage = "Name is required")] public string Name { get; set; } = "";
        [Range(0.01, 10000, ErrorMessage = "Should be > 0")] public decimal Price { get; set; }
        public string Description { get; set; } = "";
        public ObservableCollection<Variant> Variants { get; set; } = new();
    }
}
