using System;
using System.Collections.Generic;
using UniRx;
using Nodux.Core;

namespace Nodux.PluginState
{
    public class State
    {
        private IDictionary<string, Any> Map = new Dictionary<string, Any>();
        private IDictionary<string, ISubject<Any>> SubjectMap = new Dictionary<string, ISubject<Any>>();

        public State Set(string key, Any value)
        {
            this.Map[key] = value;
            this.Notify(key, value);
            return this;
        }

        public State SetValue<TValue>(string key, TValue value)
        {
            this.Set(key, new Any(value));
            return this;
        }

        public Any Get(string key)
        {
            return this.Map.ContainsKey(key) ? this.Map[key] : default(Any);
        }

        public TValue GetValue<TValue>(string key)
        {
            return this.Get(key).Value<TValue>();
        }

        public IObservable<Any> GetObservable(string key)
        {
            if (!this.SubjectMap.ContainsKey(key)) this.SubjectMap[key] = new Subject<Any>();
            return this.SubjectMap[key];
        }

        public void Notify(string key, Any value)
        {
            if (!this.SubjectMap.ContainsKey(key)) this.SubjectMap[key] = new Subject<Any>();
            this.SubjectMap[key].OnNext(value);
        }

        public void NotifyValue(string key)
        {
            this.Notify(key, this.Get(key));
        }

        public IDictionary<string, Any> GetRawMap()
        {
            return this.Map;
        }

        public void Replace(IDictionary<string, Any> dictionary)
        {
            this.Map = dictionary;
        }
    }
}