//This acts as the base class for all event data.

public class EventData {
    public readonly EventType eventType;

    public EventData(EventType eventType) {
        this.eventType = eventType;
    }
}

public class DebugEventData : EventData {
    public int debug;

    public DebugEventData(int debug, EventType eventType) : base(eventType) {
        this.debug = debug;
    }
}

public class PhaseUIEventData : EventData {
    public readonly string phaseName;
    
    public PhaseUIEventData(EventType eventType, string phaseName) : base(eventType) {
        this.phaseName = phaseName;
    }
}

public class PhaseTimeData : EventData {
    public readonly float time;

    public PhaseTimeData(EventType eventType, float time) : base(eventType) {
        this.time = time;
    }
}