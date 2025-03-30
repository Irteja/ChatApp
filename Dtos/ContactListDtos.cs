using System.ComponentModel.DataAnnotations;

namespace chatapp.Dtos;

public class ContactListDtos{
  public Guid Id{get;set;}
  public string Name{get;set;}=string.Empty;
  public string Avatar{get;set;}=string.Empty;
  public Guid conversationId{get;set;} 

}