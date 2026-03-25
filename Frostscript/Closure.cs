using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Frostscript
{
    internal class Closure(IDictionary<string, object> globalVariables) : IDictionary<string, object>
    {
        readonly Dictionary<string, object> _closure = [];

        public object this[string key] 
        { 
            get => _closure.TryGetValue(key, out object? value) ? value : globalVariables[key];
            set {
                if (_closure.ContainsKey(key) || !globalVariables.ContainsKey(key))
                    _closure[key] = value;
                else 
                    globalVariables[key] = value;
            }
        }

        public ICollection<string> Keys => [.. _closure.Keys.Union(globalVariables.Keys)];

        public ICollection<object> Values => [.. Keys.Select(x => this[x])];

        public int Count => Keys.Count;

        public bool IsReadOnly => false;

        public void Add(string key, object value) => _closure.Add(key, value);

        public void Add(KeyValuePair<string, object> item) => _closure.Add(item.Key, item.Value);

        public void Clear()
        {
            _closure.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item) => _closure.Contains(item) || globalVariables.Contains(item);

        public bool ContainsKey(string key) => _closure.ContainsKey(key) || globalVariables.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _closure.GetEnumerator();

        public bool Remove(string key) => _closure.Remove(key);

        public bool Remove(KeyValuePair<string, object> item) => _closure.Remove(item.Key);

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value) => 
            _closure.TryGetValue(key, out value) || globalVariables.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
