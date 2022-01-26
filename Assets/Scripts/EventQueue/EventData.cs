//This acts as the base class for all event data.

public class EventData {
    public readonly EventType eventType;

    public EventData(EventType eventType) {
        this.eventType = eventType;
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
    public readonly string playerName;

    public PhaseTimeData(EventType eventType, float time, string playerName) : base(eventType) {
        this.time = time;
        this.playerName = playerName;
    }
}

public class PlayerReadyEventData : EventData {
    public readonly string playerName;

    public PlayerReadyEventData(EventType eventType, string playerName) : base(eventType) {
        this.playerName = playerName;
    }
}

public class MessageEventData : EventData {
    public readonly string message;

    public MessageEventData(EventType eventType, string message) : base(eventType) {
        this.message = message;
    }
}