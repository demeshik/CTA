using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CTA.Repositories
{
    public interface ISignInManager
    {
        Task<SignInResult> Login(string username, string password, bool rememberme, bool isLock);
    }
}
