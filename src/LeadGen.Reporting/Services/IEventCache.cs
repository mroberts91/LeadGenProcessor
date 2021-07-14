using LeadGen.Core.Events.Core;
using LeadGen.Reporting.Models;
using System;
using System.Collections.Generic;

namespace LeadGen.Reporting.Services
{
    public interface IEventCache
    {
        void AddEventToCache<TEvent>(TEvent evt) where TEvent : EventBase;
        IEnumerable<TEvent>? GetCachedEvents<TEvent>() where TEvent : EventBase;
        ReportingStats? GetReportingStats();
        IDisposable OnCacheUpdated(Action<bool> onNext);
        IDisposable OnStatsUpdate(Action<bool> onNext);
        void UpdateReportingStats(ReportingStats? stats);
    }
}