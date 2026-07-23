using Godot;
using System;
using SharpHook;
using GlobalInputAddon.Data;
using System.Collections.Generic;
using System.Diagnostics;

public partial class GlobalInput : Node
{
	// TODO: FIX WHEEL DETECTION; mouse positions; and other godot Input features
	[Signal] public delegate void GlobalInputEventEventHandler(InputEvent inputEvent);
	[Signal] public delegate void GlobalInputEventExEventHandler(InputEventData inputEventData);
	// Static Fields
	static EventLoopGlobalHook hook; public static EventLoopGlobalHook Hook { get { return hook; } }
	static GlobalInput instance; public static GlobalInput Instance { get { return instance; } }
	static InputEventData inputEventData; public static InputEventData InputEventData { get { return GlobalInput.inputEventData; } }
	// State Fields
	// whether events within InputEventData.GodotInputEventToSharpHookEvents keys are pressed
	static Dictionary<string, bool> eventStates = []; public static Dictionary<string, bool> EventStates { get { return eventStates; } }
	// used to track state of actions and method callers
	static Dictionary<string, Dictionary<string, Dictionary<string, Variant>>> actionStates = []; public static Dictionary<string, Dictionary<string, Dictionary<string, Variant>>> ActionStates { get { return actionStates; } }
	
    #region Godot Methods
    public override void _EnterTree()
	{
		InitializeSelf();
	}
    public override void _ExitTree()
	{
		if (Instance != this) return;

		DisposeSelf();
	}

