﻿namespace PokesMan.Dto
{
   
        public class RegisterModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
      public class LoginModel
       {
        public string Email { get; set; }
          public string Password { get; set; }
       }

    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }

}
