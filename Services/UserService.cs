﻿using DemoVenueRental.Models;
using DemoVenueRental.Sql;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DemoVenueRental.Services
{
    public interface IUserService
    {
        Task<bool> IsEmailExist(string Email);
        Task<ResultData<int>> Register(Register model);
        Task<ResultData<int>> Login(Login model);
        Task Logout();
        bool IsLogged();
        int GetUserId();
        bool CheckRole(ClaimsPrincipal User,string Role);
    }

    public class UserService : IUserService
    {
        private readonly IUserData _userData;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserData userData, IHttpContextAccessor httpContextAccessor)
        {
            _userData = userData;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool CheckRole(ClaimsPrincipal User, string Role)
        {
            if (User.IsInRole(Role))
            {
                return true;
            }
            return false;
        }

        public int GetUserId()
        {
            int UserId = 0;
            if (_httpContextAccessor.HttpContext != null)
            {
                int.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid), out UserId);
            }
            return UserId;
        }

        public bool IsLogged()
        {
            return _httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User.Identity != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public async Task<bool> IsEmailExist(string Email)
        {
            return await _userData.IsEmailExist(Email);
        }

        public async Task<ResultData<int>> Login(Login model)
        {
            var result = await _userData.Login(model);
            var result2 = await Logined(result);
            result.state = result2;
            result.data = 0;
            return result;
        }

        public async Task<ResultData<int>> Register(Register model)
        {
            var result = await _userData.Register(model);
            await Logined(result);
            result.data = 0;
            return result;
        }

        public async Task Logout()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }

        private async Task<bool> Logined(ResultData<int> model)
        {
            if (!model.state)
                return false;

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid, model.data.ToString()),
                new Claim(ClaimTypes.Role, "User"),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                IssuedUtc = DateTimeOffset.UtcNow,
            };

            if (_httpContextAccessor.HttpContext == null)
            {
                return false;
            }

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return true;
        }
    }
}
