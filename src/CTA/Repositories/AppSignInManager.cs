using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using CTA.Models;

namespace CTA.Repositories
{
    public class AppSignInManager : ISignInManager
    {
        private SignInManager<User> signInManager;
        public AppSignInManager(SignInManager<User> _signInManager)
        {
            signInManager = _signInManager;
        }

        public Task<SignInResult> Login(string username, string password, bool rememberme, bool isLock)
        {
            return signInManager.PasswordSignInAsync(username, password, rememberme, isLock);
        }
    }
}
