namespace Domain.Enumeration
{
    public enum access_levels
    {
        readUsers,
        createUsers,
        rateUsers, //aka limited update
        updateUsers,
        deleteUsers,

        readCases,
        createCases,
        updateCases,
        deleteCases,

        readStaff,
        createStaff,
        updateStaff,
        deleteStaff
    }
}
