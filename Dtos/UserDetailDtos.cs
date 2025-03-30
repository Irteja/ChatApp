using System.ComponentModel.DataAnnotations;

namespace chatapp.Dtos;

public class UserDetailDtos{

    public Guid Id{get;set;}
    [Required]
    public string Mail{get;set;}= string.Empty;
    [Required]
    public string Name{get;set;}= string.Empty;
    [Required]
    public string Phone{get;set;}= string.Empty;
    [Required]
    public string Avatar{get;set;}= string.Empty;
}