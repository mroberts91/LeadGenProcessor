using LeadGen.Core.Events.Core;
using LeadGen.Reporting.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace LeadGen.Reporting.Services
{
    public class EventCacheManager : IEventCache
    {
        private readonly IMemoryCache _cache;
        private readonly Subject<bool> _cacheUpdatedSubject = new();
        private readonly Subject<bool> _statsUpdatedSubject = new();


        public EventCacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddEventToCache<TEvent>(TEvent evt) where TEvent : EventBase
        {
            string key = CahceKey<TEvent>();

            List<TEvent> eventList = _cache.TryGetValue<List<TEvent>>(key, out var values) ? values : new List<TEvent>();
            eventList.Add(evt);
            _cache.Set(key, eventList);
            _cacheUpdatedSubject.OnNext(true);
        }

        public IEnumerable<TEvent>? GetCachedEvents<TEvent>() where TEvent : EventBase
        {
            string key = CahceKey<TEvent>();
            return _cache.TryGetValue<List<TEvent>>(key, out var values) ? values : null;
        }

        public ReportingStats? GetReportingStats()
        {
            string key = CahceKey<ReportingStats>();
            return _cache.TryGetValue<ReportingStats>(key, out var value) ? value : null;
        }

        public void UpdateReportingStats(ReportingStats? stats)
        {
            if (stats is ReportingStats s)
            {
                string key = CahceKey<ReportingStats>();
                _cache.Set(key, s);
                _statsUpdatedSubject.OnNext(true);
            }
        }

        public IDisposable OnCacheUpdated(Action<bool> onNext) => _cacheUpdatedSubject.Subscribe(onNext);

        public IDisposable OnStatsUpdate(Action<bool> onNext) => _statsUpdatedSubject.Subscribe(onNext);

        private static string CahceKey<T>() => typeof(T).Name;
    }
}
