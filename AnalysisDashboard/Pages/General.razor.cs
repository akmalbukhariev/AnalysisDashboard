using AnalysisDashboard.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop; 
using AnalysisDashboard.Helper; 
using System.Globalization;

namespace AnalysisDashboard.Pages
{
    public class GeneralBase : BasePage
    {  
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
         
        string ClickedStyle = "text-decoration: underline solid #0077b6 1px;  a.blue,a.blue:visited {color: #0096C7;} "; 
          
        [Parameter]
        public List<BarChartItemInfo> BarChartItemInfoList { get; set; }
         
        public GeneralBase()
        { 
            BarChartItemInfoList = new List<BarChartItemInfo>();
            ClickedMonthStyle = ClickedStyle + "color: blue;";
            ClickedWeekStyle = "color: gray;";
        }

        protected override void OnInitialized()
        {
            if (dataInfo != null && dataInfo.Header != null)
            {
                string currency = " " + Localizer["Currency"];
                BalanceAtTheBeginningOfThePeriod = dataInfo.Header.BalanceAtTheBeginningOfPeriod.ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ');//ToString("N2", CultureInfo.InvariantCulture) + currency;
                FxpenseForThePeriod = dataInfo.Header.FxpenseForThePeriod.ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ') + currency;
                FceiptForThePeriod = dataInfo.Header.FceiptForThePeriod.ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ') + currency;
                BalanceAtTheEndOfThePeriod = dataInfo.Header.BalanceAtTheEndOfThePeriod.ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ') + currency;
            } 
        }
         
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (dataInfo.Header == null)
            {
                Navigation.NavigateTo("/", true); 
            }
            else
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

                    ShowLoading = true;
                    StateHasChanged();
                     
                    await DebitBar();
                    await CreditBar();
                    await SortByMonth(0);

                    ShowLoading = false;
                    StateHasChanged();

                    //var th1 = new Thread(async () => { await DebitBar(); });
                    //var th2 = new Thread(async () => { await CreditBar(); });
                    //var th3 = new Thread(async () => { await SortByMonth(0); });

                    //th1.Start();
                    //th2.Start();
                    //th3.Start();
                }
                 
