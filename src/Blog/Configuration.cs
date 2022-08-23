﻿namespace Blog;

public static class Configuration
{
    public static string JwtKey = string.Empty;
    public static string ApiKeyName = string.Empty;
    public static string ApiKey = string.Empty;
    public static SmtpConfiguration Smtp = new();

    public class SmtpConfiguration
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 25;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
