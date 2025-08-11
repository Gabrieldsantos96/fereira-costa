namespace Fereira.Costa.Shared.Consts;

public static class ApiRoutes
{
    public static class Authentication
    {
        public const string SignIn = "/auth/login";
        public const string Register = "/auth/register";
        public const string SignOut = "/auth/sign-out";
        public const string RefreshJwt = "/auth/refresh-jwt";
        public const string GetProfile = "/auth/get-profile";
    }
    public static class Users
    {
        public const string GetPaginatedUsers = "/features/users";
        public const string GetUserById = "/features/user/{id}";
        public const string UpdateUser = "/features/user/{id}";
        public const string CreateUser = "/features/user";
        public const string DeleteUser = "/features/user/{id}";
    }
}