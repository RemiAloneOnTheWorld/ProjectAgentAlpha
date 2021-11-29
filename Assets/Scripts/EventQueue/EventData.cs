using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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