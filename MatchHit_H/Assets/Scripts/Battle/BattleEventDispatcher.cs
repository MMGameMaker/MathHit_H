using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleEventDispatcher : MonoBehaviour
{
    #region Singleton
    public static BattleEventDispatcher Instance
    {
        get;
        private set;
    }


    private void Awake()
    {
        if(Instance!=null&& Instance != this)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }
    #endregion


    #region Field
    //Store all listenner
    Dictionary<EventID.EvenID, Action<object>> _listeners = new Dictionary<EventID.EvenID, Action<object>>();
    #endregion

    #region Add Listener, Post Event, Remove Listenner
    /// <summary>
    /// Register to listen for EventID
    /// </summary>
    public void RegisterListener(EventID.EvenID eventID, Action<object> callback)
    {
        //Check if listener exist in dictionary
        if (_listeners.ContainsKey(eventID))
        {
            _listeners[eventID] += callback;
        }
        else
        {
            //add new key-value pair
            _listeners.Add(eventID, null);
            _listeners[eventID] += callback;
        }
    }

    /// <summary>
    /// Posts the event. This will notify all listener that register for this event
    /// </summary>
    public void PostEvent(EventID.EvenID eventID, object param = null)
    {
        if (!_listeners.ContainsKey(eventID))
        {
            Debug.Log("No listeners for this event : " + eventID);
            return;
        }

        // posting event
        var callback = _listeners[eventID];
        // if there's no listener remain, then do nothing
        if (callback != null)
        {
            callback(param);
        }
        else
        {
            Debug.Log("PostEvent " + eventID + ", but no listener remain, Remove this key");
            _listeners.Remove(eventID);
        }
    }

    /// <summary>
    /// Removes the listener. Use to Unregister listener
    /// </summary>
    /// 
    public void RemoveListener(EventID.EvenID eventID, Action<object> callback)
    {
        // checking params
        if (_listeners.ContainsKey(eventID))
        {
            _listeners[eventID] -= callback;
        }
        else
        {
            Debug.Log("RemoveListener, not found key: " + eventID);
        }
    }

    /// <summary>
    /// Clear all listener
    /// </summary>
    /// 
    public void ClearAllListener()
    {
        _listeners.Clear();
    }

    #endregion

}
