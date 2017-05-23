using Newtonsoft.Json.Linq;

namespace Task4.Extensions
{
    public static class StringExtensions
    {
        public static bool IsValidJson(this string json)
        {
            var isValid = true;

            try
            {
                JToken.Parse(json);
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }
    }
}
