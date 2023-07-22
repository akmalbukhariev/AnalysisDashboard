using AnalysisDashboard.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace AnalysisDashboard.Pages
{
    public class BasePage : ComponentBase
    {
        [Inject]
        public DataInfo dataInfo { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        [Parameter]
        public bool ShowLoading { get; set; } = false;

        [Inject]
        protected IJSRuntime jsRuntime { get; set; }

        [Inject]
        protected IStringLocalizer<App> Localizer { get; set; }

        public BasePage()
        {
            
        }
    }
}
