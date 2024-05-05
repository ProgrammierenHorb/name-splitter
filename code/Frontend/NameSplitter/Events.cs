using NameSplitter.DTOs;
using Prism.Events;

namespace NameSplitter.Events
{
    public class ParseEvent: PubSubEvent
    { }

    public class UpdateParsedList: PubSubEvent<StructuredName>
    { }
}