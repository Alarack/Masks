using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using LL.Events;
using Debug = UnityEngine.Debug;
using GameEvent = Constants.GameEvent;

public class EventManager {

    /// <summary> This is our main list of of all subscribed listeners grouped by GameEvent type </summary>
    private readonly Dictionary<GameEvent, List<EventListener>> _listeners = new Dictionary<GameEvent, List<EventListener>>();
    private EventData _defaultData;


    public bool __DoIHaveListeners(object currentListener) {
        foreach (KeyValuePair<GameEvent, List<EventListener>> listeners in _listeners)
            for (int i = 0; i < listeners.Value.Count; i++)
                if (listeners.Value[i].target == currentListener || listeners.Value[i].callback.Target == currentListener)
                    return true;

        return false;
    }

    public void Initialize() {
        _defaultData = new EventData();

#if UNITY_EDITOR
        _defaultData.__frameless = true;
#endif
    }

    public void GameplaySceneInitialize() {
#if UNITY_EDITOR
        foreach (KeyValuePair<GameEvent, List<EventListener>> eventListeners in _listeners) {
            foreach (EventListener eventListener in eventListeners.Value) {
                MonoBehaviour behaviour = eventListener.callback.Target as MonoBehaviour;
                if (behaviour == null && behaviour as object != null) // Despite what compiler thinks, Unity fake-nulls its Object`s, so this is a valid condition
                {
                    Debug.LogError("[EVENT MANAGER] Startup check: A listener for event " + eventListeners.Key +
                        " has a null MonoBehaviour (" + eventListener.listenerClassName + ") instance (but which is still in memory)" +
                        "; did you destroy the script without unregistering its event listeners?");
                    continue;
                }
            }
        }
#endif
    }


    /// <summary>
    /// This will invoke all previously registered listeners of the specified GameEvent and pass them the given EventData container.
    /// Make sure to fill event data with what is expected of the event.
    /// </summary>
    public void SendEvent(GameEvent type, EventData eventData = null) {
        if (type == GameEvent.None) {
#if UNITY_EDITOR
            Debug.LogError("[EVENT MANAGER] Something called SendEvent(" + type + ") with " + type + " GameEvent type, we can't send " + GameEvent.None + " events");
            Debug.Log("Sending");
#endif
            return;
        }
        // Use default pre-created empty data container if the sender doesn't need one -- slight optimization
        if (eventData == null)
            eventData = _defaultData;

#if UNITY_EDITOR
        eventData.__delivered = true;
#endif
        List<EventListener> listenerList;
        if (_listeners.TryGetValue(type, out listenerList)) {
            List<EventListener> eventListeners = new List<EventListener>(listenerList); //-- CREATES GARBAGE

            //Profiler.BeginSample("EventManager.SendEvent(" + type + ") to " + eventListeners.Count + " listeners");

            for (int i = 0; i < eventListeners.Count; i++) {
#if UNITY_EDITOR || DEBUG

                // Check that the target instance is not null as per Unity's nulling - it is Destroyed, reports == null, but not garbage collected
                MonoBehaviour behaviour = eventListeners[i].callback.Target as MonoBehaviour;
                if (behaviour == null && behaviour as object != null) // Despite what compiler thinks, Unity fake-nulls its Object`s, so this is a valid condition
                {
                    Debug.LogError("[EVENT MANAGER] A listener for event " + type + " has a null MonoBehaviour (" + eventListeners[i].listenerClassName + ") instance" +
                                    " (but which is still in memory); did you destroy the script without unregistering its event listeners?");
                    continue;
                }
#endif
                //Debug.Log("Sending event " + type + " with data " + eventData + " to " + d.GetInvocationList().Count() + " listeners");
                eventData.eventType = type; // TODO: Spaghetti

#if UNITY_EDITOR || DEBUG // Exceptions not allowed in release
                try {
#endif
                    eventListeners[i].callback.Invoke(eventData);
#if UNITY_EDITOR || DEBUG
                }
                catch (Exception exception) { Debug.LogError("[EVENT MANAGER] An event " + type + " listener for " + eventListeners[i].listenerClassName + " -- " + eventListeners[i].target + " threw an exception during its callback Invoke()"); Debug.LogException(exception is TargetInvocationException ? exception.InnerException : exception); }
#endif
            }

#if UNITY_EDITOR
            __sentEventEntries.Add(new __EventEntry(type, eventData, listenerList));
            if (__sentEventEntries.Count > 500)
                __sentEventEntries.RemoveAt(0);
#endif
        }
        //else
        //{
        //    Debug.Log("Event " + type + " with data " + eventData + " had no listeners");
        //}
    }

