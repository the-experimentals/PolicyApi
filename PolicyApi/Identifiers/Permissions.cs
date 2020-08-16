using System;
namespace PolicyApi.Identifiers
{
    public class Permissions
    {
        public static IdentifierModel ADMIN_PERMISSION = new IdentifierModel()
        {
            CODE = "ADMIN_PERMISSION",
            NAME = "admin_permission",
            DISPLAY_NAME = "Give access to everything in application",
            DESCRIPTION = "This permission gives access to everything in entier application",
            POSITION = 1,
            CATEGORY = PermissionCategory.GENERAL.CODE
        };

        public static IdentifierModel CHANGE_PASSWORD = new IdentifierModel()
        {
            CODE = "CHANGE_PASSWORD",
            NAME = "change_password",
            DISPLAY_NAME = "Allow user to change his own profile password",
            DESCRIPTION = "",
            POSITION = 5,
            CATEGORY = PermissionCategory.USER_PROFILE.CODE
        };

        public static IdentifierModel SAVE_PROFILE = new IdentifierModel()
        {
            CODE = "SAVE_PROFILE",
            NAME = "save_profile",
            DISPLAY_NAME = "Allow user to save changes in his own profile",
            DESCRIPTION = "",
            POSITION = 6,
            CATEGORY = PermissionCategory.USER_PROFILE.CODE
        };
    }
}
