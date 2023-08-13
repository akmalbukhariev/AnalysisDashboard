using AnalysisDashboard.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Globalization;
using System.Linq;

namespace AnalysisDashboard.Pages
{
    public class IndexBase : BasePage
    {   
        public IndexBase()
        { 
            dataInfo = new DataInfo();
        }
         
        public async Task FileUploadOnChange(InputFileChangeEventArgs e)
        { 
            try
            {
                if (dataInfo != null)
                {
                    dataInfo.Data?.Clear();
                }
                ShowLoading = true;
                var file = e.File;
                if (file != null)
                {
                    using var ms = new MemoryStream();
                    await file.OpenReadStream().CopyToAsync(ms);
                    ms.Position = 0;

                    ISheet sheet;
                    var xsswb = new XSSFWorkbook(ms);

                    sheet = xsswb.GetSheetAt(0);
                    IRow headerRow = sheet.GetRow(0);
                    int colCount = headerRow.LastCellNum;

                    dataInfo.Header = GetDataHeader(sheet, colCount);

                    headerRow = sheet.GetRow(4);
                    colCount = headerRow.LastCellNum;

                    for (var j = 5; j <= sheet.LastRowNum; j++)
                    {
                        var row = sheet.GetRow(j);
                        DataItem item = GetRowItem(row, colCount);

                        if (item.Account_Tin.Trim() != "")
                            dataInfo.Data.Add(item);
                    }
                }

                dataInfo.Header.FxpenseForThePeriod = (from item in dataInfo.Data select item.Debit).Sum();
                dataInfo.Header.FceiptForThePeriod = (from item in dataInfo.Data select item.Credit).Sum();

                ShowLoading = false;

                Navigation.NavigateTo("/general");
            }
            catch (Exception ex)
            {
                ShowLoading = false;
                string message = Localizer["Message2"];
                await jsRuntime.InvokeVoidAsync("alert", message);
            }
        }

        private DataHeader GetDataHeader(ISheet sheet, int colCount)
        {
            DataHeader header = new DataHeader();

            var row0 = sheet.GetRow(0);
            var row1 = sheet.GetRow(1);
            var row2 = sheet.GetRow(2);
            var row3 = sheet.GetRow(3);

            List<string> tList = new List<string>();
            ICell cell0_0 = row0.GetCell(0);
            ICell cell0_1 = row0.GetCell(1);
            tList = cell0_0.ToString().Split("/").ToList();
            if (tList.Count == 2)
            {
                header.Mfo = tList[0];
                header.BranchOfTheBank = tList[1];
            }
            header.Date = cell0_1.ToString();

            cell0_0 = row1.GetCell(0);
            header.InformationAboutTheOperationOfTheAccount = cell0_0.ToString();
 
            tList.Clear();
            cell0_0 = row2.GetCell(0);
            
            string str = cell0_0.ToString();
            int startIndex = str.IndexOf("\"") + 1;
            int endIndex = str.LastIndexOf("\"");

            if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
            {
                header.NameOfFirm = str.Substring(startIndex, endIndex - startIndex);
            }

            str = str.Replace(header.NameOfFirm, "");
            str = str.Replace("\"", "");
            tList = str.Split(" ").ToList();
            tList.RemoveAll(item => item == "");

            for (int i = 0; i < tList.Count; i++)
            {
                if (tList[i].Equals("Счет:"))
                {
                    if (tList.Count > i + 1 && header.Account == "")
                    {
                        header.Account = tList[i + 1];
                    }
                }

                if (tList[i].Equals("ИНН:"))
                {
                    if (tList.Count > i + 1 && header.Tin == "")
                    {
                        header.Tin = tList[i + 1];
                    }
                }
            }
             
            cell0_0 = row3.GetCell(0);
            cell0_1 = row3.GetCell(1);
            header.BalanceAtTheBeginningOfPeriod = GetBalance(cell0_0.ToString());
            header.BalanceAtTheEndOfThePeriod = GetBalance(cell0_1.ToString());
             
            return header;
        }

        private DataItem GetRowItem(IRow row, int colCount)
        {
            DataItem item = new DataItem();

            for (var i = row.FirstCellNum; i < colCount; i++)
            { 
                switch (i)
                {
                    case 0:
                        {
                            string format = "dd.MM.yyyy HH:mm:ss";
                            DateTime dateTime;
                            if (DateTime.TryParseExact(row.GetCell(i).ToString(), format, null, DateTimeStyles.None, out dateTime))
                                item.Date = dateTime;
                        }
                        break;
                    case 1: item.Account_Tin = row.GetCell(i).ToString(); break;
                    case 2: item.NoDokta = row.GetCell(i).ToString(); break;
                    case 3: item.Op = row.GetCell(i).ToString(); break;
                    case 4: item.Mfo = row.GetCell(i).ToString(); break;
                    case 5:
                        {
                            string strDebit = row.GetCell(i).ToString().Trim();
                            item.Debit = double.Parse(strDebit); 
                        }
                        break;
                    case 6:
                        {
                            string strCredit = row.GetCell(i).ToString().Trim();
                            item.Credit = double.Parse(strCredit); 
                        }
                        break;
                    case 7:
                        {
                            item.PurposeOfPayment = row.GetCell(i).ToString();
                            if (item.PurposeOfPayment.Length >= 6)
                            {
                                item.Code = item.PurposeOfPayment.Substring(0, 5);
                            }
                        }
                        break;
                }
            }

            return item;
        }

        double GetBalance(string str)
        {
            int startIndex = 0;
            int endIndex = str.LastIndexOf(":");

            string tempStr = str.Substring(startIndex, endIndex - startIndex) + ":";
            str = str.Replace(tempStr, "").RemoveWhitespace().Trim();

            return double.Parse(str, CultureInfo.InvariantCulture);
        }
    }
}
