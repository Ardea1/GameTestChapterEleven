// Messenger.cs v1.0 by Magnus Wolffelt, magnus.wolffelt@gmail.com
//
// Inspired by and based on Rod Hyde's Messenger:
// http://www.unifycommunity.com/wiki/index.php?title=CSharpMessenger
// https://github.com/hybrid1969/Unity-Scripts/tree/master/CSharpMessenger%20Extended
//
// This is a C# messenger (notification center). It uses delegates
// and generics to provide type-checked messaging between event producers and
// event consumers, without the need for producers or consumers to be aware of
// each other. The major improvement from Hyde's implementation is that
// there is more extensive error detection, preventing silent bugs.
//
// Usage example:
// Messenger<float>.AddListener("myEvent", MyEventHandler);
// ...
// Messenger<float>.Broadcast("myEvent", 1.0f);


using System;
using System.Collections.Generic;
 
public enum MessengerMode
{
	DONT_REQUIRE_LISTENER,
	REQUIRE_LISTENER,
}


static internal class MessengerInternal
{
	static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
	static public readonly MessengerMode DEFAULT_MODE = MessengerMode.REQUIRE_LISTENER;

	static public void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
	{
		if (!eventTable.ContainsKey(eventType))
		{
			eventTable.Add(eventType, null);
		}

		Delegate d = eventTable[eventType];
		if (d != null && d.GetType() != listenerBeingAdded.GetType())
		{
			throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
		}
	}

	static public void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
	{
		if (eventTable.ContainsKey(eventType))
		{
			Delegate d = eventTable[eventType];

			if (d == null)
			{
				throw new ListenerException(string.Format("Attempting to remove listener with for event type {0} but current listener is null.", eventType));
			}
			else if (d.GetType() != listenerBeingRemoved.GetType())
			{
				throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
			}
		}
		else
		{
			throw new ListenerException(string.Format("Attempting to remove listener for type {0} but Messenger doesn't know about this event type.", eventType));
		}
	}

	static public void OnListenerRemoved(string eventType)
	{
		if (eventTable[eventType] == null)
		{
			eventTable.Remove(eventType);
		}
	}

	static public void OnBroadcasting(string eventType, MessengerMode mode)
	{
		if (mode == MessengerMode.REQUIRE_LISTENER && !eventTable.ContainsKey(eventType))
		{
			throw new MessengerInternal.BroadcastException(string.Format("Broadcasting message {0} but no listener found.", eventType));
		}
	}

	static public BroadcastException CreateBroadcastSignatureException(string eventType)
	{
		return new BroadcastException(string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", eventType));
	}

	public class BroadcastException : Exception
	{
		public BroadcastException(string msg)
			: base(msg)
		{
		}
	}

	public class ListenerException : Exception
	{
		public ListenerException(string msg)
			: base(msg)
		{
		}
	}
}


// No parameters
static public class Messenger
{
	private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

	static public void AddListener(string eventType, Callback handler)
	{
		MessengerInternal.OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback)eventTable[eventType] + handler;
	}

	static public void RemoveListener(string eventType, Callback handler)
	{
		MessengerInternal.OnListenerRemoving(eventType, handler);
		eventTable[eventType] = (Callback)eventTable[eventType] - handler;
		MessengerInternal.OnListenerRemoved(eventType);
	}

	static public void Broadcast(string eventType)
	{
		Broadcast(eventType, MessengerInternal.DEFAULT_MODE);
	}

	static public void Broadcast(string eventType, MessengerMode mode)
	{
		MessengerInternal.OnBroadcasting(eventType, mode);
		Delegate d;
		if (eventTable.TryGetValue(eventType, out d))
		{
			Callback callback = d as Callback;
			if (callback != null)
			{
				callback();
			}
			else
			{
				throw MessengerInternal.CreateBroadcastSignatureException(eventType);
			}
		}
	}
}

// One parameter
static public class Messenger<T>
{
	private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

	static public void AddListener(string eventType, Callback<T> handler)
	{
		MessengerInternal.OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
	}

	static public void RemoveListener(string eventType, Callback<T> handler)
	{
		MessengerInternal.OnListenerRemoving(eventType, handler);
		eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;
		MessengerInternal.OnListenerRemoved(eventType);
	}

	static public void Broadcast(string eventType, T arg1)
	{
		Broadcast(eventType, arg1, MessengerInternal.DEFAULT_MODE);
	}

