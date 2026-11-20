using GymManagementSystem.DAL.Contexts;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Entities.Enums;
using System;
using System.Linq;

namespace GymManagementSystem.DAL.DataSeed
{
    public static class GymDataSeeder
    {
        public static void Seed(GymDbContext context)
        {
            if (!context.Trainers.Any())
            {
                context.Trainers.AddRange(
                    new Trainer
                    {
                        Name = "Ahmed Hassan",
                        Email = "ahmed.hassan@gym.com",
                        Phone = "01012345678",
                        DateOfBirth = new DateOnly(1992, 5, 14),
                        Gender = Gender.Male,
                        Specialty = Specialties.GeneralFitness,
                        HireDate = DateTime.Now.AddYears(-2),
                        Address = new Address { BuildingNumber = 10, Street = "El Tahrir St", City = "Cairo" }
                    },
                    new Trainer
                    {
                        Name = "Sara Mohamed",
                        Email = "sara.mohamed@gym.com",
                        Phone = "01112345678",
                        DateOfBirth = new DateOnly(1995, 8, 22),
                        Gender = Gender.Female,
                        Specialty = Specialties.Yoga,
                        HireDate = DateTime.Now.AddYears(-1),
                        Address = new Address { BuildingNumber = 15, Street = "Nile Corniche", City = "Giza" }
                    },
                    new Trainer
                    {
                        Name = "Mahmoud Ali",
                        Email = "mahmoud.ali@gym.com",
                        Phone = "01212345678",
                        DateOfBirth = new DateOnly(1990, 11, 3),
                        Gender = Gender.Male,
                        Specialty = Specialties.Boxing,
                        HireDate = DateTime.Now.AddYears(-3),
                        Address = new Address { BuildingNumber = 22, Street = "Abbasia St", City = "Cairo" }
                    },
                    new Trainer
                    {
                        Name = "Omar Khaled",
                        Email = "omar.khaled@gym.com",
                        Phone = "01512345678",
                        DateOfBirth = new DateOnly(1993, 2, 19),
                        Gender = Gender.Male,
                        Specialty = Specialties.CrossFit,
                        HireDate = DateTime.Now.AddYears(-1),
                        Address = new Address { BuildingNumber = 5, Street = "El Nasr St", City = "Alexandria" }
                    }
                );
                context.SaveChanges();
            }
        }
    }
}

