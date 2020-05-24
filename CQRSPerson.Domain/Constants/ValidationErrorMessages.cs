namespace CQRSPerson.Domain.Constants
{
    public static class ValidationErrorMessages
    {
        public static string CannotBeNullEmptyOrWhiteSpace = "{PropertyName} cannot be null, empty, or white space";
        public static string InvalidInteger = "{PropertyName} must be a integer greater than or equal to zero";
        public static string MaximumCharacterLimit = "{PropertyName} exeeds the maximum character limit of ";
        public static string PropertyErrorMessage(string field, string value, string error)
        {
            switch (value)
            {
                case null:
                    value = "(null)";
                    break;
                case "":
                    value = "(empty)";
                    break;
                default:
                    {
                        if (value.Trim().Length == 0)
                        {
                            value = "(whitespace)";
                        }
                        break;
                    }
            }
            return $"Property: {field} ,Value: {value}, Error: {error}.";
        }
    }
}
