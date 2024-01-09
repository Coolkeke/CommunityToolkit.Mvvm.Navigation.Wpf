using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text; 

namespace CommunityToolkit.Mvvm.Navigation.Wpf
{
    public class NavigationParameters : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly List<KeyValuePair<string, object>> _entries = new List<KeyValuePair<string, object>>(); 
        public int Count => _entries.Count; 
        public IEnumerable<string> Keys => _entries.Select((KeyValuePair<string, object> x) => x.Key); 
        public object this[string key]
        {
            get
            {
                foreach (KeyValuePair<string, object> entry in _entries)
                {
                    if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
                    {
                        return entry.Value;
                    }
                }

                return null;
            }
        }
         
        public NavigationParameters()
        {
        } 
        public NavigationParameters(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }

            int length = query.Length;
            for (int i = ((query.Length > 0 && query[0] == '?') ? 1 : 0); i < length; i++)
            {
                int num = i;
                int num2 = -1;
                for (; i < length; i++)
                {
                    switch (query[i])
                    {
                        case '=':
                            if (num2 < 0)
                            {
                                num2 = i;
                            }

                            continue;
                        default:
                            continue;
                        case '&':
                            break;
                    }

                    break;
                }

                string text = null;
                string text2 = null;
                if (num2 >= 0)
                {
                    text = query.Substring(num, num2 - num);
                    text2 = query.Substring(num2 + 1, i - num2 - 1);
                }
                else
                {
                    text2 = query.Substring(num, i - num);
                }

                if (text != null)
                {
                    Add(Uri.UnescapeDataString(text), Uri.UnescapeDataString(text2));
                }
            }
        } 
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        } 
        public void Add(string key, object value)
        {
            _entries.Add(new KeyValuePair<string, object>(key, value));
        } 
        public bool ContainsKey(string key)
        {
            foreach (KeyValuePair<string, object> entry in _entries)
            {
                if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
                {
                    return true;
                }
            }

            return false;
        } 
        public T GetValue<T>(string key)
        {
            foreach (KeyValuePair<string, object> entry in _entries)
            {
                if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
                {
                    if (entry.Value == null)
                    {
                        return default(T);
                    }

                    if (entry.Value.GetType() == typeof(T))
                    {
                        return (T)entry.Value;
                    }

                    if (typeof(T).GetTypeInfo().IsAssignableFrom(entry.Value.GetType().GetTypeInfo()))
                    {
                        return (T)entry.Value;
                    }

                    return (T)Convert.ChangeType(entry.Value, typeof(T));
                }
            }

            return default(T);
        } 
        public bool TryGetValue<T>(string key, out T value)
        {
            foreach (KeyValuePair<string, object> entry in _entries)
            {
                if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
                {
                    if (entry.Value == null)
                    {
                        value = default(T);
                    }
                    else if (entry.Value.GetType() == typeof(T))
                    {
                        value = (T)entry.Value;
                    }
                    else if (typeof(T).GetTypeInfo().IsAssignableFrom(entry.Value.GetType().GetTypeInfo()))
                    {
                        value = (T)entry.Value;
                    }
                    else
                    {
                        value = (T)Convert.ChangeType(entry.Value, typeof(T));
                    }

                    return true;
                }
            }

            value = default(T);
            return false;
        } 
        public IEnumerable<T> GetValues<T>(string key)
        {
            List<T> list = new List<T>();
            foreach (KeyValuePair<string, object> entry in _entries)
            {
                if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
                {
                    if (entry.Value == null)
                    {
                        list.Add(default(T));
                    }
                    else if (entry.Value.GetType() == typeof(T))
                    {
                        list.Add((T)entry.Value);
                    }
                    else if (typeof(T).GetTypeInfo().IsAssignableFrom(entry.Value.GetType().GetTypeInfo()))
                    {
                        list.Add((T)entry.Value);
                    }
                    else
                    {
                        list.Add((T)Convert.ChangeType(entry.Value, typeof(T)));
                    }
                }
            }

            return list.AsEnumerable();
        }
         
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (_entries.Count > 0)
            {
                stringBuilder.Append('?');
                bool flag = true;
                foreach (KeyValuePair<string, object> entry in _entries)
                {
                    if (!flag)
                    {
                        stringBuilder.Append('&');
                    }
                    else
                    {
                        flag = false;
                    }

                    stringBuilder.Append(Uri.EscapeDataString(entry.Key));
                    stringBuilder.Append('=');
                    stringBuilder.Append(Uri.EscapeDataString((entry.Value != null) ? entry.Value.ToString() : ""));
                }
            }

            return stringBuilder.ToString();
        }
    }
}
