namespace CustomEvent
{
    public class SimpleEvent : IEventBase { }

    public class IntEvent : IEventBase { public int Value { get; set; } }

    public class TestMessageEvent : IEventBase { public string msg { get; set; } }
}