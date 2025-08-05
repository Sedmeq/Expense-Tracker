using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
        [Display(Name = "Amount")]
        public int Amount { get; set; }

        [Column(TypeName = "nvarchar(75)")]
        [StringLength(75, ErrorMessage = "Note cannot exceed 75 characters.")]
        [Display(Name = "Note")]
        public string? Note { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today; // Use Today instead of Now for date-only

        [NotMapped]
        public string? CategoryTitleWithIcon
        {
            get
            {
                return Category?.TitleWithIcon ?? "";
            }
        }

        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                if (Category == null) return Amount.ToString("C0");

                var prefix = Category.Type == "Expense" ? "- " : "+ ";
                return prefix + Amount.ToString("C0");
            }
        }
    }
}