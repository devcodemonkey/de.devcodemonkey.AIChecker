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

        public static JsonElement? ConvertToJsonFormat(string? json)
        {
            if(json == null)
                return null;
            if (!IsValidJson(json))
                throw new JsonException("Invalid JSON");
            return JsonDocument.Parse(json).RootElement;
        }
    }
}
