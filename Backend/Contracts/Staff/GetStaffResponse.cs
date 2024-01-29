namespace Contracts.Staff
{
    public sealed class GetStaffResponse
    {
        public StaffPreview[] rows { get; set; } = null!;
        public int currentPage { get; set; }
        public int pagesTotal { get; set; }
    }

    public sealed class StaffPreview
    {
        public int id { get; set; }
        public string fullName { get; set; } = null!;
        public string roleName { get; set; } = null!;
    }
}
