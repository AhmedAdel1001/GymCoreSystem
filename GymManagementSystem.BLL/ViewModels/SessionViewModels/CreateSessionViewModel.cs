using System;
using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem.BLL.ViewModels.SessionViewModels
{
    public class CreateSessionViewModel
    {
        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 500 characters")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 25, ErrorMessage = "Capacity must be between 1 and 25")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date & Time")]
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(1);

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date & Time")]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1).AddHours(1);

        [Required(ErrorMessage = "Trainer is required")]
        [Display(Name = "Trainer")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid trainer")]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid category")]
        public int CategoryId { get; set; }
    }
}

