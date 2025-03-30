using System.ComponentModel.DataAnnotations;

namespace chatapp.Entities;

public class User{
    [Key]
    public Guid Id{get;set;}
    [Required]
    public string Mail{get;set;}= string.Empty;
    [Required]
    public string Name{get;set;}= string.Empty;
    [Required]
    public string Phone{get;set;}= string.Empty;
    [Required]
    public string Password{get;set;}= string.Empty;
    [Required]
    public string Avatar{get;set;}= string.Empty;
}