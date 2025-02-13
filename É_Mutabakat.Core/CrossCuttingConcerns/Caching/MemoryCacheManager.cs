﻿using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace E_Mutabakat.Core.CrossCuttingConcerns.Caching
{

    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return Get<object>(key);
        }
        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            var cacheExtriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntiresCollection",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var cacheExtriesCollection = cacheExtriesCollectionDefinition.GetValue(_memoryCache) as dynamic;
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();
            foreach(var cacheItem in cacheExtriesCollection)
            {
                ICacheEntry cacheItemValue=cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }
           var regex=new Regex(pattern,RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keyToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();
            foreach ( var key in keyToRemove)
            {
                _memoryCache.Remove(key);
            }
        }
    }
}
