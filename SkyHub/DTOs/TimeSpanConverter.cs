using Newtonsoft.Json;
using System;

public class TimeSpanConverter : Newtonsoft.Json.JsonConverter<TimeSpan>
{
    public override TimeSpan ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, TimeSpan existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
        if (reader.TokenType == Newtonsoft.Json.JsonToken.String)
        {
            var value = reader.Value.ToString();
            return TimeSpan.Parse(value); // Assuming the format is HH:mm:ss
        }
        throw new Newtonsoft.Json.JsonSerializationException("Invalid value for TimeSpan.");
    }

    public override void WriteJson(Newtonsoft.Json.JsonWriter writer, TimeSpan value, Newtonsoft.Json.JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(@"hh\:mm\:ss"));
    }
}
