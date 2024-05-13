namespace ifm.IoTCore.UserManager;

using System;
using System.IO;
using Common;
using Contracts;

public class UserManager : IUserManager
{
    private class User
    {
        public readonly string Name;
        public readonly string Password;

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
    private readonly User _user;

    public string Scheme => "standard";

    public bool IsAuthenticationRequired { get; }

    public UserManager()
    {
        var userFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ifm", "iotcore", "users.txt");

        if (!File.Exists(userFile)) return;
        using var fileStream = File.OpenText(userFile);
        var user = fileStream.ReadLine();
        if (user == null) return;
        var name = user.Left(':');
        var password = user.Right(':');
        _user = new User(name, password);
        IsAuthenticationRequired = true;
    }

    public bool Authenticate(string user, string password)
    {
        return _user == null || _user.Name == user && _user.Password == password;
    }
}