using Contracts.Cases;
using Contracts.Participant;
using Contracts.Staff;
using Contracts.User;
using Domain.Entities;
using Domain.Enumeration;
using System.Diagnostics;
using static Domain.Enumeration.CommonErrors;

namespace Contracts
{
    public static class Mapper
    {
        public static ParticipantDto toParticipant(this RegisterRequest request) => new ParticipantDto()
        {
            User = new UserDto() { email = request.email, password = request.password },
            typeId = request.userType
        };

        public static ParticipantDto appendPersonalData(this ParticipantDto participant, PersonalDataAppendRequest request)
        {
            participant.User.firstName = request.firstName;
            participant.User.lastName = request.lastName;
            participant.User.patronymic = request.patronymic;

            participant.phone = request.phone;
            participant.city = request.city;            
            participant.institution = request.institution;
            participant.grade = request.grade;
            participant.speciality = request.speciality;

            participant.caseId = request.caseId;

            participant.status = Roles.ParticipantsStatus.sentPersonalData;

            return participant;
        }

        public static PersonalDataAppendResponse toPersonalDataResponse(this ParticipantDto participant) => new PersonalDataAppendResponse()
        {
            userId = participant.userId,
            status = participant.status,
            email = participant.User.email,
            firstName = participant.User.firstName,
            youtubeId = participant.Case.youtubeId
        };

        public static ParticipantDto toParticipant(this CreateParticipantRequest request) => new ParticipantDto()
        {
            User = new UserDto() { email = request.email, password = request.password, firstName = request.firstName, lastName = request.lastName, patronymic = request.patronymic },
            parentName = request.parentName,
            typeId = request.typeId,
            caseId = request.caseId,
            status = request.solution is null || request.consent is null ? Roles.ParticipantsStatus.sentPersonalData : Roles.ParticipantsStatus.awaitingResults,
            phone = request.phone,
            city = request.city,
            institution = request.institution,
            grade = request.grade,
            speciality = request.speciality,
            consentFilename = request.consentFilename,
            solutionFilename = request.solutionFilename,
        };

        public static ParticipantDto updateParticipant(this ParticipantDto participant, UpdateParticipantRequest request)
        {
            if (request.userTypeId is not null)
                participant.typeId = (int)request.userTypeId;
            if (request.caseId is not null)
                participant.caseId = (int)request.caseId;

            participant.parentName = request.parentName;
            participant.User.email = request.email;
            participant.phone = request.phone;            
            participant.User.firstName = request.firstName;
            participant.User.lastName = request.lastName;
            participant.User.patronymic = request.patronymic;
            participant.city = request.city;
            participant.institution = request.institution;
            participant.grade = request.grade;
            participant.speciality = request.speciality;
            participant.rating = request.rating;
            participant.status = request.status;

            return participant;
        }

        public static IEnumerable<ParticipantExportModel> toParticipantExportModel(this ParticipantDto[] participants)
        {
            foreach (var participant in participants)
            {
                yield return new ParticipantExportModel()
                {
                    firstName = participant.User.firstName,
                    lastName = participant.User.lastName,
                };
            }
        }

        public static GetParticipantResponse toParticipantResponse(this ParticipantDto participant) => new GetParticipantResponse()
        {
            parentName = participant.parentName,
            email = participant.User.email,
            firstName = participant.User.firstName,
            lastName = participant.User.lastName,
            patronymic = participant.User.patronymic,
            typeId = participant.typeId,
            caseId = participant.caseId,
            status = participant.status,
            phone = participant.phone,
            city = participant.city,
            institution = participant.institution,
            grade = participant.grade,
            speciality = participant.speciality,
            consentFilename = participant.consentFilename,
            solutionFilename = participant.solutionFilename,
            rating = participant.rating,
        };

        public static GetStaffMemberResponse toStaffResponse(this StaffDto staff) => new GetStaffMemberResponse()
        {
            email = staff.User.email,
            firstName = staff.User.firstName,
            lastName = staff.User.lastName,
            patronymic = staff.User.patronymic,
            roleId = staff.roleId
        };

        public static StaffDto toStaff(this CreateStaffRequest request) => new StaffDto()
        {
            roleId = request.roleId,
            User = new UserDto()
            {
                email = request.email,
                password = request.password,
                firstName = request.firstName,
                lastName = request.lastName,
                patronymic = request.patronymic,
            }
        };

        public static StaffDto updateStaff(this StaffDto staff, UpdateStaffRequest request)
        {
            if (request.roleId is not null)
                staff.roleId = (int)request.roleId;

            staff.User.email = request.email;
            staff.User.firstName = request.firstName;
            staff.User.lastName = request.lastName;
            staff.User.patronymic = request.patronymic;

            return staff;
        }

        public static CaseDto toCase(this UpsertCaseRequest request) => new CaseDto()
        {
            name = request.name,
            task = request.task,
            youtubeId = request.youtubeId,
            stages = request.stage,
            criterias = request.criteria
        };

        public static CaseDto updateCase(this CaseDto _case, UpsertCaseRequest request)
        {
            _case.name = request.name;
            _case.task = request.task;
            _case.youtubeId = request.youtubeId;
            _case.stages = request.stage;
            _case.criterias = request.criteria;
            return _case;
        }
    }
}
