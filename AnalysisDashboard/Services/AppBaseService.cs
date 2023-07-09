using AnalysisDashboard.DataAccess;

namespace AnalysisDashboard.Services
{
    public abstract class AppBaseService
    {
        protected DashboardContext dataBase { get; set; }

        protected AppBaseService(DashboardContext db)
        {
            dataBase = db;
        }

        protected AppBaseService()
        {

        }
    }
}
