namespace EPMS.Shared.Constants
{
    public static class CacheKeys
    {
        public static class Auth
        {
            public static string UserByEmail(string email) => $"auth:user:email:{email.ToLower()}";
            public static string UserById(long id) => $"auth:user:id:{id}";
            public static string UserRoles(long userId) => $"auth:user:{userId}:roles";
            public static string UserPermissions(long userId) => $"auth:user:{userId}:permissions";

            public static string RoleById(int roleId) => $"auth:role:{roleId}";
            public static string AllRoles() => "auth:roles:all";

            public static string PermissionById(int permissionId) => $"auth:permission:{permissionId}";
            public static string AllPermissions() => "auth:permissions:all";

            public static string TokenBlacklist(string token) => $"auth:blacklist:{token}";
        }

        public static class Hr
        {
            public static string LevelLookups() => "hr:levels:lookups";
            public static string TeamLookups() => "hr:teams:lookups";
            public static string PositionLookups() => "hr:positions:lookups";
            public static string DepartmentLookups() => "hr:departments:lookups";
            public static string EmployeeById(long id) => $"hr:employee:id:{id}";
        }

        public static class System
        {
            public static string Setting(string key) => $"system:setting:{key.ToLower()}";
            public static string DefaultPassword() => "system:setting:defaultpassword";
        }
    }
}
