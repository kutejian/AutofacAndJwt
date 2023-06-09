﻿using Autofac.Extras.DynamicProxy;
using WebWithAutofacAndJwt.Interceptor;
using WebWithAutofacAndJwt.Model;

namespace WebWithAutofacAndJwt.Service;
[Intercept(typeof(UserInterceptor))]
public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;


    private readonly IDictionary<string, string> _users = new Dictionary<string, string>
        {
            { "test1", "password1" },
            { "test2", "password2" },
            { "admin", "admin" }
        };
    // inject your database here for user validation
    public UserService(ILogger<UserService> logger)
    {
        _logger = logger;
    }

    public bool IsValidUserCredentials(string userName, string password)
    {
        _logger.LogInformation("Validating user [{userName}]", userName);
        if (string.IsNullOrWhiteSpace(userName))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        return _users.TryGetValue(userName, out var p) && p == password;
    }

    public bool IsAnExistingUser(string userName)
    {
        return _users.ContainsKey(userName);
    }

    public string GetUserRole(string userName)
    {
        if (!IsAnExistingUser(userName))
        {
            return string.Empty;
        }

        if (userName == "admin")
        {
            return UserRoles.Admin;
        }

        return UserRoles.BasicUser;
    }


}

