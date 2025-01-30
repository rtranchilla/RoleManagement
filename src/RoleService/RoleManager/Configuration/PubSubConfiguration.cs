using Microsoft.Extensions.Configuration;

namespace RoleManager.Configuration;

public sealed class PubSubConfiguration(IConfiguration configuration)
{
    public string Name { get; } = configuration["PubSubName"] ?? "pubsub";
    public PubSubTopics Topic { get; } = new PubSubTopics();

    public sealed class PubSubTopics
    {
        internal PubSubTopics() { }

        public PubSubTopic Members { get; } = new PubSubTopic("members");
        public PubSubTopic Trees { get; } = new PubSubTopic("trees");
        public PubSubTopic Nodes { get; } = new PubSubTopic("nodes");
        
        public sealed class PubSubTopic
        {
            internal PubSubTopic(string name) => this.Name = name;

            public string Name { get; }
            public string Created => $"{Name}-created";
            public string Updated => $"{Name}-updated";
            public string Deleted => $"{Name}-deleted";
        }
    }
}
