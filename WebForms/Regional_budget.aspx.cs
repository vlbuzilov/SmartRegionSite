using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Drawing.Imaging;
using System.Drawing;

namespace Site1.WebForms
{
    public partial class Regional_budget : System.Web.UI.Page
    {
        /// <summary>
        /// On load page take image from cshtml page and take data from Excel and display on Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// Load data from Excel file
                string filePath = Server.MapPath("/WebForms/Budget.xlsx");
                DataTable dt = LoadExcelFile(filePath);

                /// Bind data to GridView
                GridView1.DataSource = dt;
                GridView1.DataBind();

               
            }
        }

        /// <summary>
        /// Load data from Excel
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private DataTable LoadExcelFile(string filePath)
        {
            DataTable dt = new DataTable();

            // Open Excel file and read data
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart workbookPart = doc.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.FirstOrDefault();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();

                // Read header row
                Row headerRow = sheetData.Elements<Row>().First();
                foreach (Cell cell in headerRow.Elements<Cell>())
                {
                    dt.Columns.Add(GetCellValue(doc, cell));
                }

                // Read data rows
                foreach (Row row in sheetData.Elements<Row>().Skip(1))
                {
                    DataRow dataRow = dt.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        Cell cell = row.Elements<Cell>().ElementAt(i);
                        dataRow[i] = GetCellValue(doc, cell);
                    }
                    dt.Rows.Add(dataRow);
                }
            }

            return dt;
        }

        /// <summary>
        /// Helper method which take value from cell
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        private string GetCellValue(SpreadsheetDocument doc, Cell cell)
        {
            SharedStringTablePart stringTablePart = doc.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[int.Parse(value)].InnerText;
            }
            return value;
        }
    }
}