    public override void _Notification(int what)
	{
		switch ((long)what)
		{
			case NotificationWMCloseRequest:
				// DisposeSelf();
				break;
		}
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventKey)
		{
			OnInputEvent(InputEventData.EnumInputEventType.Key, inputEvent.IsPressed(), inputEvent);
		}
		if (inputEvent is InputEventMouseButton inputEventMouseButton) // need this so no memory leak
		{
			if (inputEventMouseButton.ButtonIndex == MouseButton.WheelUp || inputEventMouseButton.ButtonIndex == MouseButton.WheelDown || inputEventMouseButton.ButtonIndex == MouseButton.WheelLeft || inputEventMouseButton.ButtonIndex == MouseButton.WheelRight)
			{
				OnInputEvent(InputEventData.EnumInputEventType.MouseWheel, true, inputEvent);
			}
			else
			{
				OnInputEvent(InputEventData.EnumInputEventType.MouseButton, inputEvent.IsPressed(), inputEvent);			
			}
		}
		if (inputEvent is InputEventMouseMotion) // need this so no memory leak
		{
			OnInputEvent(InputEventData.EnumInputEventType.MouseMotion, false, inputEvent);
		}
	}

    public override void _Ready()
	{
		InitializeSignalCallbacks();
		InitializeEventStates();

		Hook.RunAsync();
	}

	#endregion

	#region Public Methods
	public static bool IsActionJustPressed(string actionName)
	{
		string uniqueCallerName = GetUniqueCallerName();
		return IsActionJustPressed(actionName, uniqueCallerName);
	}
	public static bool IsActionPressed(string actionName)
	{
		string uniqueCallerName = GetUniqueCallerName();
		return IsActionPressed(actionName, uniqueCallerName);
	}
	public static bool IsActionJustReleased(string actionName)
	{
		string uniqueCallerName = GetUniqueCallerName();
		return IsActionJustReleased(actionName, uniqueCallerName);
	}
	
	public static Vector2 GetVector(string negativeX, string positiveX, string negativeY, string positiveY)
	{
		int negX = IsActionJustPressed(negativeX) ? -1 : 0;
		int posX = IsActionJustPressed(positiveX) ? 1 : 0;
		int negY = IsActionJustPressed(negativeY) ? -1 : 0;
		int posY = IsActionJustPressed(positiveY) ? 1 : 0;
		return new Vector2(negX + posX, negY + posY);
	}
	
	public static bool IsAnythingPressed()
	{
		foreach (string eventName in EventStates.Keys)
		{
			if (EventStates[eventName]) { return true; }
		}
		return false;
	}

	public static bool IsKeyPressed(Key key)
	{
		return EventStates[key.ToString()];
	}

	public static bool IsMouseButtonPressed(MouseButton mouseButton)
	{
		return EventStates["Mouse" + mouseButton.ToString()];
	}
	#endregion

	#region Private Methods
	void InitializeSelf()
	{
		if (Hook != null || Instance != null) { GD.Print("GlobalInput is already initialized."); QueueFree(); return; }

		hook = new();
		instance = this;
		inputEventData = new();
	}
	static void DisposeSelf()
	{
		if (Hook == null || Instance == null) { GD.Print("GlobalInput is not initialized."); return; }

		hook.Dispose();
		instance.Dispose();
		instance = null;
		inputEventData.Dispose();
		GC.Collect();
	}
	static void InitializeSignalCallbacks()
	{
		Hook.HookEnabled += OnHookEnabled;
		Hook.HookDisabled += OnHookDisabled;

		Hook.KeyPressed += OnHookKeyPressed;
		Hook.KeyReleased += OnHookKeyReleased;

		Hook.MouseMoved += OnHookMouseMoved;
		Hook.MousePressed += OnHookMousePressed;
		Hook.MouseReleased += OnHookMouseReleased;
		Hook.MouseWheel += OnHookMouseWheel;
	}
	static void InitializeEventStates()
	{
		string[] keys = Enum.GetNames<Key>();
		string[] mouseButtons = Enum.GetNames<MouseButton>();

		foreach (string key in keys) { eventStates.Add(key, false); }
		foreach (string mouseButton in mouseButtons) { eventStates.Add("Mouse" + mouseButton, false); }
	}
	static void ChangeInputEventData(InputEventData.EnumInputEventType type, bool pressed, InputEvent inputEvent)
	{
		// Update InputEventData
		inputEventData.Type = type;
		inputEventData.Pressed = pressed;

		// mouse fields
		
		inputEventData.GodotInputEventNames = InputEventData.GetGodotInputEventNames(inputEvent);
		inputEventData.SharpHookEventNames = InputEventData.GodotInputEventNameToSharpHookEventNames(inputEventData.GodotInputEventNames[^1]);

		inputEventData.GodotInputEvent = inputEvent;
		inputEventData.SharpHookEvents = InputEventData.GodotInputEventToSharpHookEvents(inputEvent);
	}
	
	static void OnInputEvent(InputEventData.EnumInputEventType type, bool pressed, InputEvent inputEvent)
	{
		// update InputEventData
		ChangeInputEventData(type, pressed, inputEvent);
		

		// change event states; changes key and modifier states
		foreach (string godotInputEventName in inputEventData.GodotInputEventNames)
		{
			eventStates[godotInputEventName] = pressed;
		}
		// Instance.EmitSignal(SignalName.GlobalInputEventEx, inputEventData);
		// Instance.EmitSignal(SignalName.GlobalInputEvent, inputEventData.GodotInputEvent);
		Instance.CallDeferred("EmitGlobalInputEventSignal", inputEventData);
	}
	static void OnInputEvent(InputEventData.EnumInputEventType type, bool pressed, HookEventArgs sharpHookEvent)
	{
		OnInputEvent(type, pressed, InputEventData.SharpHookEventToGodotInputEvent(sharpHookEvent));	
	}
	
	static void UpdateActionState(string actionName)
	{
		// check if action exists
		if (!ActionStates.ContainsKey(actionName))
		{
			ActionStates[actionName] = [];
		}
		// check if overall dictionary exists
		if (!ActionStates[actionName].ContainsKey("Overall"))
		{
			ActionStates[actionName]["Overall"] = new()
			{
				{"UpdateFrame", 0}, // use to update only once per frame
				{"PrevState", false},
				{"State", false}
			};
		}
		
		// check if unique caller names dictionary exists and reset if overall state and prev state is false
		if (!ActionStates[actionName].ContainsKey("UniqueCallerNames") || !(bool)ActionStates[actionName]["Overall"]["State"] && !(bool)ActionStates[actionName]["Overall"]["PrevState"])
		{
			ActionStates[actionName]["UniqueCallerNames"] = [];
		}

		// update action update frame
		if ((int)ActionStates[actionName]["Overall"]["UpdateFrame"] == Engine.GetFramesDrawn() % int.MaxValue) 
		{ 
			return; 
		} // update only once per frame
		ActionStates[actionName]["Overall"]["UpdateFrame"] = Engine.GetFramesDrawn() % int.MaxValue;
		// reset overall state
		ActionStates[actionName]["Overall"]["PrevState"] = false;
		ActionStates[actionName]["Overall"]["State"] = false;

		// all input events in action
		foreach (InputEvent inputEvent in InputMap.ActionGetEvents(actionName))
		{
			string[] godotInputEventNames = InputEventData.GetGodotInputEventNames(inputEvent); // [Shift, A]
			string godotInputEventAsText = godotInputEventNames.Join("+"); // [Shift, A] -> Shift+A
			// check if input event exists
			if (!ActionStates[actionName].ContainsKey(godotInputEventAsText))
			{
				ActionStates[actionName][godotInputEventAsText] = new()
				{
					{"PrevState", EventStates[InputEventData.GetGodotInputEventName(godotInputEventNames)]},
					{"State", EventStates[InputEventData.GetGodotInputEventName(godotInputEventNames)]},
				};
			}

			// update event state
			bool eventState = true;
			foreach (string godotInputEventName in godotInputEventNames)
			{
				eventState = eventState && EventStates[godotInputEventName]; // set to false if any event is false
			}

			if (godotInputEventAsText.Contains("Wheel"))
			{
				if (eventState)
				{
					ActionStates[actionName][godotInputEventAsText]["PrevState"] = false; // keep it false, wheel events are not continuous
					ActionStates[actionName][godotInputEventAsText]["State"] = eventState; // update current state
				}
				else
				{
					ActionStates[actionName][godotInputEventAsText]["PrevState"] = ActionStates[actionName][godotInputEventAsText]["State"]; // update prev state
					ActionStates[actionName][godotInputEventAsText]["State"] = eventState; // update current state
				}
			}

			else
			{
				ActionStates[actionName][godotInputEventAsText]["PrevState"] = ActionStates[actionName][godotInputEventAsText]["State"]; // update prev state
				ActionStates[actionName][godotInputEventAsText]["State"] = eventState; // update current state
			}
		}

		// update overall state
		foreach (var inputEventPair in ActionStates[actionName])
		{
			string inputEventName = inputEventPair.Key;
			if (inputEventName == "Overall" || inputEventName == "UniqueCallerNames") { continue; }

			ActionStates[actionName]["Overall"]["PrevState"] = (bool)ActionStates[actionName]["Overall"]["PrevState"] || (bool)ActionStates[actionName][inputEventName]["PrevState"]; // update prev state
			ActionStates[actionName]["Overall"]["State"] = (bool)ActionStates[actionName]["Overall"]["State"] || (bool)ActionStates[actionName][inputEventName]["State"]; // update current state to true if any event is true
		}
	}
	// returns CLASSNAME.METHODNAME.OFFSET; used to keep track of which caller has already called per frame
	static string GetUniqueCallerName()
	{
		StackTrace stackTrace = new();
		StackFrame[] stackFrames = stackTrace.GetFrames();
		string s = "";
		foreach (StackFrame stackFrame in stackFrames)
		{
			s += stackFrame.GetMethod().DeclaringType.Name + "." + stackFrame.GetMethod().Name + "." + stackFrame.GetNativeOffset() + ";";
		}
		return $"{s}";
	}

	static bool IsActionJustPressed(string actionName, string uniqueCallerName)
	{
		UpdateActionState(actionName);
		// GD.Print(uniqueCallerName);
		bool hasCalledThisFrame = false;
		if (ActionStates[actionName]["UniqueCallerNames"].ContainsKey(uniqueCallerName) && (int)ActionStates[actionName]["UniqueCallerNames"][uniqueCallerName] == Engine.GetFramesDrawn() % int.MaxValue) { hasCalledThisFrame = true; }
		ActionStates[actionName]["UniqueCallerNames"][uniqueCallerName] = Engine.GetFramesDrawn() % int.MaxValue;

		foreach (var inputEventPair in ActionStates[actionName])
		{
			if (inputEventPair.Key == "Overall" || inputEventPair.Key == "UniqueCallerNames") { continue; }
			string inputEventName = inputEventPair.Key;
		}

		UpdateActionState(actionName);
		
		return !(bool)ActionStates[actionName]["Overall"]["PrevState"] && (bool)ActionStates[actionName]["Overall"]["State"] && !hasCalledThisFrame;
	}
	static bool IsActionPressed(string actionName, string uniqueCallerName)
	{
		UpdateActionState(actionName);

		bool hasCalledThisFrame = false;
		if (ActionStates[actionName]["UniqueCallerNames"].ContainsKey(uniqueCallerName) && (int)ActionStates[actionName]["UniqueCallerNames"][uniqueCallerName] == Engine.GetFramesDrawn() % int.MaxValue) { hasCalledThisFrame = true; }
		ActionStates[actionName]["UniqueCallerNames"][uniqueCallerName] = Engine.GetFramesDrawn() % int.MaxValue;
		
		return (bool)ActionStates[actionName]["Overall"]["PrevState"] && (bool)ActionStates[actionName]["Overall"]["State"] && !hasCalledThisFrame;
	}
	static bool IsActionJustReleased(string actionName, string uniqueCallerName)
	{
		UpdateActionState(actionName);

		bool hasCalledThisFrame = false;
		if (ActionStates[actionName]["UniqueCallerNames"].ContainsKey(uniqueCallerName) && (int)ActionStates[actionName]["UniqueCallerNames"][uniqueCallerName] == Engine.GetFramesDrawn() % int.MaxValue) { hasCalledThisFrame = true; }
		ActionStates[actionName]["UniqueCallerNames"][uniqueCallerName] = Engine.GetFramesDrawn() % int.MaxValue;
		
		foreach (var inputEventPair in ActionStates[actionName])
		{
			if (inputEventPair.Key == "Overall" || inputEventPair.Key == "UniqueCallerNames") { continue; }
			string inputEventName = inputEventPair.Key;
		}

		return (bool)ActionStates[actionName]["Overall"]["PrevState"] && !(bool)ActionStates[actionName]["Overall"]["State"] && !hasCalledThisFrame;
	}
	

	static void EmitGlobalInputEventSignal(InputEventData inputEventData)
	{
		Instance.EmitSignal(SignalName.GlobalInputEventEx, inputEventData);
		Instance.EmitSignal(SignalName.GlobalInputEvent, inputEventData.GodotInputEvent);
		GC.Collect();
	}
	#endregion

	#region Signal Callbacks
	static void OnHookEnabled(object sender, HookEventArgs e) {}
	static void OnHookDisabled(object sender, HookEventArgs e) {}

	static void OnHookKeyPressed(object sender, KeyboardHookEventArgs e) { OnInputEvent(InputEventData.EnumInputEventType.Key, true, e); }
	static void OnHookKeyReleased(object sender, KeyboardHookEventArgs e) { OnInputEvent(InputEventData.EnumInputEventType.Key, false, e); }

	static void OnHookMouseMoved(object sender, MouseHookEventArgs e) { OnInputEvent(InputEventData.EnumInputEventType.MouseMotion, false, e); }
	static void OnHookMousePressed(object sender, MouseHookEventArgs e) { OnInputEvent(InputEventData.EnumInputEventType.MouseButton, true, e); }
	static void OnHookMouseReleased(object sender, MouseHookEventArgs e) { OnInputEvent(InputEventData.EnumInputEventType.MouseButton, false, e); }
	static void OnHookMouseWheel(object sender, MouseWheelHookEventArgs e) 
	{
		Instance.CallDeferred("ResetMouseWheelStates");
		OnInputEvent(InputEventData.EnumInputEventType.MouseWheel, true, e);
	}

	static void ResetMouseWheelStates()
	{
		EventStates["MouseWheelUp"] = false;
		EventStates["MouseWheelDown"] = false;
		EventStates["MouseWheelLeft"] = false;
		EventStates["MouseWheelRight"] = false;
	}
	#endregion
}
