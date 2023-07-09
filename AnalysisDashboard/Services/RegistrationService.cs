using AnalysisDashboard.DataAccess;
using AnalysisDashboard.Helper;
using AnalysisDashboard.Models;
using AnalysisDashboard.Response;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore; 

namespace AnalysisDashboard.Services
{
    public class RegistrationService : AppBaseService, IRegistrationService
    {
        public static IWebHostEnvironment _environment;
        public RegistrationService(DashboardContext db, IWebHostEnvironment environment)
           : base(db)
        {
            _environment = environment;
        }

        public async Task<ResponseRegistration> signUp(RegistrationInfoWithFile info)
        {
            ResponseRegistration response = new();

            RegistrationInfo foundUser = await dataBase.RegistrationInfos.Where(item => item.PhoneNumber == info.PhoneNumber)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (foundUser != null)
            { 
                response.message = Constants.Exist;
                return response;
            }

            info.Image = "";
            if (info.File != null)
            {
                await CreateFile($"{_environment.WebRootPath}{Constants.SaveUserImagePath}", info.File);
                info.Image = Constants.SaveUserImagePath.Replace("\\", "/") + info.File.Name;
            }

            response.result = true; 
            dataBase.Add(info);
            await dataBase.SaveChangesAsync();

            return response;
        }

        private async Task CreateFile(string savePath, IBrowserFile file)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            if (file.Size <= 0) return;

            long maxFileSize = file.Size;//1024 * 15;
            //using (var filestream = File.Create($"{savePath}{file.Name}"))
            
            await using FileStream fs = new($"{savePath}{file.Name}", FileMode.Create);
            await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
        }
    }
}
