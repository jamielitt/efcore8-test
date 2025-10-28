using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.EntityModels;

public class Category
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }

    [Column(TypeName = "ntext")]
    public string Description { get; set; } = null!;
    
    public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
}