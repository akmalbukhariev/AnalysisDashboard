using AnalysisDashboard.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Linq;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using AnalysisDashboard.Helper;

namespace AnalysisDashboard.Pages
{
    public class GeneralBase : IPage
    {
        [Inject]
        IJSRuntime jsruntime { get; set; }

        //[Inject]
        //ProtectedSessionStorage ProtectedSessionStore { get; set; }

        [Inject]
        public DataInfo dataInfo { get; set; } 

        [Parameter]
        public string BalanceAtTheBeginningOfThePeriod { get; set; }

        [Parameter]
        public string FxpenseForThePeriod { get; set; }

        [Parameter]
        public string FceiptForThePeriod { get; set; }

        [Parameter]
        public string BalanceAtTheEndOfThePeriod { get; set; }

        [Parameter]
        public string ClickedMonthStyle { get; set; } = "";

        [Parameter]
        public string ClickedWeekStyle { get; set; } = "";

        [Parameter]
        public bool ShowTable { get; set; } = false;

        string ClickedStyle = "text-decoration: underline solid #0077b6 2px;";
        
        [Parameter]
        public List<BarChartItemInfo> BarChartItemInfoList { get; set; }
         
        public GeneralBase()
        { 
            BarChartItemInfoList = new List<BarChartItemInfo>();
            ClickedMonthStyle = ClickedStyle + "color: blue;";
            ClickedWeekStyle = "color: gray;";
        }

        protected override async void OnInitialized()
        {
            var dotNetReference = DotNetObjectReference.Create(this);
            await jsruntime.InvokeVoidAsync("setObjReference", dotNetReference);

            if (dataInfo != null && dataInfo.Header != null)
            {
                BalanceAtTheBeginningOfThePeriod = dataInfo.Header.BalanceAtTheBeginningOfPeriod.ToString("#,##0.########");
                FxpenseForThePeriod = dataInfo.Header.FxpenseForThePeriod.ToString("#,##0.########");
                FceiptForThePeriod = dataInfo.Header.FceiptForThePeriod.ToString("#,##0.########");
                BalanceAtTheEndOfThePeriod = dataInfo.Header.BalanceAtTheEndOfThePeriod.ToString("#,##0.########"); 
            }
        }
         
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            { 
                foreach (string code in CodeTable.Instance.table.Keys)
                {
                    var grouppedSearchCodeList = dataInfo.Data.GroupBy(item => item.Code.Equals(code)).ToList();
                    foreach (var group in grouppedSearchCodeList)
                    {
                        if (group.Key)
                        {
                            foreach (var item in group)
                            {
                                item.NameOfCode = CodeTable.Instance.GetData(code).Trim();
                            }
                        }
                    }
                }

                await DebitBar();
                await CreditBar();
                await SortByMonth(0); 
            } 
        }

        public void CloseTableInfo()
        {
            ShowTable = false;
        }

        async Task DebitBar()
        { 
            var grouppedCodeList = dataInfo.Data.GroupBy(item => item.Code).ToList();
            List<(string, double)> sumDebitList = new List<(string, double)>();

            double sumDebit = 0;
            foreach (var group in grouppedCodeList)
            {
                sumDebitList.Add((group.Key, group.Sum(item => item.Debit)));
            }

            foreach (var sumItem in sumDebitList)
            {
                BarChartInfo newItem = new BarChartInfo();
                newItem.Code = sumItem.Item1;
                newItem.Amount = sumItem.Item2;
                newItem.Name = CodeTable.Instance.GetData(sumItem.Item1).Trim();
                newItem.Color = getRandColor();

                await jsruntime.InvokeVoidAsync("addDebitBar", newItem);
            }

            sumDebit = sumDebitList.Sum(item => item.Item2);
            string strSum = double.Parse(String.Format("{0:0.00}", sumDebit)).ToString("#,##0.########");
            await jsruntime.InvokeVoidAsync("Chart_1", strSum + " Сум");
        }

