using AnalysisDashboard.Models;
using AnalysisDashboard.Response;

namespace AnalysisDashboard.Services
{
    public interface IRegistrationService
    {
        public Task<ResponseRegistration> signUp(RegistrationInfoWithFile info);
    }
}
