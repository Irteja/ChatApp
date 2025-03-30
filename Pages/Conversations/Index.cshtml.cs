using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using chatapp.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using chatapp.Data;
using chatapp.Dtos;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authorization;

namespace chatapp.Pages.Conversations
{
    public class ChatModel : PageModel
    {
        private readonly ChatAppDbContext _context; // Assuming EF Core
        private readonly HttpClient _httpClient; // For API calls (optional)

        public ChatModel(ChatAppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public List<ContactListDtos> ContactList { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
        public ContactListDtos? SelectedUser { get; set; } = new();

        [BindProperty]
        public string? NewMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid? SelectedUserId { get; set; }
        [BindProperty]
        public Guid? SelectedUserConversationId { get; set; }

        public Guid? currentUserId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Console.WriteLine("checking"+User.Identity.IsAuthenticated);
            if (!User.Identity.IsAuthenticated)
            {
                // Console.WriteLine("came");
                return LocalRedirect("/login.html");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value;
            currentUserId = Guid.Parse(userId);
            // foreach (var claim in User.Claims)
            // {
            //     Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            // }

            Console.WriteLine("Current user id:  "+ currentUserId);

            await GenerateAllContactList(currentUserId.Value);

            // Fetch messages for the selected user
            if (SelectedUserId.HasValue)
            {
                SelectedUser = GetSelectedUser(SelectedUserId.Value);
                SelectedUserConversationId = SelectedUser.conversationId;
                // Console.WriteLine(SelectedUser.conversationId);
                Messages = await LoadMessages();

            }
            return Page();
        }

        private ContactListDtos GetSelectedUser(Guid id)
        {
            return ContactList.FirstOrDefault(user => user.Id == id) ?? new ContactListDtos();
        }

        private async Task GenerateAllContactList(Guid currentUserId)
        {
            string apiUrl = $"http://localhost:5124/api/user/getcontactlist/{currentUserId}";

            var response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                ContactList = JsonSerializer.Deserialize<List<ContactListDtos>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<ContactListDtos>();
            }
        }

        private async Task<List<Message>> LoadMessages()
        {
            string apiUrlForMessages = $"http://localhost:5124/api/conversation/{SelectedUser.conversationId}";
            var responseForMessages = await _httpClient.GetAsync(apiUrlForMessages);
            if (responseForMessages.IsSuccessStatusCode)
            {
                var jsonString = await responseForMessages.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Message>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? new List<Message>();
            }
            return new List<Message>();
        }

    }


}