namespace EPMS.Shared.Constants
{
    public static class CacheKeys
    {
        // User-related cache keys
        public static string UserByEmail(string email) => $"user:email:{email.ToLower()}";
        public static string UserById(long id) => $"user:id:{id}";
        public static string UserRoles(long userId) => $"user:{userId}:roles";
        public static string UserPermissions(long userId) => $"user:{userId}:permissions";

        // Role-related cache keys
        public static string RoleById(int roleId) => $"role:{roleId}";
        public static string AllRoles() => "roles:all";

        // Permission-related cache keys
        public static string PermissionById(int permissionId) => $"permission:{permissionId}";
        public static string AllPermissions() => "permissions:all";

        // System settings cache keys
        public static string SystemSetting(string key) => $"setting:{key}";
        public static string DefaultPassword() => "setting:defaultpassword";

        // General auth cache keys
        public static string AuthTokenBlacklist(string token) => $"auth:blacklist:{token}";
    }
}
