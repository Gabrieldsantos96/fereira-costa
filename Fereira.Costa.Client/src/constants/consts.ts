export const AUTH_STORAGE_KEY = "auth_issuer";

export const APP_THEME = "app_theme";

export const REFRESH_TOKEN_STORAGE_KEY = "issuer";

export const Routes = {
  Authentication: {
    SignIn: "/auth/login",
    Register: "/auth/register",
    SignOut: "/auth/sign-out",
    RefreshJwt: "/auth/refresh-jwt",
    GetProfile: "/auth/get-profile",
  },
  Users: {
    GetPaginatedUsers: "features/users",
    GetUserById: "features/user/{id}",
    UpdateUser: "features/user/{id}",
    DeleteUser: "features/user/{id}",
  },
};
