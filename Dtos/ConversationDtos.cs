using System.ComponentModel.DataAnnotations;

namespace chatapp.Dtos;

public class ConversationDtos{
    [Required]
    public Guid CreatorId{get;set;}
    [Required]
    public Guid ParticipantId{get;set;}
}