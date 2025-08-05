using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(50, ErrorMessage = "Title cannot exceed 50 characters.")]
        public string Title { get; set; } = "";

        [Column(TypeName = "nvarchar(5)")]
        [StringLength(5, ErrorMessage = "Icon cannot exceed 5 characters.")]
        public string Icon { get; set; } = "";

        [Column(TypeName = "nvarchar(10)")]
        [Required(ErrorMessage = "Type is required.")]
        [RegularExpression("^(Income|Expense)$", ErrorMessage = "Type must be either 'Income' or 'Expense'.")]
        public string Type { get; set; } = "Expense";

        [NotMapped]
        public string? TitleWithIcon
        {
            get
            {
                return string.IsNullOrEmpty(Icon) ? Title : $"{Icon} {Title}";
            }
        }

        // Navigation property
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}