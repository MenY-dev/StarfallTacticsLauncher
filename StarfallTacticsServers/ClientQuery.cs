using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace StarfallTactics.StarfallTacticsServers
{
    public class ClientQuery : ICollection<ClientQuery.Parameter>
    {
        public string Function { get; protected set; }

        public int Count => Parameters.Count;

        public bool IsReadOnly => true;

        public Parameter this[string i]
        {
            get
            {
                foreach (var item in Parameters)
                    if (item.Key == i)
                        return item;

                return null;
            }
        }

        protected List<Parameter> Parameters { get; } = new List<Parameter>();

        public static ClientQuery Parse(string httpQuery)
        {
            if (httpQuery is null)
                return new ClientQuery();

            return Parse(HttpUtility.ParseQueryString(httpQuery));
        }

        public static ClientQuery Parse(NameValueCollection httpQuery)
        {
            ClientQuery query = new ClientQuery();

            if (httpQuery is null)
                return query;

            foreach (var key in httpQuery.Keys)
            {
                if (string.IsNullOrWhiteSpace(key as string))
                    continue;

                if (key is string stringKey && string.IsNullOrWhiteSpace(stringKey) == false)
                {
                    string value = httpQuery[stringKey];

                    if (string.IsNullOrWhiteSpace(value))
                        continue;

                    if (stringKey == "func" && query.Function is null)
                        query.Function = value;

                    query.Parameters.Add(new Parameter(stringKey, value));
                }
            }

            return query;
        }

        public bool Contains(Parameter item) => Parameters.Contains(item);

        public void CopyTo(Parameter[] array, int arrayIndex) => Parameters.CopyTo(array, arrayIndex);

        public IEnumerator<Parameter> GetEnumerator() => Parameters.GetEnumerator();

        void ICollection<Parameter>.Add(Parameter item) { }

        void ICollection<Parameter>.Clear() { }

        bool ICollection<Parameter>.Remove(Parameter item) => false;

        IEnumerator IEnumerable.GetEnumerator() => Parameters.GetEnumerator();

        public class Parameter
        {
            public string Key { get; }
            public string Value { get; }

            public Parameter(string key, string value)
            {
                Key = key;
                Value = value;
            }
            
            public static explicit operator bool?(Parameter parameter)
            {
                string value = parameter?.Value;

                if (value is null)
                    return null;

                if (string.IsNullOrWhiteSpace(value) ||
                    (long)parameter <= 0 ||
                    (double)parameter <= 0)
                    return false;

                return true;
            }

            public static explicit operator int?(Parameter parameter)
            {
                if (int.TryParse(parameter?.Value, out int value))
                    return value;

                return null;
            }

            public static explicit operator long?(Parameter parameter)
            {
                if (long.TryParse(parameter?.Value, out long value))
                    return value;

                return null;
            }

            public static explicit operator float?(Parameter parameter)
            {
                if (float.TryParse(parameter?.Value, out float value))
                    return value;

                return null;
            }

            public static explicit operator double?(Parameter parameter)
            {
                if (double.TryParse(parameter?.Value, out double value))
                    return value;

                return null;
            }

            public static explicit operator string(Parameter parameter)
            {
                return parameter?.Value;
            }

            public override string ToString()
            {
                return Value ?? string.Empty;
            }
        }
    }
}
