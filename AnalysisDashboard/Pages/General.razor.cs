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
        /*        
         a.blue,a.blue:visited {
          color: #0096C7;
        } 
        a.blue:hover,a.blue:active {
          color: #0077b6;
        } 
        a.gray,a.gray:visited {
          color: #495057;
        } 
        a.gray:hover,a.gray:active {
          color: #343A40;
        } 
        */
         
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
                BalanceAtTheBeginningOfThePeriod = dataInfo.Header.BalanceAtTheBeginningOfPeriod.ToString("N2", CultureInfo.InvariantCulture) + currency;
                FxpenseForThePeriod = dataInfo.Header.FxpenseForThePeriod.ToString("N2", CultureInfo.InvariantCulture) + currency;
                FceiptForThePeriod = dataInfo.Header.FceiptForThePeriod.ToString("N2", CultureInfo.InvariantCulture) + currency;
                BalanceAtTheEndOfThePeriod = dataInfo.Header.BalanceAtTheEndOfThePeriod.ToString("N2", CultureInfo.InvariantCulture) + currency;
            } 
        }
         
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (dataInfo.Header == null)
            {
                Navigation.NavigateTo("/");
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

                    await DebitBar();
                    await CreditBar();
                    await SortByMonth(0);
                }
                 
                var dotNetReference = DotNetObjectReference.Create(this);
                await jsRuntime.InvokeVoidAsync("setObjReference", dotNetReference);
            }
        }

        [JSInvokable]
        public async void RefreshChartPage()
        {
            string message = Localizer["Message1"];
            await jsRuntime.InvokeVoidAsync("alert", message);
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
            string strSum = double.Parse(String.Format("{0:0.00}", sumDebit)).ToString("N2", CultureInfo.InvariantCulture);
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
            string strSum = double.Parse(String.Format("{0:0.00}", sumCredit)).ToString("N2", CultureInfo.InvariantCulture);
            await jsRuntime.InvokeVoidAsync("Chart_2", strSum + currency);
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

                    if (newItem.Sum != "0")
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

                    if (newItem.Sum != "0")
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
            ClickedMonthStyle = ClickedStyle + "color: #0077b6;";
            ClickedWeekStyle = "color: #343A40;";

            await SortByMonth(0);
        }

        public async void ClickedWeek()
        {
            forward = 0;
            clickedMonth = false;
            ClickedWeekStyle = ClickedStyle + "color: #0077b6;";
            ClickedMonthStyle = "color: #343A40;";

            await SortByWeek(0);
        }

        async Task SortByMonth(int index)
        {
            await jsRuntime.InvokeVoidAsync("cleanDebitList");
            await jsRuntime.InvokeVoidAsync("cleanCreditList");
              
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
                    newItem.Amount = debitSums3[i];

                    await jsRuntime.InvokeVoidAsync("addDebit", newItem);
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
                    newItem.Amount = creditSums3[i];

                    await jsRuntime.InvokeVoidAsync("addCredit", newItem);
                }
            }
             
            await jsRuntime.InvokeVoidAsync("Chart_3");
        }

        async Task SortByWeek(int index)
        {
            await jsRuntime.InvokeVoidAsync("cleanDebitList");
            await jsRuntime.InvokeVoidAsync("cleanCreditList");

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

        private string getRandColor(System.Drawing.Color baseColor, float lightnessIncrement)
        {
            float h, s, l;
            ColorToHSL(baseColor, out h, out s, out l);

            l = Math.Max(Math.Min(l + lightnessIncrement, 1.0f), 0.0f); 
            System.Drawing.Color color = HSLToColor(h, s, l);

            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

            //Random rnd = new Random();
            //string hexOutput = String.Format("{0:X}", rnd.Next(0, 0xFFFFFF));
            //while (hexOutput.Length < 6)
            //    hexOutput = "0" + hexOutput;
            //return "#" + hexOutput;
        }

        System.Drawing.Color ChangeBrightness(System.Drawing.Color color, int amount)
        {
            int r = Math.Max(Math.Min(color.R + amount, 255), 0);
            int g = Math.Max(Math.Min(color.G + amount, 255), 0);
            int b = Math.Max(Math.Min(color.B + amount, 255), 0);
            return System.Drawing.Color.FromArgb(r, g, b);
        }

        System.Drawing.Color GetDarkerColor(System.Drawing.Color color, int increment)
        {
            int r = Math.Max(color.R - increment, 0);
            int g = Math.Max(color.G - increment, 0);
            int b = Math.Max(color.B - increment, 0);

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        void ColorToHSL(System.Drawing.Color color, out float h, out float s, out float l)
        {
            int r = color.R;
            int g = color.G;
            int b = color.B;

            float rf = r / 255.0f;
            float gf = g / 255.0f;
            float bf = b / 255.0f;

            float max = Math.Max(Math.Max(rf, gf), bf);
            float min = Math.Min(Math.Min(rf, gf), bf);

            h = 0;
            s = 0;
            l = (max + min) / 2;

            if (max != min)
            {
                float d = max - min;
                s = l > 0.5 ? d / (2 - max - min) : d / (max + min);

                if (max == rf)
                {
                    h = (gf - bf) / d + (gf < bf ? 6 : 0);
                }
                else if (max == gf)
                {
                    h = (bf - rf) / d + 2;
                }
                else if (max == bf)
                {
                    h = (rf - gf) / d + 4;
                }

                h /= 6;
            }
        }
         
        System.Drawing.Color HSLToColor(float h, float s, float l)
        {
            if (s == 0)
            {
                int gray = (int)(l * 255.0);
                return System.Drawing.Color.FromArgb(gray, gray, gray);
            }

            float q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            float p = 2 * l - q;

            float r = HueToRGB(p, q, h + 1.0f / 3);
            float g = HueToRGB(p, q, h);
            float b = HueToRGB(p, q, h - 1.0f / 3);

            return System.Drawing.Color.FromArgb((int)(r * 255.0), (int)(g * 255.0), (int)(b * 255.0));
        }

        float HueToRGB(float p, float q, float t)
        {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;

            if (t < 1.0f / 6) return p + (q - p) * 6 * t;
            if (t < 1.0f / 2) return q;
            if (t < 2.0f / 3) return p + (q - p) * (2.0f / 3 - t) * 6;

            return p;
        }
    }
}
