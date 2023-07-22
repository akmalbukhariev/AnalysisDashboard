using Microsoft.AspNetCore.Components;

namespace AnalysisDashboard.Pages
{
    public class LoginBase : BasePage
    { 
        public LoginBase()
        {
            
        }

        private void ClickLogin()
        {
            Navigation.NavigateTo("statistics", true);
        }
    }
}
