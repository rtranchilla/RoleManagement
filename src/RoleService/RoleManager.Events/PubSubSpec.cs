namespace RoleManager.Events;

public static class PubSubSpec
{
    public static string Name { get; } = "pubsub";
    public static string TopicMembers { get; } = "members";
    public static string TopicTrees { get; } = "trees";
    public static string TopicNodes { get; } = "nodes";
}