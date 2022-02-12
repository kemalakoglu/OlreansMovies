using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Movies.Core.Response
{
    public static class ResponseCodes
    {
        public const string Success = "RC0000";
        public const string NotNullable = "RC0001";
        public const string Failed = "RC0002";
        public const string NotFound = "RC0003";
        public const string Unauthorized = "RC0004";
        public const string BadRequest = "RC0005";
    }
}