        async Task CreditBar()
        {
            var grouppedCodeList = dataInfo.Data.GroupBy(item => item.Code).ToList();
            List<(string, double)> sumCreditList = new List<(string, double)>();

            double sumCredit = 0;
            foreach (var group in grouppedCodeList)
            {
                sumCreditList.Add((group.Key, group.Sum(item => item.Credit)));
            }

            foreach (var sumItem in sumCreditList)
            {
                BarChartInfo newItem = new BarChartInfo();
                newItem.Code = sumItem.Item1;
                newItem.Amount = sumItem.Item2;
                newItem.Name = CodeTable.Instance.GetData(sumItem.Item1).Trim();
                newItem.Color = getRandColor();

                await jsruntime.InvokeVoidAsync("addBarCredit", newItem);
            }

            sumCredit = sumCreditList.Sum(item => item.Item2);
            string strSum = double.Parse(String.Format("{0:0.00}", sumCredit)).ToString("#,##0.########");
            await jsruntime.InvokeVoidAsync("Chart_2", strSum + " Сум");
        }

        [JSInvokable]
        public void ClickBarChart1(string barId)
        { 
            BarChartItemInfoList.Clear();
            var grouppedCodeList = dataInfo.Data.GroupBy(item => item.Code).ToList().Where(group => group.Key.Equals(barId)).ToList();

            if (grouppedCodeList.Count > 0)
            {
                foreach (var group in grouppedCodeList[0])
                {
                    BarChartItemInfo newItem = new BarChartItemInfo();
                    newItem.Date = group.Date.ToString("dd.MM.yyyy");
                    newItem.Sum = group.Debit.ToString("#,##0.########");
                    newItem.PurposeOfPayment = group.PurposeOfPayment;

                    BarChartItemInfoList.Add(newItem);
                }
            }

            ShowTable = true;
            StateHasChanged(); 
        }

        [JSInvokable]
        public void ClickBarChart2(string barId)
        {
            BarChartItemInfoList.Clear();
            var grouppedCodeList = dataInfo.Data.GroupBy(item => item.Code).ToList().Where(group => group.Key.Equals(barId)).ToList();

            if (grouppedCodeList.Count > 0)
            {
                foreach (var group in grouppedCodeList[0])
                {
                    BarChartItemInfo newItem = new BarChartItemInfo();
                    newItem.Date = group.Date.ToString("dd.MM.yyyy");
                    newItem.Sum = group.Credit.ToString("#,##0.########");
                    newItem.PurposeOfPayment = group.PurposeOfPayment;

                    BarChartItemInfoList.Add(newItem);
                }
            }

            ShowTable = true;
            StateHasChanged();
        }

        int forward = 0;
        public async void ClickForward()
        { 
            forward++;
            if (clickedMonth)
            {
                await SortByMonth(forward);
            }
            else
            {
                await SortByWeek(forward);
            }
        }

        public async void ClickBack()
        {
            forward--;
            if(forward < 0)
                forward = 0;

            if (clickedMonth)
            {
                await SortByMonth(forward);
            }
            else
            {
                await SortByWeek(forward);
            }
        }

        bool clickedMonth = true;
        public async void ClickedMonth()
        {
            forward = 0;
            clickedMonth = true;
            ClickedMonthStyle = ClickedStyle + "color: blue;";
            ClickedWeekStyle = "color: gray;";

            await SortByMonth(0);
        }

        public async void ClickedWeek()
        {
            forward = 0;
            clickedMonth = false;
            ClickedWeekStyle = ClickedStyle + "color: blue;";
            ClickedMonthStyle = "color: gray;";

            await SortByWeek(0);
        }

