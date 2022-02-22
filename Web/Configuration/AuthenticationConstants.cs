// This code is under Copyright (C) 2021 of Cegid SAS all right reserved

namespace Api.Configuration
{
    internal static class AuthenticationConstants
    {
        public static class Setup
        {
            public const string Audience = "simpleApiTemplateAudience";
            public const string TokenType = "at+jwt";
        }

        public static class Policies
        {
            public const string SimpleApiTemplate = "SimpleApiTemplatePolicy";
            public const string SimpleApiTemplateScope = "simpleApiTemplate";
        }
    }
}