    /// <summary>
    /// This will register/subscribe the given callback function to the specified GameEvent.
    /// The callback will be called/invoked whenever something calls SendEvent() for that event.
    /// If you don't specify methods directly, you need to explicitly specify target object for later unregistering.
    /// Note that there is no priority queue, so listeners will be invoked in a first come, first serve manner.
    /// Note also that destroying or disabling GameObjects does not remove their listeners.
    /// </summary>
    public void RegisterListener(GameEvent type, Action<EventData> listener, object target = null) {
        if (type == GameEvent.None)
            return;

#if UNITY_EDITOR
        //Debug.Log("Registering a Listener");
        // Check (editor-time) that the listener is not an anonumous delegate -- we wouldn't know who it belongs to unless told
        if (listener.Target.GetType().Name.Contains("AnonStorey")) // This is probably true only for Mono
        {
            //Debug.Log ("Anonymous target" + listener.Target.GetType().Name);
            if (target == null)
                Debug.LogError("[EVENT MANAGER] RegisterListener() was called on an anonymous (compiler-generated) method; we need to know the responsible class instance to properly remove the listener. Set the target parameter to your class in the method.");
        }

        // Check for duplicates, same event type for same method in same class insatnce
        if (_listeners.ContainsKey(type)) {
            foreach (EventListener checkListener in _listeners[type]) {
                if (checkListener.target == listener.Target && checkListener.callback == listener) {
                    Debug.LogError("[EVENT MANAGER] We were asked to register the same listener for event " + type + " to " + listener + " in " + target + " " +
                                   "; the event will be invoked more than once");
                    break;
                }
            }
        }
#endif

        //Debug.Log ("Registering for event " + type);
        List<EventListener> eventListener;
        if (_listeners.TryGetValue(type, out eventListener)) {
            eventListener.Add(
                new EventListener(listener, listener.Target.GetType().Name, target)
            );
        }
        else {
            _listeners[type] = new List<EventListener>()
            {
                new EventListener(listener, listener.Target.GetType().Name, target)
            };
        }
    }

    /// <summary>
    /// This will remove the given callback function from the invocation list for the specified GameEvent.
    /// Use this when you no longer need this event or are destroying the class instance linked with the delegate.
    /// </summary>
    public void RemoveListener(GameEvent type, Action<EventData> listener) {
        if (type == GameEvent.None)
            return;

        if (_listeners.ContainsKey(type)) {
            for (int i = 0; i < _listeners[type].Count; i++) {
                if (_listeners[type][i].callback.Target == listener.Target) {
                    _listeners[type].RemoveAt(i);
                    i--;
                }
            }
        }
    }

    /// <summary>
    /// Remove all listenered for any events for the given class instance
    /// </summary>
    public void RemoveMyListeners(object currentListener) {

        foreach (KeyValuePair<GameEvent, List<EventListener>> listeners in _listeners) {
            for (int i = 0; i < listeners.Value.Count; i++) {
                if (listeners.Value[i].target == currentListener || listeners.Value[i].callback.Target == currentListener) {
                    listeners.Value.RemoveAt(i);
                    i--;
                }
            }
        }
    }


    public class EventListener {
        public readonly Action<EventData> callback;
        public readonly string listenerClassName;
        /// <summary> Optionally specified actual target class that recives the callback in case we created it anonymously </summary>
        public readonly object target;

        public EventListener(Action<EventData> callback, string listenerClassName, object target) {
            this.callback = callback;
            this.listenerClassName = listenerClassName;
            this.target = target;
        }
    }


#if UNITY_EDITOR
    public List<__EventEntry> __sentEventEntries = new List<__EventEntry>();
    public Dictionary<GameEvent, List<EventListener>> __GetListenerList() { return _listeners; }

    public class __EventEntry {
        public readonly GameEvent type;
        public readonly EventData eventData;
        public readonly List<EventListener> listenerList;
        public readonly float time;
        public readonly int frame;

        public __EventEntry(GameEvent type, EventData eventData, List<EventListener> listenerList) {
            this.type = type;
            this.eventData = eventData;
            this.listenerList = listenerList;
            time = Time.realtimeSinceStartup;
            frame = Time.frameCount;
        }
    }

#endif

}
