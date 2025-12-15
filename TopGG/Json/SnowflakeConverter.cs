using System.Text.Json;
using System.Text.Json.Serialization;

namespace TopGG.Json;

/// <summary>
/// JSON converter for Discord snowflake IDs (ulong as string).
/// </summary>
public class SnowflakeConverter : JsonConverter<ulong>
{
    /// <inheritdoc />
    public override ulong Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (ulong.TryParse(stringValue, out var result))
                return result;
            throw new JsonException($"Unable to parse '{stringValue}' as ulong");
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetUInt64();
        }

        throw new JsonException($"Unexpected token type {reader.TokenType} when parsing snowflake");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ulong value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

/// <summary>
/// JSON converter for a list of Discord snowflake IDs (List&lt;ulong&gt; as string[]).
/// </summary>
public class SnowflakeListConverter : JsonConverter<List<ulong>>
{
    /// <inheritdoc />
    public override List<ulong>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException($"Expected array, got {reader.TokenType}");

        var list = new List<ulong>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            if (reader.TokenType == JsonTokenType.String)
            {
                var stringValue = reader.GetString();
                if (ulong.TryParse(stringValue, out var result))
                    list.Add(result);
                else
                    throw new JsonException($"Unable to parse '{stringValue}' as ulong");
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                list.Add(reader.GetUInt64());
            }
            else
            {
                throw new JsonException($"Unexpected token type {reader.TokenType} in snowflake array");
            }
        }

        return list;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, List<ulong> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var id in value)
        {
            writer.WriteStringValue(id.ToString());
        }
        writer.WriteEndArray();
    }
}

/// <summary>
/// JSON converter for nullable Discord snowflake IDs (ulong? as string).
/// </summary>
public class NullableSnowflakeConverter : JsonConverter<ulong?>
{
    /// <inheritdoc />
    public override ulong? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
                return null;
            if (ulong.TryParse(stringValue, out var result))
                return result;
            throw new JsonException($"Unable to parse '{stringValue}' as ulong");
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetUInt64();
        }

        throw new JsonException($"Unexpected token type {reader.TokenType} when parsing nullable snowflake");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ulong? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToString());
        else
            writer.WriteNullValue();
    }
}
