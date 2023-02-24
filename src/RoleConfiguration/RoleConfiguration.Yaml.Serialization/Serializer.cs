using YamlDotNet.Serialization;

namespace RoleConfiguration.Yaml.Serialization;

public sealed class Serializer
{
    private readonly ISerializer serializer;
    public Serializer() => serializer = new SerializerBuilder().Build();

    public string Serialize(object graph) => serializer.Serialize(graph);
}
