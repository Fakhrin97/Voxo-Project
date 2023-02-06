﻿using System.ComponentModel.DataAnnotations;

namespace Voxo.BLL.ViewModels
{
    public class RegisterVM
    {
        public string Fristname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmedPassword { get; set; }   
    }
}
