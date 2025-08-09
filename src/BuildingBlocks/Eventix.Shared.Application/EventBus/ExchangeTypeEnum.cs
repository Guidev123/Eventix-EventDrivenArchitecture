using System.ComponentModel;

namespace Eventix.Shared.Application.EventBus
{
    public enum ExchangeTypeEnum
    {
        [Description("direct")]
        Direct,

        [Description("fanout")]
        Fanout,

        [Description("topic")]
        Topic
    }
}