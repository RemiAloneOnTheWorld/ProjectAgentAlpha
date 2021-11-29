using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    void Update()
    {
        EventQueue.GetEventQueue().RaiseEvents();
    }
}
