using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteMapper.Services.Web.StateManagement
{
    public class StateContainer : IStateContainer
    {
        private readonly ConcurrentDictionary<string, object?> _tempData = new();

        public T? GetTempData<T>(string key)
        {
            if (!_tempData.Remove(key, out object? value))
            {
                return default;
            }

            if (value is T typedValue)
            {
                return typedValue;
            }

            return default;
        }

        public string SetTempData<T>(T obj)
        {
            string key;
            do
            {
                key = Guid.NewGuid().ToString();
            } while (!_tempData.TryAdd(key, obj));

            return key;
        }

        private string GetTempDataKey<T>(string key)
        {
            return $"TEMP.{key}.{typeof(T).FullName}";
        }
    }
}
