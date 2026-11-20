using AutoMapper;
using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MemberViewModels;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.MemberRepository.GetAllWithHealthRecord();
            if (!members.Any()) return Enumerable.Empty<MemberViewModel>();
            return _mapper.Map<IEnumerable<MemberViewModel>>(members);
        }

        public MemberViewModel? GetMemberDetails(int id)
        {
            var member = _unitOfWork.MemberRepository.GetMemberWithDetails(id);
            if (member == null) return null;

            var viewModel = _mapper.Map<MemberViewModel>(member);

            var activeMembership = member.Memberships
                .Where(m => m.EndDate > DateTime.Now)
                .OrderByDescending(m => m.EndDate)
                .FirstOrDefault();

            if (activeMembership != null)
            {
                viewModel.PlanName = activeMembership.Plan?.Name;
                viewModel.MembershipStartDate = activeMembership.StartDate.ToString("MMM dd, yyyy");
                viewModel.MembershipEndDate = activeMembership.EndDate.ToString("MMM dd, yyyy");
            }

            return viewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecord(int id)
        {
            var member = _unitOfWork.MemberRepository.GetWithHealthRecord(id);
            if (member?.HealthRecord == null) return null;
            return _mapper.Map<HealthRecordViewModel>(member.HealthRecord);
        }

        public Result CreateMember(CreateMemberViewModel model)
        {
            try
            {
                if (_unitOfWork.MemberRepository.EmailExists(model.Email))
                    return Result.Fail("Email already exists.");

                if (_unitOfWork.MemberRepository.PhoneExists(model.Phone))
                    return Result.Fail("Phone number already exists.");

                string? photoFileName = null;
                if (model.PhotoFile != null && model.PhotoFile.Length > 0)
                {
                    photoFileName = _attachmentService.Upload(model.PhotoFile, "members");
                    if (string.IsNullOrEmpty(photoFileName))
                        return Result.Fail("Failed to upload profile photo. Please ensure it is a valid image (JPG, PNG, GIF) under 5MB.");
                }

                var member = _mapper.Map<Member>(model);
                member.Photo = photoFileName;
                member.JoinDate = DateTime.Now;
                member.CreatedAt = DateTime.UtcNow;

                if (member.HealthRecord != null)
                {
                    member.HealthRecord.LastUpdate = DateTime.Now;
                    member.HealthRecord.CreatedAt = DateTime.UtcNow;
                }

                _unitOfWork.MemberRepository.Add(member);
                var saved = _unitOfWork.SaveChanges() > 0;

                return saved ? Result.Ok() : Result.Fail("Failed to save member.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while creating member: {ex.Message}");
            }
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int id)
        {
            var member = _unitOfWork.MemberRepository.GetById(id);
            if (member == null) return null;

            var viewModel = _mapper.Map<MemberToUpdateViewModel>(member);
            viewModel.Id = id;
            return viewModel;
        }

        public Result UpdateMemberDetails(int id, MemberToUpdateViewModel model)
        {
            try
            {
                var member = _unitOfWork.MemberRepository.GetById(id);
                if (member == null)
                    return Result.Fail("Member not found.");

                if (_unitOfWork.MemberRepository.EmailExists(model.Email, id))
                    return Result.Fail("Email already in use by another user.");

                if (_unitOfWork.MemberRepository.PhoneExists(model.Phone, id))
                    return Result.Fail("Phone number already in use by another user.");

                _mapper.Map(model, member);
                _unitOfWork.MemberRepository.Update(member);
                var saved = _unitOfWork.SaveChanges() > 0;

                return saved ? Result.Ok() : Result.Fail("Failed to update member.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while updating member: {ex.Message}");
            }
        }

        public Result RemoveMember(int id)
        {
            try
            {
                var member = _unitOfWork.MemberRepository.GetMemberWithDetails(id);
                if (member == null)
                    return Result.Fail("Member not found.");

                if (_unitOfWork.MemberRepository.HasActiveBookings(id))
                    return Result.Fail("Cannot delete member with active bookings.");

                var photo = member.Photo;
                _unitOfWork.MemberRepository.Delete(member);
                var saved = _unitOfWork.SaveChanges() > 0;

                if (saved && !string.IsNullOrEmpty(photo))
                {
                    _attachmentService.Delete(photo, "members");
                }

                return saved ? Result.Ok() : Result.Fail("Failed to delete member.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"An error occurred while removing member: {ex.Message}");
            }
        }
    }
}

