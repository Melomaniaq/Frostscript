using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Frostscript.Domain
{
    public class Closure<TKey, TValue>(IDictionary<TKey, TValue> globalVariables) : IDictionary<TKey, TValue> where TKey : notnull
    {
        readonly Dictionary<TKey, TValue> _closure = [];

        public TValue this[TKey key] 
        { 
            get => _closure.TryGetValue(key, out TValue? value) ? value : globalVariables[key];
            set {
                if (_closure.ContainsKey(key) || !globalVariables.ContainsKey(key))
                    _closure[key] = value;
                else 
                    globalVariables[key] = value;
            }
        }

        public ICollection<TKey> Keys => [.. _closure.Keys.Union(globalVariables.Keys)];

        public ICollection<TValue> Values => [.. Keys.Select(x => this[x])];

        public int Count => Keys.Count;

        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value) => _closure.Add(key, value);

        public void Add(KeyValuePair<TKey, TValue> item) => _closure.Add(item.Key, item.Value);

        public void Clear() => _closure.Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item) => _closure.Contains(item) || globalVariables.Contains(item);

        public bool ContainsKey(TKey key) => _closure.ContainsKey(key) || globalVariables.ContainsKey(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _closure.GetEnumerator();

        public bool Remove(TKey key) => _closure.Remove(key);

        public bool Remove(KeyValuePair<TKey, TValue> item) => _closure.Remove(item.Key);

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => 
            _closure.TryGetValue(key, out value) || globalVariables.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
