using System.Text.Json.Serialization;
using StudioClock.Models;

namespace StudioClock.Serialization;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(AppSettings))]
internal partial class AppJsonSerializerContext : JsonSerializerContext;