                var dotNetReference = DotNetObjectReference.Create(this);
                await jsRuntime.InvokeVoidAsync("setObjReference", dotNetReference);
            }
        }

        [JSInvokable]
        public void RefreshChartPage()
        {
            //await jsRuntime.InvokeVoidAsync("history.back");

            Navigation.NavigateTo("/");
            //string message = Localizer["Message1"];
            //await jsRuntime.InvokeVoidAsync("alert", message);
        }

        public void CloseTableInfo()
        {
            ShowTable = false;
        }

        async Task DebitBar()
        {  
            await jsRuntime.InvokeVoidAsync("cleanDebitBarList");

            var grouppedCodeList = dataInfo.Data.GroupBy(item => item.Code).ToList();
            List<(string, double)> sumDebitList = new List<(string, double)>();

            double sumDebit = 0;
            foreach (var group in grouppedCodeList)
            {
                sumDebitList.Add((group.Key, group.Sum(item => item.Debit)));
            }

            sumDebitList = sumDebitList.OrderBy(item => item.Item2).ToList(); 
            int i = 0; 
            foreach (var sumItem in sumDebitList)
            {
                if (sumItem.Item2 == 0.0) continue;

                BarChartInfo newItem = new BarChartInfo();
                newItem.Code = sumItem.Item1;
                newItem.Amount = sumItem.Item2;
                newItem.Name = CodeTable.Instance.GetData(sumItem.Item1).Trim();
                if (i < ColorTable.ColorDebitList.Count)
                    newItem.Color = ColorTable.ColorDebitList[i];

                await jsRuntime.InvokeVoidAsync("addDebitBar", newItem);
                i++;
            }

            string currency = " " + Localizer["Currency"];
            sumDebit = sumDebitList.Sum(item => item.Item2);
            string strSum = double.Parse(String.Format("{0:0.00}", sumDebit)).ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ');
            await jsRuntime.InvokeVoidAsync("Chart_1", strSum + currency); 
        }

        async Task CreditBar()
        { 
            await jsRuntime.InvokeVoidAsync("cleanCreditBarList");

            var grouppedCodeList = dataInfo.Data.GroupBy(item => item.Code).ToList();
            List<(string, double)> sumCreditList = new List<(string, double)>();

            double sumCredit = 0;
            foreach (var group in grouppedCodeList)
            {
                sumCreditList.Add((group.Key, group.Sum(item => item.Credit)));
            }

            sumCreditList = sumCreditList.OrderBy(item => item.Item2).ToList(); 
            int i = 0; 
            foreach (var sumItem in sumCreditList)
            {
                if (sumItem.Item2 == 0.0) continue;

                BarChartInfo newItem = new BarChartInfo();
                newItem.Code = sumItem.Item1;
                newItem.Amount = sumItem.Item2;
                newItem.Name = CodeTable.Instance.GetData(sumItem.Item1).Trim();
                if (i < ColorTable.ColorCreditList.Count)
                    newItem.Color = ColorTable.ColorCreditList[i];  

                await jsRuntime.InvokeVoidAsync("addBarCredit", newItem);
                i++;
            }

            string currency = " " + Localizer["Currency"];
            sumCredit = sumCreditList.Sum(item => item.Item2);
            string strSum = double.Parse(String.Format("{0:0.00}", sumCredit)).ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ');
            await jsRuntime.InvokeVoidAsync("Chart_2", strSum + currency); 
        }

        [JSInvokable]
        public void ClickBarChart1(string barId)
        {
            ShowLoading = true;
            StateHasChanged();

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

                    if (newItem.Sum != "0")
                        BarChartItemInfoList.Add(newItem);
                }
            }

            ShowLoading = false;  
            ShowTable = true;
            StateHasChanged(); 
        }

        [JSInvokable]
        public void ClickBarChart2(string barId)
        {
            ShowLoading = true;
            StateHasChanged();

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

                    if (newItem.Sum != "0")
                        BarChartItemInfoList.Add(newItem);
                }
            }

            ShowLoading = false;
            ShowTable = true;
            StateHasChanged();
        }

        int forward = 0;
        public async Task ClickForward()
        {
            ShowLoading = true;
            StateHasChanged();

            forward++;
            if (clickedMonth)
            {
                await SortByMonth(forward);
            }
            else
            {
                await SortByWeek(forward);
            }

            ShowLoading = false;
            StateHasChanged();
        }

        public async Task ClickBack()
        {
            ShowLoading = true;
            StateHasChanged();

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

            ShowLoading = false;
            StateHasChanged();
        }

        bool clickedMonth = true;
        public async Task ClickedMonth()
        {
            forward = 0;
            clickedMonth = true;
            ClickedMonthStyle = ClickedStyle + "color: #0077b6;";
            ClickedWeekStyle = "color: #343A40;";

            ShowLoading = true;
            StateHasChanged();

            await SortByMonth(0);

            ShowLoading = false;
            StateHasChanged();
        }

        public async Task ClickedWeek()
        {
            forward = 0;
            clickedMonth = false;
            ClickedWeekStyle = ClickedStyle + "color: #0077b6;";
            ClickedMonthStyle = "color: #343A40;";

            ShowLoading = true;
            StateHasChanged();

            await SortByWeek(0);

            ShowLoading = false;
            StateHasChanged();
        }

        async Task SortByMonth(int index)
        { 
            var debitList = (from item in dataInfo.Data select (item.Date, item.Debit)).GroupBy(item => item.Date.Month).ToList();
            var creditList = (from item in dataInfo.Data select (item.Date, item.Credit)).GroupBy(item => item.Date.Month).ToList();

            if (debitList.Count <= index)
            {
                forward--;
                return;
            }
            if (creditList.Count <= index)
            {
                forward--;
                return;
            }

            await jsRuntime.InvokeVoidAsync("cleanDebitList");
            await jsRuntime.InvokeVoidAsync("cleanCreditList");

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
                    newItem.Amount = debitSums[i];

                    await jsRuntime.InvokeVoidAsync("addDebit", newItem);
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
                    newItem.Amount = creditSums[i];

                    await jsRuntime.InvokeVoidAsync("addCredit", newItem);
                }
            }
             
            await jsRuntime.InvokeVoidAsync("Chart_3"); 
        }

        async Task SortByWeek(int index)
        {  
            var debitList = (from item in dataInfo.Data select (item.Date, item.Debit)).GroupBy(item => item.Date.StartOfWeek(DayOfWeek.Monday)).ToList();
            var creditList = (from item in dataInfo.Data select (item.Date, item.Credit)).GroupBy(item => item.Date.StartOfWeek(DayOfWeek.Monday)).ToList();

            if (debitList.Count <= index)
            {
                forward--;
                return;
            }
            if (creditList.Count <= index)
            {
                forward--;
                return;
            }

            await jsRuntime.InvokeVoidAsync("cleanDebitList");
            await jsRuntime.InvokeVoidAsync("cleanCreditList");

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
                    newItem.Amount = debitSums[i];

                    await jsRuntime.InvokeVoidAsync("addDebit", newItem);
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
                    newItem.Amount = creditSums[i];

                    await jsRuntime.InvokeVoidAsync("addCredit", newItem);
                }
            }

            await jsRuntime.InvokeVoidAsync("Chart_3"); 
        } 
    }
}
