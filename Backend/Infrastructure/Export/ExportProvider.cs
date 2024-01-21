using ClosedXML.Excel;
using Contracts.File;
using Contracts.Participant;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Runtime.CompilerServices;

namespace Infrastructure.Export
{
    internal class ExportProvider : IExportProvider
    {
        private const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        private IXLWorksheet CreateHeader(string[] headers, IXLWorksheet sheet)
        {
            for(int i=0; i<headers.Length; i++)
            {
                sheet.Cell(1, i + 1).SetValue(headers[i]);
            }
            return sheet;
        }
        private IXLWorksheet CreateRows<T>(T[] rows, IXLWorksheet sheet) where T : class
        {
            for (int i = 0; i < rows.Length; i++)
            {
                var rowFields = rows[i].GetType().GetProperties();
                for (int j=0; j < rowFields.Length; j++)
                {
                    var value = rowFields[j].GetValue(rows[i]);
                    sheet.Cell(i+2, j+1).SetValue(value?.ToString());
                }
            }
            return sheet;
        }

        public FileResponse ExportParticipants(ParticipantExportModel[] participants)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.AddWorksheet();
            sheet = CreateHeader(["Имя", "Фамилия", "Отчество", "Тип", "Направление", "Балл", "Город", "Номер телефона", "Email", "Учебное заведение", "Специальность", "Год обучения"], sheet);
            sheet = CreateRows<ParticipantExportModel>(participants, sheet);

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
