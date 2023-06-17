using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : Singleton<EventsManager>
{
    static List<Event> eventQueue = new();

    private void Start()
    {

    }

    public void AddEventToQueue()
    {
        eventQueue.Add(new Event());
    }

    public void CheckIfAnyEventsToInvoke()
    {
        if(eventQueue.Count != 0)
        {
            InvokeEventQueue();
        }
    }

    void InvokeEventQueue()
    {
        if (eventQueue.Count != 0)
        {
            foreach (Event e in eventQueue)
            {
                e.ChooseEvent();
            }
            eventQueue.Clear();
        }
    }
}