        async Task SortByMonth(int index)
        {
            await jsruntime.InvokeVoidAsync("cleanDebitList");
            await jsruntime.InvokeVoidAsync("cleanCreditList");
              
            var debitList3 = (from item in dataInfo.Data select (item.Date, item.Debit)).GroupBy(item => item.Date.Month).ToList();
            var creditList3 = (from item in dataInfo.Data select (item.Date, item.Credit)).GroupBy(item => item.Date.Month).ToList();

            if (debitList3.Count <= index)
            {
                forward--;
                return;
            }
            if (creditList3.Count <= index)
            {
                forward--;
                return;
            }

            var groupedDebit3 = debitList3[index].GroupBy(item => item.Date.Day).ToList();
            var debitSums3 = groupedDebit3.Select(group => group.Sum(item => item.Debit)).ToList();

            for (int i = 0; i < debitSums3.Count; i++)
            {
                var tList = groupedDebit3[i].ToList();
                if (tList.Count > 0)
                {
                    DynamicChartInfo newItem = new DynamicChartInfo();
                    newItem.Year = tList[0].Date.Year;
                    newItem.Month = tList[0].Date.Month;
                    newItem.Day = tList[0].Date.Day;
                    newItem.Amount = Convert.ToDouble(debitSums3[i]);

                    await jsruntime.InvokeVoidAsync("addDebit", newItem);
                }
            }

            var groupedCredit3 = creditList3[index].GroupBy(item => item.Date.Day).ToList();
            var creditSums3 = groupedCredit3.Select(group => group.Sum(item => item.Credit)).ToList();

            for (int i = 0; i < creditSums3.Count; i++)
            {
                var tList = groupedCredit3[i].ToList();
                if (tList.Count > 0)
                {
                    DynamicChartInfo newItem = new DynamicChartInfo();
                    newItem.Year = tList[0].Date.Year;
                    newItem.Month = tList[0].Date.Month;
                    newItem.Day = tList[0].Date.Day;
                    newItem.Amount = Convert.ToDouble(creditSums3[i]);

                    await jsruntime.InvokeVoidAsync("addCredit", newItem);
                }
            }
             
            await jsruntime.InvokeVoidAsync("Chart_3");
        }

        async Task SortByWeek(int index)
        {
            await jsruntime.InvokeVoidAsync("cleanDebitList");
            await jsruntime.InvokeVoidAsync("cleanCreditList");

            var debitList = (from item in dataInfo.Data select (item.Date, item.Debit)).GroupBy(item => item.Date.StartOfWeek(DayOfWeek.Monday)).ToList();
            var creditList = (from item in dataInfo.Data select (item.Date, item.Credit)).GroupBy(item => item.Date.StartOfWeek(DayOfWeek.Monday)).ToList();

            if (debitList.Count <= index) return;
            if (creditList.Count <= index) return;

            var groupedDebit = debitList[index].GroupBy(item => item.Date.Day).ToList();
            var debitSums = groupedDebit.Select(group => group.Sum(item => item.Debit)).ToList();

            for (int i = 0; i < debitSums.Count; i++)
            {
                var tList = groupedDebit[i].ToList();
                if (tList.Count > 0)
                {
                    DynamicChartInfo newItem = new DynamicChartInfo();
                    newItem.Year = tList[0].Date.Year;
                    newItem.Month = tList[0].Date.Month;
                    newItem.Day = tList[0].Date.Day;
                    newItem.Amount = Convert.ToDouble(debitSums[i]);

                    await jsruntime.InvokeVoidAsync("addDebit", newItem);
                }
            }

            var groupedCredit = creditList[index].GroupBy(item => item.Date.Day).ToList();
            var creditSums = groupedCredit.Select(group => group.Sum(item => item.Credit)).ToList();

            for (int i = 0; i < creditSums.Count; i++)
            {
                var tList = groupedCredit[i].ToList();
                if (tList.Count > 0)
                {
                    DynamicChartInfo newItem = new DynamicChartInfo();
                    newItem.Year = tList[0].Date.Year;
                    newItem.Month = tList[0].Date.Month;
                    newItem.Day = tList[0].Date.Day;
                    newItem.Amount = Convert.ToDouble(creditSums[i]);

                    await jsruntime.InvokeVoidAsync("addCredit", newItem);
                }
            }

            await jsruntime.InvokeVoidAsync("Chart_3");
        }

        private string getRandColor()
        {
            Random rnd = new Random();
            string hexOutput = String.Format("{0:X}", rnd.Next(0, 0xFFFFFF));
            while (hexOutput.Length < 6)
                hexOutput = "0" + hexOutput;
            return "#" + hexOutput;
        }
    }
}
