using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoleManager.RoleHierarchy;

public sealed class ForestMap : IReadOnlyDictionary<Guid, TreeMap>
{
    internal ForestMap(IEnumerable<Role> roles) 
    {
        var treeLists = new Dictionary<Guid, List<Role>>();
        foreach (var role in roles)
            if (treeLists.ContainsKey(role.TreeId))
                treeLists[role.TreeId].Add(role);
            else
                treeLists[role.TreeId] = new List<Role> { role };

        trees = treeLists.ToDictionary(kvp => kvp.Key, kvp => new TreeMap(kvp.Value[0].Tree!, kvp.Value.ToArray()));
    }

    public static ForestMap Build(IEnumerable<Role> roles) => new(roles);

    private readonly IReadOnlyDictionary<Guid, TreeMap> trees;
    #region Dictionary Methods
    public TreeMap this[Guid key] => trees![key];
    public IEnumerable<Guid> Keys => trees!.Keys;
    public IEnumerable<TreeMap> Values => trees!.Values;
    public int Count => trees!.Count;
    public bool ContainsKey(Guid key) => trees!.ContainsKey(key);
    public IEnumerator<KeyValuePair<Guid, TreeMap>> GetEnumerator() => trees!.GetEnumerator();
    public bool TryGetValue(Guid key, [MaybeNullWhen(false)] out TreeMap value) => trees!.TryGetValue(key, out value);
    IEnumerator IEnumerable.GetEnumerator() => trees!.GetEnumerator();
    #endregion
}
