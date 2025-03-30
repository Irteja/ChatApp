using System.ComponentModel.DataAnnotations;

namespace chatapp.Entities;

public class Conversation
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid CreatorId { get; set; }
    [Required]
    public Guid ParticipantId { get; set; }
    [Required]
    public DateTime LastUpdate { get; set; } = DateTime.Now;
}