namespace RefinedShell.Matching
{
    internal static class RegexTokens
    {
        public const string Bell = @"\a";
        public const string Tab = "\t";
        public const string CarriageReturn = "\r";
        public const string VerticalTab = "\v";
        public const string Newline = "\n";
        public const string Null = "\0";

        //Anchors
        public const string StartOfLine = "^";
        public const string EndOfLine = "$";

        public const string Or = "|";

        public const string WordCharacters = "abcdefjhijklmnopqrstuvwxyzABCDEFJHIJKLMNOPQRSTUVWXYZ0123456789_";
        //public const string NonWordCharacters = @" .,;!?@#$%*+/|\~^[]{}&()-='<>\n\r";
        //public const string AllCharacters = WordCharacters + NonWordCharacters;
        public const string Whitespaces = " \t\n\r\f\v";
        public const string Numbers = "0123456789";
    }
}
