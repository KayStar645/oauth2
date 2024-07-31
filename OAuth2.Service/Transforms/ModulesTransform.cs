namespace OAuth2.Infrastructure.Transforms
{
    public static class Modules
    {
        public const int UserNameMin = 3;
        public const int UserNameMax = 50;
        public const int PasswordMin = 6;
        public const int PasswordMax = 50;
        public const int NameMin = 3;
        public const int NameMax = 190;
        public const int PhoneNumberLength = 10;
        public const int DescriptionLength = 6000;

        public static class User
        {
            public const string Module = "User";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string Email = "Email";
            public const string PhoneNumber = "PhoneNumber";
            public const string UserName = "UserName";
            public const string Password = "Password";
            public const string ConfirmPassword = "ConfirmPassword";
        }

        public static class Role
        {
            public const string Module = "Role";
            public const string Name = "Name";
            public const string Description = "Description";
        }

        public static class UserRole
        {
            public const string UserId = "UserId";
            public const string RoleId = "RoleId";
        }

        public static class UserPermission
        {
            public const string UserId = "UserId";
            public const string PermissionId = "RoleId";
        }

        public static class Permission
        {
            public const string Module = "Permission";
            public const string Name = "Name";
        }
    }
}
