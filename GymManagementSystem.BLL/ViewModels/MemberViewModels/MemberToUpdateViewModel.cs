using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.MemberViewModels
{
    public class MemberToUpdateViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Photo { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone number must be a valid Egyptian mobile number")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Building Number is required")]
        [Range(1, 9999, ErrorMessage = "Building Number must be greater than 0")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City must be between 2 and 30 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only contain letters and spaces")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Street is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 30 characters")]
        public string Street { get; set; } = null!;
    }
}

