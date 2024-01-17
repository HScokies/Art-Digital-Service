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

    }
}
