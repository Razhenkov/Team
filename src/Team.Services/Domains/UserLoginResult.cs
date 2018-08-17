using System;
using System.Collections.Generic;
using System.Text;

namespace Team.Services.Domains
{
    public enum UserLoginResult
    {
        Success = 1,
        NotFound,
        InvalidCredentials
    }
}
