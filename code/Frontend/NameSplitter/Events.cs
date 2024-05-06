using NameSplitter.DTOs;
using Prism.Events;

namespace NameSplitter.Events
{
    public class OpenParsedElementsView: PubSubEvent<ParseResponseDto>
    { }

    public class ParseEvent: PubSubEvent
    { }

    public class SaveParsedElements: PubSubEvent
    { }

    public class UpdateParsedList: PubSubEvent<StructuredName>
    { }
}