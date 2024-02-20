using Newtonsoft.Json.Serialization;

namespace YaDiskBackup.YandexDisk.Client.Http.Serialization;

internal class SnakeCasePropertyNamesContractResolver : DefaultContractResolver
{
    protected override string ResolvePropertyName(string propertyName)
    {
        return SnakeCasePropertyResolver.ToSnakeCase(propertyName);
    }
}