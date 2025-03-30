using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using chatapp.Data;
using chatapp.Dtos;
using chatapp.Entities;

namespace chatapp.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ChatAppDbContext context;
    private readonly IAuthService authService;
    public UserController(ChatAppDbContext _context,IAuthService _authService)
    {
        context = _context;
        authService=_authService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> UserLogIn([FromBody] LoginDtos user)
    {
        try
        {
            var exist = await context.Users.FirstOrDefaultAsync(usr =>
            usr.Mail == user.Mail
            );
            if (exist == null) return Unauthorized("Invalid email or password.");
            Console.WriteLine("mail ->"+user.Mail);
            // bool isValidPassword = BCryptPasswordHasher.VerifyPassword(user.Password, exist.Password);
            // if (!isValidPassword)
            //     return Unauthorized("Invalid email or password.");
            return Ok(new { jwtToken = authService.GenerateJwtToken(exist.Id) });
        }
        catch
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("signup")]
    public async Task<IActionResult> AddNewUser([FromBody] UserDtos user)
    {

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Mail = user.Mail,
            Phone = user.Phone,
            Name = user.Name,
            Password = BCryptPasswordHasher.HashPassword(user.Password),
            Avatar = user.Avatar
        };
        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync();

        return Ok(new {message="Added New User Successfully.",JwtToken = authService.GenerateJwtToken(newUser.Id) });
    }
    [HttpGet("getuserdetail")]
    public async Task<ActionResult<UserDetailDtos>> GetUserDetail([FromQuery] Guid Id)
    {
        try
        {
            var user = await context.Users.FindAsync(Id);

            if (user == null)
            {
                return NotFound();
            }

            var userDetail = new UserDetailDtos
            {
                Id = user.Id,
                Name = user.Name,
                Mail = user.Mail,
                Phone = user.Phone,
                Avatar = user.Avatar
            };

            return Ok(userDetail);
        }
        catch
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> check()
    {
        var allUser = await context.Users.ToListAsync();
        List<ContactListDtos>? finalRes = new List<ContactListDtos>();
        foreach (var SingleUser in allUser)
        {
            finalRes.Add(new ContactListDtos
            {
                Id = SingleUser.Id,
                Name = SingleUser.Name,
                Avatar = SingleUser.Avatar
            });
        }
        return Ok(finalRes);
    }

    [HttpGet("getcontactlist/{Userid}")]
    public async Task<IActionResult> getContactList(Guid Userid)
    {
        var contacts = await context.Conversations.Where(user =>
        user.CreatorId == Userid ||
        user.ParticipantId == Userid
        ).ToListAsync();

        var finalContacts = await MakeFinalContactList(Userid, contacts);
        return Ok(finalContacts);
    }

    private async Task<List<ContactListDtos>> MakeFinalContactList(Guid Userid, List<Conversation> contacts)
    {
        List<ContactListDtos> finalContacts = new List<ContactListDtos>();
        foreach (Conversation conversation in contacts)
        {
            Guid singleContactId = (conversation.CreatorId == Userid ? conversation.ParticipantId : conversation.CreatorId);
            var singleUser = await context.Users.FirstOrDefaultAsync(u =>
            u.Id == singleContactId
            );
            if (singleUser != null)
            {
                finalContacts.Add(new ContactListDtos
                {
                    Id = singleUser.Id,
                    Name = singleUser.Name,
                    Avatar = singleUser.Avatar,
                    conversationId = conversation.Id
                });
            }
        }
        return finalContacts;
    }
}