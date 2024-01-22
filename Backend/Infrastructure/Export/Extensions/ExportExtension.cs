using ClosedXML.Excel;

namespace Infrastructure.Export.Extensions
{
    public static class ExportExtension
    {
        public static IXLWorksheet AutoSize(this IXLWorksheet sheet)
        {
            sheet.Rows().AdjustToContents();
            sheet.Columns().AdjustToContents();            
            return sheet;
        }

        public static IXLWorksheet CreateRows<T>(this IXLWorksheet sheet, T[] rows) where T : class
        {
            for (int i = 0; i < rows.Length; i++)
            {
                var rowFields = rows[i].GetType().GetProperties();
                for (int j = 0; j < rowFields.Length; j++)
                {
                    var value = rowFields[j].GetValue(rows[i]);
                    
                    var row = i + 2;
                    var column = j + 1;
                    var cell = sheet.Cell(row, column);
                    
                    cell.SetValue(value?.ToString());
                    cell.Style.Font.FontName = "Arial";
                    cell.Style.Font.FontSize = 12;
                }
            }
            return sheet;
        }

        public static IXLWorksheet CreateHeader(this IXLWorksheet sheet, string[] headers)
        {
            const int row = 1;
            for (int i = 0; i < headers.Length; i++)
            {
                int column = i + 1;
                var cell = sheet.Cell(row, column);
                
                cell.SetValue(headers[i]);
                cell.Style.Font.FontName = "Arial";
                cell.Style.Font.FontSize = 14;
                cell.Style.Font.Bold = true;                
            }
            return sheet;
        }
    }
}
