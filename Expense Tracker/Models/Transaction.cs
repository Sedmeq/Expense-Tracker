using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Expense_Tracker.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        [Display(Name = "Category")]
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
        [CustomValidation(typeof(Transaction), nameof(ValidateDate))]
        public DateTime Date { get; set; } = DateTime.Today;

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

        // Custom validation method for date
        public static ValidationResult? ValidateDate(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Today)
            {
                return new ValidationResult("Date cannot be in the future.");
            }

            // Optional: Prevent very old dates (more than 5 years ago)
            if (date < DateTime.Today.AddYears(-5))
            {
                return new ValidationResult("Date cannot be more than 5 years ago.");
            }

            return ValidationResult.Success;
        }
    }
}