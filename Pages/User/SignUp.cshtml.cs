using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using chatapp.Dtos;
using chatapp.Entities;

namespace chatapp.Pages.UserProfile;
public class SignupModel : PageModel
{
    private readonly HttpClient _httpClient;
    // private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment environment;
    public SignupModel(IWebHostEnvironment _environment, HttpClient httpClient)
    {
        _httpClient = httpClient;
        environment = _environment;
    }

    [BindProperty]
    public string? Name { get; set; }

    [BindProperty]
    public string? Mail { get; set; }

    [BindProperty]
    public string? Phone { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    [BindProperty]
    public string? ConfirmPassword { get; set; }
    [BindProperty]
    public IFormFile? AvatarFile { get; set; }

    public string? Avatar { get; set; }

    public string? Message { get; set; }

    public IActionResult OnGet()
    {

        // Any setup before the page renders
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Check if passwords match
        if (Password != ConfirmPassword)
        {
            Message = "Passwords do not match.";
            return Page();
        }
        Avatar = await SaveAvatarFile(AvatarFile);
        //  Console.WriteLine("file :",Avatar);
        // Create a new student DTO
        var user = new UserDtos
        {
            Name = Name,
            Mail = Mail,
            Phone = Phone,
            Password = Password,
            Avatar = Avatar
        };

        // Send request to API for creating a student
        var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

        // Assuming your API is running locally, update this URL if needed
        var response = await _httpClient.PostAsync("http://localhost:5124/api/user/signup", content);

        if (!response.IsSuccessStatusCode)
        {
            Message = "An error occurred while creating the account.";
            return Page();
        }
        // var responseData = JsonSerializer.Deserialize<LoginResponseDtos>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // if (responseData != null)
        // {
        // Store JWT token in cookies
        // CookieHelper.SetJwtCookie(HttpContext, responseData.JwtToken);
        // }
        // Redirect to login or some other page after successful registration
        return RedirectToPage("/Index");
    }


    private async Task<string?> SaveAvatarFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null;

        // Define the folder to store avatars (e.g., wwwroot/images/avatars)
        string uploadsFolder = Path.Combine(environment.WebRootPath, "images/avatars");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        // Get the original file name without the extension
        string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
        // Get the file extension (e.g., .jpg, .png)
        string fileExtension = Path.GetExtension(file.FileName);
        // Generate a unique Guid
        string uniqueGuid = Guid.NewGuid().ToString();
        // Combine Guid + OriginalFileName + Extension
        string uniqueFileName = $"{uniqueGuid}{originalFileName}{fileExtension}";
        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Save the file
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        // Return the relative path (e.g., "/images/avatars/abc123profile.jpg")
        return $"/images/avatars/{uniqueFileName}";
    }


}
