using System;
using System.Collections.Generic;

public class EventQueue {
    public delegate void EventQueueHandler(EventData eventData);

    private readonly Dictionary<EventType, EventQueueHandler> _subscriberDictionary 
        = new Dictionary<EventType, EventQueueHandler>();
    private static EventQueue _eventQueue;
    private readonly List<EventData> _eventDataList = new List<EventData>();

    //Don't allow constructor calls.
    private EventQueue() {
    }

    //Use Singleton here.
    public static EventQueue GetEventQueue() {
        return _eventQueue ??= new EventQueue();
    }

    public void Subscribe(EventType eventType, EventQueueHandler eventQueueHandler) {
        if (!_subscriberDictionary.ContainsKey(eventType)) {
            _subscriberDictionary.Add(eventType, null);
        }
        
        _subscriberDictionary[eventType] += eventQueueHandler;
    }

    public void Unsubscribe(EventType eventType, EventQueueHandler eventQueueHandler) {
        if (_subscriberDictionary.ContainsKey(eventType)) {
            _subscriberDictionary[eventType] -= eventQueueHandler;
        }
        else {
            throw new ArgumentException("Event type for unsubscribe does not exist in the event queue.");
        }
    }

    public void AddEvent(EventData eventData) {
        if (!Enum.IsDefined(typeof(EventType), eventData.eventType)) {
            throw new ArgumentException("Could not add event: EventType does not exist.");
        }
        
        _eventDataList.Add(eventData);
    }

    public void RaiseEvents() {
        foreach (var eventData in _eventDataList) {
            if (_subscriberDictionary.ContainsKey(eventData.eventType)) {
                _subscriberDictionary[eventData.eventType]?.Invoke(eventData);
            }
        }

        _eventDataList.Clear();
    }
}

public enum EventType {
    Debug,
    //Preparation
    InFadeToPreparation,
    InitPreparationPhase,
    PreparationPhase,
    PreparationPhaseOver,
    
    //Attack
    InFadeToAttack,
    AttackPhase,
    AttackPhaseOver,
    
    //Destruction
    InFadeToDestruction,
    DestructionPhase,
    DestructionPhaseOver,
    
    //Time related
    PrepPhaseTimeUpdate,
    AttackPhaseTimeUpdate,
    DestructionPhaseTimeUpdate,
    
    
    PlayerPreparationReady,
    PlayerDestructionReady
}