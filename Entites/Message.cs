using System.ComponentModel.DataAnnotations;

namespace chatapp.Entities;

public class Message
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid ConversationId { get; set; }
    [Required]
    public Guid SenderId { get; set; }
    [Required]
    public Guid ReceiverId { get; set; }
    public string? Text { get; set; }
    public string? Attachment { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

}