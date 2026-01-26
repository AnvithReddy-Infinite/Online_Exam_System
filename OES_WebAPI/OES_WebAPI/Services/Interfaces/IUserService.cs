using FinalProject.Common;
using FinalProject.Models.DTOs;
using OES_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Services.Interfaces
{
    public interface IUserService
    {
        ApiResponse<object> RegisterUser(RegisterUserDTO model);
        ApiResponse<object> LoginUser(LoginUserDTO model);
        ApiResponse<string> ResetPassword(ResetPasswordDTO model);
    }
}