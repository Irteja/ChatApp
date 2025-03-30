using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using chatapp.Data;
using chatapp.Dtos;
using chatapp.Entities;

namespace chatapp.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ConversationController : ControllerBase
{
    private readonly ChatAppDbContext context;

    public ConversationController(ChatAppDbContext _context)
    {
        context = _context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateNewConversation([FromBody] ConversationDtos UserIds)
    {
        var existingConversation = await context.Conversations
        .FirstOrDefaultAsync(conversation =>
            (conversation.CreatorId == UserIds.CreatorId && conversation.ParticipantId == UserIds.ParticipantId) ||
            (conversation.CreatorId == UserIds.ParticipantId && conversation.ParticipantId == UserIds.CreatorId)
        );

        if (existingConversation != null)
        {
            return Ok();
        }
        var newConversation = new Conversation
        {
            Id = Guid.NewGuid(),
            CreatorId = UserIds.CreatorId,
            ParticipantId = UserIds.ParticipantId,
            LastUpdate = DateTime.UtcNow
        };
        await context.Conversations.AddAsync(newConversation);
        await context.SaveChangesAsync();
        return Ok(newConversation);

    }
    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] MessageDtos messageDto)
    {
        if (messageDto == null || messageDto.ConversationId == Guid.Empty ||
            messageDto.SenderId == Guid.Empty || messageDto.ReceiverId == Guid.Empty)
        {
            return BadRequest("Invalid message data.");
        }
        var conversationExists = await context.Conversations.AnyAsync(c => c.Id == messageDto.ConversationId);
        if (!conversationExists)
        {
            return NotFound("Conversation not found.");
        }
        var newMessage = new Message
        {
            Id = Guid.NewGuid(),
            ConversationId = messageDto.ConversationId,
            SenderId = messageDto.SenderId,
            ReceiverId = messageDto.ReceiverId,
            Text = messageDto.Text,
            Attachment = messageDto.Attachment,
            CreatedAt = DateTime.UtcNow
        };
        await context.Messages.AddAsync(newMessage);
        await context.SaveChangesAsync();

        return Ok(newMessage);

    }
    [HttpGet("{conversationId}")]
    public async Task<IActionResult> GetMessagesByConversation(Guid conversationId)
    {
        var messages = await context.Messages
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        if (messages == null || messages.Count == 0)
        {
            return NotFound("No messages found for this conversation.");
        }

        return Ok(messages);
    }
}