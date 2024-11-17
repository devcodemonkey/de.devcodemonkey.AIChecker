using System.Text.Json;

namespace de.devcodemonkey.AIChecker.UseCases.Global
{
    public class JsonValidator
    {
        public static bool IsValidJson(string? json)
        {
            if (json == null)
                return false;
            try
            {
                JsonDocument.Parse(json);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }
    }
}
