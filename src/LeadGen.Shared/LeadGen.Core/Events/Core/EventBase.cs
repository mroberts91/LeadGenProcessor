using System.Reflection;
using LeadGen.Core.Attributes;

namespace LeadGen.Core.Events.Core
{
    public abstract class EventBase
    {
        private readonly EventSubscription? _eventSubscription;

        public EventBase()
        {
            string? pubSubName = GetType().GetCustomAttribute<PubSubAttribute>()?.Name;

            if (pubSubName is string)
                _eventSubscription = new(pubSubName, GetType().Name);
        }

        public EventSubscription? Subscription => _eventSubscription;

        public object AsObject() => this;
    }
}
