using AnalysisDashboard.DataAccess;

namespace AnalysisDashboard.Services
{
    public class LoginService : AppBaseService, ILoginService
    {
        public LoginService(DashboardContext db)
           : base(db)
        {

        }
    }
}
