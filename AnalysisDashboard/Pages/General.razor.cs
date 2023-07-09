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

        [Inject]
        ProtectedSessionStorage ProtectedSessionStore { get; set; }

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
        public GeneralBase()
        {
            ClickedMonthStyle = ClickedStyle;
        }

        protected override void OnInitialized()
        {
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
                await SortByMonth(0);
                await jsruntime.InvokeVoidAsync("Chart_1");
                await jsruntime.InvokeVoidAsync("Chart_2");
                StateHasChanged();
            } 
        }

        [JSInvokable]
        public static void ClickChart_1(string barId)
        { 

        }

        [JSInvokable]
        public static void ClickChart_2(string barId)
        {

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
            ClickedMonthStyle = ClickedStyle;
            ClickedWeekStyle = "";

            await SortByMonth(0);
        }

        public async void ClickedWeek()
        {
            forward = 0;
            clickedMonth = false;
            ClickedMonthStyle = "";
            ClickedWeekStyle = ClickedStyle;

            await SortByWeek(0);
        }

        async Task SortByMonth(int index)
        {
            await jsruntime.InvokeVoidAsync("cleanDebitList");
            await jsruntime.InvokeVoidAsync("cleanCreditList");

            /*======================================For Chart 1===========================================================*/
            foreach (string cod in CodeTable.Instance.table.Keys)
            {
                var grouppedCodeList = (from item in dataInfo.Data select (item.Date, item.Debit, item.PurposeOfPayment))
                    .GroupBy(group => group.PurposeOfPayment.Contains(cod)).ToList();

                //var grouppedMonthDebit = (from item in grouppedCodeList[0] select(item.))
                  
                //var grouppedDebitMonthList = (from item in grouppedCodeList select(item.Da)) 
            }

            /*======================================For Chart 3===========================================================*/
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
    }
}
