using System.ComponentModel.DataAnnotations;

namespace chatapp.Dtos;

public class LoginDtos{

    [Required]
    public string Mail{get;set;}= string.Empty;
    [Required]
    public string Password{get;set;}= string.Empty;

}