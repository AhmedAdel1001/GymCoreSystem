using GymManagementSystem.DAL.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;

namespace GymManagementSystem.DAL.Entities
{
    [Owned]
    public class Address
    {
        public int BuildingNumber { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }

    public abstract class GymUser : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; } = new Address();
    }
}

