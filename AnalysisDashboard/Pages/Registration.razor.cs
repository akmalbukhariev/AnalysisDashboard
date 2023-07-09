using AnalysisDashboard.Models;
using AnalysisDashboard.Response;
using AnalysisDashboard.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace AnalysisDashboard.Pages
{
    public partial class Registration
    {
        [Inject]
        IJSRuntime js { get; set; }

        [Inject]
        NavigationManager Navigation { get; set; }

        [Inject]
        IRegistrationService service { get; set; }

        [Parameter]
        public RegistrationInfoWithFile Model { get; set; } = null;

        [Parameter]
        public ResponseRegistration response { get; set; } = null;

        [Parameter]
        public string RepeatPassword { get; set; }

        [Parameter]
        public bool Agree { get; set; } = false;

        [Parameter]
        public bool ShowLoading { get; set; } = false;

        [Parameter]
        public string SelectedImage { get; set; } = "images/user.png";
        public Registration()
        {
            Model = new RegistrationInfoWithFile();
            Model.FirstName = "Akmal";
            Model.LastName = "Bukhariev";
            Model.Login = "Click";
            Model.Password = "123";
            Model.Email = "gmail.com";
            Model.PhoneNumber = "010844106941";
            Model.CompanyName = "Shuba";
            Model.WebAddress = "mail.ru";
        }

        public async Task ClickLogin()
        {
            if (!Model.CheckInfo())
            {
                await js.InvokeVoidAsync("alert", "Field cannot be empty!");
                return;
            }

            if (Model.Password != RepeatPassword)
            {
                await js.InvokeVoidAsync("alert", "Check the password!");
                return;
            }

            if (!Agree)
            {
                await js.InvokeVoidAsync("alert", "Please check agreement!");
                return;
            }

            ShowLoading = true;  
            response = await service.signUp(Model);
            ShowLoading = false;

            var message = response.result ? "Ok" : "Not Ok " + response.message;
            await js.InvokeVoidAsync("alert", message);
        }

        public async Task FileUploadOnChange(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null)
            {
                //var fileContent = new byte[file.Size];
                //await file.OpenReadStream().ReadAsync(fileContent);
                //var base64Image = Convert.ToBase64String(fileContent);

                //SelectedImage = $"data:{file.ContentType};base64,{base64Image}";

                try
                {
                    using var image = await Image.LoadAsync(file.OpenReadStream());
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(800, 600), // Adjust the desired dimensions accordingly
                        Mode = ResizeMode.Max
                    }));

                    // Convert the resized image to a base64 string
                    using var ms = new MemoryStream();
                    await image.SaveAsync(ms, new JpegEncoder());
                    var base64Image = Convert.ToBase64String(ms.ToArray());

                    // Set the selected image
                    SelectedImage = $"data:image/jpeg;base64,{base64Image}";
                    Model.File = file;
                }
                catch (Exception ex)
                {
                    await js.InvokeVoidAsync("alert", "Image size is too big!");
                }
            }
        } 
    }
}
