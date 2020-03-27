namespace AspNetCoreApi_Boilerplate.Common.ErrorMessages
{
    public class ErrorMessages
    {
        public static class Default
        {
            public static string NotEmpty(string propertyName)
            {
                return $"'{propertyName}' must not be empty.";
            }
            public static string MaximumLength(string propertyName, int maximumLength, int enteredLength)
            {
                return $"The length of '{propertyName}' must be {maximumLength} characters or fewer. You entered " + enteredLength + " characters.";
            }
        }

        public static class General
        {
            public static string DoesNotExistInDatabase(string propertyName)
            {
                return $"'{propertyName}' does not exist.";
            }

            public static string AlreadyExists(string parentProperty, string childProperty)
            {
                return $"'{parentProperty}' with this '{childProperty}' already exists.";
            }

            public static string ThisAreExistsWithThis(string prop1, string prop2)
            {
                return $"This '{prop1}' already exists with this '{prop2}'.";
            }

            public static string ThisDoesNotExistsWithThis(string prop1, string prop2)
            {
                return $"This '{prop1}' does not exist with this '{prop2}'.";
            }

            public static string AreNotAssociated(string prop1, string prop2)
            {
                return $"'{prop1}' does not belong to the '{prop2}'.";
            }
        }

        public class User
        {
            public const string EmailOrPasswordIsIncorrect = "Email or Password is incorrect.";

            public const string EmailAlreadyExists = "Email already exists.";

            public const string NotValidPhoneNumber = "The Phone Number was not valid.";

            public const string PasswordIsTooShort = "Password must be at least '8' characters.";

            public const string PasswordDoesNotHaveUpper = "Password must contain at least one uppercase character.";

            public const string PasswordDoesNotHaveDigit = "Password must contain at least one digit.";

            public const string PasswordDoesNotHaveSymbol = "Password must contain at least one symbol.";

        }
    }
}
