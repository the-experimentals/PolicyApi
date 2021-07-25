using System;
namespace PolicyApi.Identifiers
{
    public class PermissionCategory
    {
        public static IdentifierModel GENERAL = new IdentifierModel()
        {
            CODE = "GENERAL",
            NAME = "general",
            DISPLAY_NAME = "General",
            DESCRIPTION = "",
            POSITION = 1
        };

        public static IdentifierModel USER_PROFILE = new IdentifierModel()
        {
            CODE = "USER_PROFILE",
            NAME = "user_profile",
            DISPLAY_NAME = "User profile",
            DESCRIPTION = "",
            POSITION = 2
        };

        public static IdentifierModel INVOICE = new IdentifierModel()
        {
            CODE = "INVOICE",
            NAME = "invoice",
            DISPLAY_NAME = "Invoice",
            DESCRIPTION = "",
            POSITION = 3
        };

        public static IdentifierModel TASK = new IdentifierModel()
        {
            CODE = "TASK",
            NAME = "task",
            DISPLAY_NAME = "Task",
            DESCRIPTION = "",
            POSITION = 4
        };

        public static IdentifierModel FEEDBACK = new IdentifierModel()
        {
            CODE = "FEEDBACK",
            NAME = "feedback",
            DISPLAY_NAME = "Feedback",
            DESCRIPTION = "",
            POSITION = 5
        };
    }
}
