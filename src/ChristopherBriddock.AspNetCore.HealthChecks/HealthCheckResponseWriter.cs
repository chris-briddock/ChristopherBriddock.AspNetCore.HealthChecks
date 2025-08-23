using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ChristopherBriddock.AspNetCore.HealthChecks;

public static class HealthCheckResponseWriter
{
    internal static async Task WriteResponse(HttpContext context,
                                             HealthReport healthReport)
    {
        context.Response.ContentType = "application/json; charset=utf-8";

        var options = new JsonWriterOptions { Indented = true };

        using var memoryStream = new MemoryStream();
        using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
        {
            WriteHealthReport(jsonWriter, healthReport);
        }

        memoryStream.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(memoryStream, Encoding.UTF8);
        var json = await reader.ReadToEndAsync();
        await context.Response.WriteAsync(json);
    }

    private static void WriteHealthReport(Utf8JsonWriter writer,
                                          HealthReport report)
    {
        writer.WriteStartObject();
        writer.WriteString("status", report.Status.ToString());
        writer.WriteStartObject("results");

        foreach (var entry in report.Entries)
        {
            WriteHealthReportEntry(writer, entry.Key, entry.Value);
        }

        writer.WriteEndObject();
        writer.WriteEndObject();
    }

    private static void WriteHealthReportEntry(Utf8JsonWriter writer, string key, HealthReportEntry entry)
    {
        writer.WriteStartObject(key);
        writer.WriteString("status", entry.Status.ToString());
        writer.WriteString("description", entry.Description);
        writer.WriteStartObject("data");

        foreach (var item in entry.Data)
        {
            writer.WritePropertyName(item.Key);
            JsonSerializer.Serialize(writer, item.Value, item.Value?.GetType() ?? typeof(object));
        }

        writer.WriteEndObject();
        writer.WriteEndObject();
    }
}