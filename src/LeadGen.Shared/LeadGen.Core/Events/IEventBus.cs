using LeadGen.Core.Events.Core;
using System.Threading.Tasks;

namespace LeadGen.Core.Events
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T eventData) where T : EventBase;
    }
}
