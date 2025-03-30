using System.ComponentModel.DataAnnotations;

namespace chatapp.Dtos;

public class MessageDtos
{

    [Required]
    public Guid ConversationId { get; set; }
    [Required]
    public Guid SenderId { get; set; }
    [Required]
    public Guid ReceiverId { get; set; }
    public string? Text { get; set; }
    public string? Attachment { get; set; }
}