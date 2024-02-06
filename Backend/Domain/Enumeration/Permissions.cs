namespace Domain.Enumeration
{
    public static class Roles
    {
        public static class Permissions
        {
            public const string createUsers = "7lzR";
            public const string readUsers = "Nac2";
            public const string updateUsers = "BrJG";
            public const string rateUsers = "D9Bs"; // aka soft update
            public const string deleteUsers = "CKb6";

            public const string createStaff = "8DTO";
            public const string readStaff = "pMtW";
            public const string updateStaff = "tZmn";
            public const string deleteStaff = "nsr8";

            public const string createCases = "OWR9";
            public const string readCases = "nqIz";
            public const string updateCases = "MTBp";
            public const string deleteCases = "BSeT";

            public const string utilsAccess = "qUX1";
        }

        public static class ParticipantsStatus 
        {
            public const string justRegistered = "OWvU"; //can send personal data
            public const string sentPersonalData = "gtiZ"; //can send consent & solution
            public const string awaitingResults = "ryAe"; // awaiting next status
            public const string invited = "QCTw"; // was invited to 2nd stage
            public const string droppedOut = "5Occ"; // was not
            
        }
    }
}
