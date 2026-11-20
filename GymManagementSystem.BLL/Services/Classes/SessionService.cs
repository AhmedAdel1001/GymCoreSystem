using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Entities.Enums;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategory();
            if (sessions == null || !sessions.Any())
                return Enumerable.Empty<SessionViewModel>();

            var mappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions).ToList();

            foreach (var session in mappedSessions)
            {
                var bookedCount = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
                session.AvailableSlots = Math.Max(0, session.Capacity - bookedCount);
            }

            return mappedSessions;
        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetSessionWithTrainerAndCategory(sessionId);
            if (session == null)
                return null;

            var mapped = _mapper.Map<SessionViewModel>(session);
            var bookedCount = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);
            mapped.AvailableSlots = Math.Max(0, mapped.Capacity - bookedCount);
            return mapped;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (session == null)
                return null;

            if (session.StartDate <= DateTime.Now)
                return null;

            return _mapper.Map<UpdateSessionViewModel>(session);
        }

        public Result CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                if (createSession.Capacity < 1 || createSession.Capacity > 25)
                    return Result.Fail("Capacity must be between 1 and 25 participants.");

                if (createSession.StartDate <= DateTime.Now)
                    return Result.Fail("Session start date must be scheduled in the future.");

                if (createSession.EndDate <= createSession.StartDate)
                    return Result.Fail("Session end date must be after the start date.");

                var trainer = _unitOfWork.GetRepository<Trainer>().GetById(createSession.TrainerId);
                if (trainer == null)
                    return Result.Fail("Selected trainer does not exist.");

                var category = _unitOfWork.GetRepository<Category>().GetById(createSession.CategoryId);
                if (category == null)
                    return Result.Fail("Selected category does not exist.");

                if (!DoesSpecialtyMatchCategory(trainer.Specialty, category.CategoryName))
                    return Result.Fail($"Trainer specialty ({trainer.Specialty}) does not match session category ({category.CategoryName}).");

                if (_unitOfWork.SessionRepository.HasTrainerOverlap(createSession.TrainerId, createSession.StartDate, createSession.EndDate))
                    return Result.Fail("Trainer already has an overlapping session during this time window.");

                var entity = _mapper.Map<Session>(createSession);
                entity.CreatedAt = DateTime.UtcNow;

                _unitOfWork.SessionRepository.Add(entity);
                var saved = _unitOfWork.SaveChanges() > 0;

                return saved ? Result.Ok() : Result.Fail("Failed to save session.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while creating session: {ex.Message}");
            }
        }

        public Result UpdateSession(int id, UpdateSessionViewModel updateSession)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(id);
                if (session == null)
                    return Result.Fail("Session not found.");

                if (session.StartDate <= DateTime.Now)
                    return Result.Fail("Cannot update a session that has already started or finished.");

                if (updateSession.Capacity < 1 || updateSession.Capacity > 25)
                    return Result.Fail("Capacity must be between 1 and 25 participants.");

                if (updateSession.StartDate <= DateTime.Now)
                    return Result.Fail("Session start date must be scheduled in the future.");

                if (updateSession.EndDate <= updateSession.StartDate)
                    return Result.Fail("Session end date must be after the start date.");

                var trainer = _unitOfWork.GetRepository<Trainer>().GetById(updateSession.TrainerId);
                if (trainer == null)
                    return Result.Fail("Selected trainer does not exist.");

                if (updateSession.CategoryId > 0)
                {
                    var category = _unitOfWork.GetRepository<Category>().GetById(updateSession.CategoryId);
                    if (category != null && !DoesSpecialtyMatchCategory(trainer.Specialty, category.CategoryName))
                        return Result.Fail($"Trainer specialty ({trainer.Specialty}) does not match category ({category.CategoryName}).");
                }

                if (_unitOfWork.SessionRepository.HasTrainerOverlap(updateSession.TrainerId, updateSession.StartDate, updateSession.EndDate, id))
                    return Result.Fail("Trainer already has an overlapping session during this time window.");

                _mapper.Map(updateSession, session);
                session.UpdatedAt = DateTime.Now;

                _unitOfWork.SessionRepository.Update(session);
                var saved = _unitOfWork.SaveChanges() > 0;

                return saved ? Result.Ok() : Result.Fail("Failed to update session.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while updating session: {ex.Message}");
            }
        }

        public Result RemoveSession(int sessionId)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (session == null)
                    return Result.Fail("Session not found.");

                if (session.StartDate > DateTime.Now)
                    return Result.Fail("Cannot delete sessions with future dates.");

                _unitOfWork.SessionRepository.Delete(session);
                var saved = _unitOfWork.SaveChanges() > 0;

                return saved ? Result.Ok() : Result.Fail("Failed to delete session.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while deleting session: {ex.Message}");
            }
        }

        public IEnumerable<TrainerSelectViewModel> GetTrainersForDropDown()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetCategoriesForDropDown()
        {
            var categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        #region Helper Methods
        private static bool DoesSpecialtyMatchCategory(Specialties specialty, string categoryName)
        {
            if (string.Equals(specialty.ToString(), categoryName, StringComparison.OrdinalIgnoreCase))
                return true;

            if (specialty == Specialties.GeneralFitness &&
                (categoryName.Equals("Cardio", StringComparison.OrdinalIgnoreCase) ||
                 categoryName.Equals("Strength", StringComparison.OrdinalIgnoreCase)))
                return true;

            if (specialty == Specialties.CrossFit &&
                (categoryName.Equals("Strength", StringComparison.OrdinalIgnoreCase) ||
                 categoryName.Equals("Cardio", StringComparison.OrdinalIgnoreCase)))
                return true;

            return false;
        }
        #endregion
    }
}

