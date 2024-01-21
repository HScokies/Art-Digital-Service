using Contracts.User;
using Domain.Entities;
using Domain.Enumeration;
using System.Diagnostics;

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

            participant.User.email = request.email;
            participant.phone = request.phone;            
            participant.User.firstName = request.firstName;
            participant.User.lastName = request.lastName;
            participant.User.patronymic = request.patronymic;
            participant.city = request.city;
            participant.institution = request.institution;
            participant.grade = request.grade;
            participant.speciality = request.speciality;
            participant.rating = request.score;
            participant.status = request.status;

            return participant;
        }
    }
}
