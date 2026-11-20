using GymManagementSystem.DAL.Entities.Enums;
using System;
using System.Collections.Generic;

namespace GymManagementSystem.DAL.Entities
{
    public class Trainer : GymUser
    {
        public Specialties Specialty { get; set; }
        public DateTime HireDate { get; set; } = DateTime.Now;

        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}

