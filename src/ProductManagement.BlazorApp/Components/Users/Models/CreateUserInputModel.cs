﻿using System.ComponentModel.DataAnnotations;

namespace ProductManagement.BlazorApp.Components.Users.Models;

public class CreateUserInputModel
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
        
    [DataType(DataType.Text)]
    public string Role { get; set; } = string.Empty;
}
