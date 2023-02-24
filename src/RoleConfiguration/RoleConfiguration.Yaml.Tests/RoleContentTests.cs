using RoleConfiguration.Yaml.Serialization;
using System.ComponentModel.DataAnnotations;
using Xunit;
using YamlDotNet.Core;

namespace RoleConfiguration.Yaml.Tests;

public class RoleContentTests
{
    [Fact]
    public void Can_Deserialize_Yaml()
    {
        var content = File.ReadAllText(".\\Yaml\\RoleCorrect.yaml");
        var deserializer = new Deserializer();
        var test = deserializer.DeserializeRoleTree(content);

        Assert.NotNull(test);
        Assert.True(test.Trees.Count > 0);
        Assert.True(test.Roles.Count > 0);
    }

    [Fact]
    public void Can_Serialize_Yaml()
    {
        var content = File.ReadAllText(".\\Yaml\\RoleCorrect.yaml");
        var deserializer = new Deserializer();
        var test = deserializer.DeserializeRoleTree(content);

        Assert.NotNull(test);

        var serializer = new Serializer();
        var newContent = serializer.Serialize(test);
        var test2 = deserializer.DeserializeRoleTree(newContent);

        Assert.Equal(test.DefaultRoleTree, test2.DefaultRoleTree);
        Assert.Equal(test.Trees.Count, test2.Trees.Count);
        Assert.Equal(test.Roles.Count, test2.Roles.Count);
    }

    [Fact]    
    public void Fail_Deserialize_Yaml_Wrong_Property_Name()
    {
        var content = File.ReadAllText(".\\Yaml\\RoleIncorrectName.yaml");
        var deserializer = new Deserializer();
        Assert.Throws<YamlException>(() => deserializer.DeserializeRoleTree(content));
    }

    [Fact]    
    public void Fail_Deserialize_Yaml_Missing_Required()
    {
        var content = File.ReadAllText(".\\Yaml\\RoleMissingRequired.yaml");
        var deserializer = new Deserializer();
        Assert.Throws<ValidationException>(() => deserializer.DeserializeRoleTree(content));
    }
}