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
public class LoginModel : PageModel
{

    public  IActionResult OnGet(){
        if(User.Identity.IsAuthenticated){
            return RedirectToPage("/Conversations/Index");
        }

        return Page();
    }

}