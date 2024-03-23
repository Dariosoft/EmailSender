namespace Dariosoft.EmailSender.I18n
{
    public static class MessagesX
    {
        public static string Error_MissingRequiredField(string fieldName)
            => Messages.Error_MissingRequiredField.Replace("@Field", fieldName);

        public static string Error_UserNameIsReserved(string username)
            => Messages.Error_UserNameIsReserved.Replace("@UserName", username);
  
        public static string Error_InvalidFieldValue(string fieldName)
            => Messages.Error_InvalidFieldValue.Replace("@Field", fieldName);

        public static string Error_TooSimplePassword(ushort minLen)
            => Messages.Error_TooSimplePassword.Replace("@Len", minLen.ToString());
   
    }
}