	static public void Broadcast(string eventType, T arg1, MessengerMode mode)
	{
		MessengerInternal.OnBroadcasting(eventType, mode);
		Delegate d;
		if (eventTable.TryGetValue(eventType, out d))
		{
			Callback<T> callback = d as Callback<T>;
			if (callback != null)
			{
				callback(arg1);
			}
			else
			{
				throw MessengerInternal.CreateBroadcastSignatureException(eventType);
			}
		}
	}
}


// Two parameters
static public class Messenger<T, U>
{
	private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

	static public void AddListener(string eventType, Callback<T, U> handler)
	{
		MessengerInternal.OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback<T, U>)eventTable[eventType] + handler;
	}

	static public void RemoveListener(string eventType, Callback<T, U> handler)
	{
		MessengerInternal.OnListenerRemoving(eventType, handler);
		eventTable[eventType] = (Callback<T, U>)eventTable[eventType] - handler;
		MessengerInternal.OnListenerRemoved(eventType);
	}

	static public void Broadcast(string eventType, T arg1, U arg2)
	{
		Broadcast(eventType, arg1, arg2, MessengerInternal.DEFAULT_MODE);
	}

	static public void Broadcast(string eventType, T arg1, U arg2, MessengerMode mode)
	{
		MessengerInternal.OnBroadcasting(eventType, mode);
		Delegate d;
		if (eventTable.TryGetValue(eventType, out d))
		{
			Callback<T, U> callback = d as Callback<T, U>;
			if (callback != null)
			{
				callback(arg1, arg2);
			}
			else
			{
				throw MessengerInternal.CreateBroadcastSignatureException(eventType);
			}
		}
	}
}


// Three parameters
static public class Messenger<T, U, V>
{
	private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

	static public void AddListener(string eventType, Callback<T, U, V> handler)
	{
		MessengerInternal.OnListenerAdding(eventType, handler);
		eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] + handler;
	}

	static public void RemoveListener(string eventType, Callback<T, U, V> handler)
	{
		MessengerInternal.OnListenerRemoving(eventType, handler);
		eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] - handler;
		MessengerInternal.OnListenerRemoved(eventType);
	}

	static public void Broadcast(string eventType, T arg1, U arg2, V arg3)
	{
		Broadcast(eventType, arg1, arg2, arg3, MessengerInternal.DEFAULT_MODE);
	}

	static public void Broadcast(string eventType, T arg1, U arg2, V arg3, MessengerMode mode)
	{
		MessengerInternal.OnBroadcasting(eventType, mode);
		Delegate d;
		if (eventTable.TryGetValue(eventType, out d))
		{
			Callback<T, U, V> callback = d as Callback<T, U, V>;
			if (callback != null)
			{
				callback(arg1, arg2, arg3);
			}
			else
			{
				throw MessengerInternal.CreateBroadcastSignatureException(eventType);
			}
		}
	}
}







//Usage

//The main difference in usage from Hyde's implementation, is that with this messenger you can not have several events with same name,
//but different parameter signature. So for example, if you register a listener for "myEvent" with no parameters, and later try to register
//an event listener for "myEvent" that takes a float parameter, an exception will be thrown. Furthermore, there's an optional MessengerMode
//that allows for requiring at least one listener to exist when broadcasting an event. Generally, this Messenger will throw a lot of exceptions
//as soon as the programmer makes mistakes. Not all potential errors can be covered, but it tries to be strict in order to prevent silent,
//undetected bugs. 

// Writing an event listener 


//    void OnSpeedChanged(float speed)
//    {
//	this.speed = speed;
//}

//Registering an event listener 


//    void OnEnable()
//    {
//	Messenger<float>.AddListener("speed changed", OnSpeedChanged);
//}

//Unregistering an event listener 


//    void OnDisable()
//    {
//	Messenger<float>.RemoveListener("speed changed", OnSpeedChanged);
//}

//Warning

//RemoveListener should always be called on messages when loading a new level.Otherwise many MissingReferenceExceptions will be thrown,
//when invoking messages on destroyed objects. For example: 1.We registered a "speed changed" message in a Level1 scene. Afterwards the
//scene has been destroyed, but the "speed changed" message is still pointing to the OnSpeedChanged message handler in the destroyed class.
//2.We loaded Level2 and registered another "speed changed" message, but the previous reference to the destroyed object hasn't been removed.
//We'll get a MissingReferenceException, because by invoking the "speed changed" message, the messaging system will first invoke the OnSpeedChanged
//handler of the destroyed object. 


// Broadcasting an event 


//    if (speed != lastSpeed)
//    {
//	Messenger<float>.Broadcast("speed changed", speed);