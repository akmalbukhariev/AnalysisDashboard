using Microsoft.AspNetCore.Components.Forms;

namespace AnalysisDashboard.Models
{
    public class RegistrationInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; } 
        public string CompanyName { get; set; }
        public string WebAddress { get; set; }
        public string Image { get; set; }

        public bool CheckInfo()
        {
            if(!FirstName.IsOk()||
                !LastName.IsOk()||
                !Login.IsOk() ||
                !Email.IsOk() ||
                !PhoneNumber.IsOk() ||
                !Password.IsOk())
                return false;

            return true;
        }
    }

    public class RegistrationInfoWithFile : RegistrationInfo
    {
        public IBrowserFile File { get; set; }

    }
}
