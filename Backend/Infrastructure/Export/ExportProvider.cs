using ClosedXML.Excel;
using Contracts.File;
using Contracts.Participant;
using Contracts.Staff;
using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Export.Extensions;
using System.Runtime.CompilerServices;

namespace Infrastructure.Export
{
    internal class ExportProvider : IExportProvider
    {
        private const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";




        public FileResponse ExportParticipants(ParticipantExportModel[] participants)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.AddWorksheet();
            sheet.CreateHeader(["Имя", "Фамилия", "Отчество", "Тип", "Направление", "Балл", "Город", "Номер телефона", "Email", "Учебное заведение", "Специальность", "Год обучения"]);
            sheet.CreateRows(participants);
            sheet.AutoSize();

            MemoryStream ms = new();
            workbook.SaveAs(ms);
            ms.Position = 0;

            FileResponse response = new()
            {
                contentType = ContentType,
                fileStream = ms
            };
            return response;
        }

        public FileResponse ExportStaff(StaffExportModel[] staff)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.AddWorksheet();
            sheet.CreateHeader(["Имя", "Фамилия", "Отчество", "Роль", "Email"]);
            sheet.CreateRows(staff);
            sheet.AutoSize();

            MemoryStream ms = new();
            workbook.SaveAs(ms);
            ms.Position = 0;

            FileResponse response = new()
            {
                contentType = ContentType,
                fileStream = ms
            };
            return response;
        }
    }
}
