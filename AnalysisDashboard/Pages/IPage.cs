using Microsoft.AspNetCore.Components;

namespace AnalysisDashboard.Pages
{
    public class IPage : ComponentBase
    {
        [Inject]
        protected NavigationManager Navigation { get; set; }

        [Parameter]
        public bool ShowLoading { get; set; } = false;

        public IPage()
        {
            
        }
    }
}
