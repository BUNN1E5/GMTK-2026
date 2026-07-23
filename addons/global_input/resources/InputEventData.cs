using Godot;
using System;
using SharpHook;
using SharpHook.Data;
using System.Collections.Generic;

namespace GlobalInputAddon.Data;

public partial class InputEventData: Resource
{
	// Event Name Maps
	static readonly Dictionary<string, string[]> GodotInputEventToSharpHookEventMap = new()
	{
		{Key.None.ToString(), new string[] {KeyCode.VcUndefined.ToString()}},

		{Key.Escape.ToString(), new string[] {KeyCode.VcEscape.ToString()}},
		{Key.F1.ToString(), new string[] {KeyCode.VcF1.ToString()}},
		{Key.F2.ToString(), new string[] {KeyCode.VcF2.ToString()}},
		{Key.F3.ToString(), new string[] {KeyCode.VcF3.ToString()}},
		{Key.F4.ToString(), new string[] {KeyCode.VcF4.ToString()}},
		{Key.F5.ToString(), new string[] {KeyCode.VcF5.ToString()}},
		{Key.F6.ToString(), new string[] {KeyCode.VcF6.ToString()}},
		{Key.F7.ToString(), new string[] {KeyCode.VcF7.ToString()}},
		{Key.F8.ToString(), new string[] {KeyCode.VcF8.ToString()}},
		{Key.F9.ToString(), new string[] {KeyCode.VcF9.ToString()}},
		{Key.F10.ToString(), new string[] {KeyCode.VcF10.ToString()}},
		{Key.F11.ToString(), new string[] {KeyCode.VcF11.ToString()}},
		{Key.F12.ToString(), new string[] {KeyCode.VcF12.ToString()}},
		{Key.F13.ToString(), new string[] {KeyCode.VcF13.ToString()}},
		{Key.F14.ToString(), new string[] {KeyCode.VcF14.ToString()}},
		{Key.F15.ToString(), new string[] {KeyCode.VcF15.ToString()}},
		{Key.F16.ToString(), new string[] {KeyCode.VcF16.ToString()}},
		{Key.F17.ToString(), new string[] {KeyCode.VcF17.ToString()}},
		{Key.F18.ToString(), new string[] {KeyCode.VcF18.ToString()}},
		{Key.F19.ToString(), new string[] {KeyCode.VcF19.ToString()}},
		{Key.F20.ToString(), new string[] {KeyCode.VcF20.ToString()}},
		{Key.F21.ToString(), new string[] {KeyCode.VcF21.ToString()}},
		{Key.F22.ToString(), new string[] {KeyCode.VcF22.ToString()}},
		{Key.F23.ToString(), new string[] {KeyCode.VcF23.ToString()}},
		{Key.F24.ToString(), new string[] {KeyCode.VcF24.ToString()}},

		{Key.Quoteleft.ToString(), new string[] {KeyCode.VcBackQuote.ToString()}},
		{Key.Key0.ToString(), new string[] {KeyCode.Vc0.ToString()}},
		{Key.Key1.ToString(), new string[] {KeyCode.Vc1.ToString()}},
		{Key.Key2.ToString(), new string[] {KeyCode.Vc2.ToString()}},
		{Key.Key3.ToString(), new string[] {KeyCode.Vc3.ToString()}},
		{Key.Key4.ToString(), new string[] {KeyCode.Vc4.ToString()}},
		{Key.Key5.ToString(), new string[] {KeyCode.Vc5.ToString()}},
		{Key.Key6.ToString(), new string[] {KeyCode.Vc6.ToString()}},
		{Key.Key7.ToString(), new string[] {KeyCode.Vc7.ToString()}},
		{Key.Key8.ToString(), new string[] {KeyCode.Vc8.ToString()}},
		{Key.Key9.ToString(), new string[] {KeyCode.Vc9.ToString()}},
		{Key.Minus.ToString(), new string[] {KeyCode.VcMinus.ToString()}},
		{Key.Equal.ToString(), new string[] {KeyCode.VcEquals.ToString()}},
		{Key.Backspace.ToString(), new string[] {KeyCode.VcBackspace.ToString()}},

		{Key.Tab.ToString(), new string[] {KeyCode.VcTab.ToString()}},
		{Key.Capslock.ToString(), new string[] {KeyCode.VcCapsLock.ToString()}},

		{Key.A.ToString(), new string[] {KeyCode.VcA.ToString()}},
		{Key.B.ToString(), new string[] {KeyCode.VcB.ToString()}},
		{Key.C.ToString(), new string[] {KeyCode.VcC.ToString()}},
		{Key.D.ToString(), new string[] {KeyCode.VcD.ToString()}},
		{Key.E.ToString(), new string[] {KeyCode.VcE.ToString()}},
		{Key.F.ToString(), new string[] {KeyCode.VcF.ToString()}},
		{Key.G.ToString(), new string[] {KeyCode.VcG.ToString()}},
		{Key.H.ToString(), new string[] {KeyCode.VcH.ToString()}},
		{Key.I.ToString(), new string[] {KeyCode.VcI.ToString()}},
		{Key.J.ToString(), new string[] {KeyCode.VcJ.ToString()}},
		{Key.K.ToString(), new string[] {KeyCode.VcK.ToString()}},
		{Key.L.ToString(), new string[] {KeyCode.VcL.ToString()}},
		{Key.M.ToString(), new string[] {KeyCode.VcM.ToString()}},
		{Key.N.ToString(), new string[] {KeyCode.VcN.ToString()}},
		{Key.O.ToString(), new string[] {KeyCode.VcO.ToString()}},
		{Key.P.ToString(), new string[] {KeyCode.VcP.ToString()}},
		{Key.Q.ToString(), new string[] {KeyCode.VcQ.ToString()}},
		{Key.R.ToString(), new string[] {KeyCode.VcR.ToString()}},
		{Key.S.ToString(), new string[] {KeyCode.VcS.ToString()}},
		{Key.T.ToString(), new string[] {KeyCode.VcT.ToString()}},
		{Key.U.ToString(), new string[] {KeyCode.VcU.ToString()}},
		{Key.V.ToString(), new string[] {KeyCode.VcV.ToString()}},
		{Key.W.ToString(), new string[] {KeyCode.VcW.ToString()}},
		{Key.X.ToString(), new string[] {KeyCode.VcX.ToString()}},
		{Key.Y.ToString(), new string[] {KeyCode.VcY.ToString()}},
		{Key.Z.ToString(), new string[] {KeyCode.VcZ.ToString()}},

		{Key.Bracketleft.ToString(), new string[] {KeyCode.VcOpenBracket.ToString()}},
		{Key.Bracketright.ToString(), new string[] {KeyCode.VcCloseBracket.ToString()}},
		{Key.Backslash.ToString(), new string[] {KeyCode.VcBackslash.ToString()}},
		{Key.Semicolon.ToString(), new string[] {KeyCode.VcSemicolon.ToString()}},
		{Key.Apostrophe.ToString(), new string[] {KeyCode.VcQuote.ToString()}},
		// {Key.Quotedbl.ToString(), new string[] {KeyCode.VcQuote.ToString()}}, // TODO: check this; don't know if this is the same as apostrophe
		{Key.Enter.ToString(), new string[] {KeyCode.VcEnter.ToString()}},
		{Key.Comma.ToString(), new string[] {KeyCode.VcComma.ToString()}},
		{Key.Period.ToString(), new string[] {KeyCode.VcPeriod.ToString()}},
		{Key.Slash.ToString(), new string[] {KeyCode.VcSlash.ToString()}},
		{Key.Space.ToString(), new string[] {KeyCode.VcSpace.ToString()}},

		{Key.Print.ToString(), new string[] {KeyCode.VcPrintScreen.ToString()}},
		{Key.Scrolllock.ToString(), new string[] {KeyCode.VcScrollLock.ToString()}},
		{Key.Pause.ToString(), new string[] {KeyCode.VcPause.ToString()}},
		{Key.Insert.ToString(), new string[] {KeyCode.VcInsert.ToString()}},
		{Key.Home.ToString(), new string[] {KeyCode.VcHome.ToString()}},
		{Key.Pageup.ToString(), new string[] {KeyCode.VcPageUp.ToString()}},
		{Key.Delete.ToString(), new string[] {KeyCode.VcDelete.ToString()}},
		{Key.End.ToString(), new string[] {KeyCode.VcEnd.ToString()}},
		{Key.Pagedown.ToString(), new string[] {KeyCode.VcPageDown.ToString()}},

		{Key.Down.ToString(), new string[] {KeyCode.VcDown.ToString()}},
		{Key.Up.ToString(), new string[] {KeyCode.VcUp.ToString()}},
		{Key.Right.ToString(), new string[] {KeyCode.VcRight.ToString()}},
		{Key.Left.ToString(), new string[] {KeyCode.VcLeft.ToString()}},

		{Key.Numlock.ToString(), new string[] {KeyCode.VcNumLock.ToString()}},
		{Key.Kp0.ToString(), new string[] {KeyCode.VcNumPad0.ToString()}},
		{Key.Kp1.ToString(), new string[] {KeyCode.VcNumPad1.ToString()}},
		{Key.Kp2.ToString(), new string[] {KeyCode.VcNumPad2.ToString()}},
		{Key.Kp3.ToString(), new string[] {KeyCode.VcNumPad3.ToString()}},
		{Key.Kp4.ToString(), new string[] {KeyCode.VcNumPad4.ToString()}},
		{Key.Kp5.ToString(), new string[] {KeyCode.VcNumPad5.ToString()}},
		{Key.Kp6.ToString(), new string[] {KeyCode.VcNumPad6.ToString()}},
		{Key.Kp7.ToString(), new string[] {KeyCode.VcNumPad7.ToString()}},
		{Key.Kp8.ToString(), new string[] {KeyCode.VcNumPad8.ToString()}},
		{Key.Kp9.ToString(), new string[] {KeyCode.VcNumPad9.ToString()}},
		{Key.Clear.ToString(), new string[] {KeyCode.VcNumPadClear.ToString()}},
		{Key.KpDivide.ToString(), new string[] {KeyCode.VcNumPadDivide.ToString()}},
		{Key.KpMultiply.ToString(), new string[] {KeyCode.VcNumPadMultiply.ToString()}},
		{Key.KpSubtract.ToString(), new string[] {KeyCode.VcNumPadSubtract.ToString()}},
		{Key.KpAdd.ToString(), new string[] {KeyCode.VcNumPadAdd.ToString()}},
		{Key.KpPeriod.ToString(), new string[] {KeyCode.VcNumPadDecimal.ToString()}},
		{Key.KpEnter.ToString(), new string[] {KeyCode.VcNumPadEnter.ToString()}},

		{Key.Shift.ToString(), new string[] {KeyCode.VcLeftShift.ToString(), KeyCode.VcRightShift.ToString()}},
		{Key.Ctrl.ToString(), new string[] {KeyCode.VcLeftControl.ToString(), KeyCode.VcRightControl.ToString()}},
		{Key.Alt.ToString(), new string[] {KeyCode.VcLeftAlt.ToString(), KeyCode.VcRightAlt.ToString()}},
		{Key.Meta.ToString(), new string[] {KeyCode.VcLeftMeta.ToString(), KeyCode.VcRightMeta.ToString()}},

		{"Mouse" + Godot.MouseButton.Left.ToString(), new string[] {SharpHook.Data.MouseButton.Button1.ToString()}},
		{"Mouse" + Godot.MouseButton.Right.ToString(), new string[] {SharpHook.Data.MouseButton.Button2.ToString()}},
		{"Mouse" + Godot.MouseButton.Middle.ToString(), new string[] {SharpHook.Data.MouseButton.Button3.ToString()}},
		{"Mouse" + Godot.MouseButton.Xbutton1.ToString(), new string[] {SharpHook.Data.MouseButton.Button4.ToString()}},
		{"Mouse" + Godot.MouseButton.Xbutton2.ToString(), new string[] {SharpHook.Data.MouseButton.Button5.ToString()}},

		{"Mouse" + Godot.MouseButton.WheelUp.ToString(), new string[] {"WheelUp"}},
		{"Mouse" + Godot.MouseButton.WheelDown.ToString(), new string[] {"WheelDown"}},
		{"Mouse" + Godot.MouseButton.WheelLeft.ToString(), new string[] {"WheelLeft"}},
		{"Mouse" + Godot.MouseButton.WheelRight.ToString(), new string[] {"WheelRight"}},
		{"MouseMotion", new string[] {"VcMouseMotion"}},
	};
	static readonly Dictionary<string, string> SharpHookEventToGodotInputEventMap = new()
	{
		{KeyCode.VcUndefined.ToString(), Key.None.ToString()},

		{KeyCode.VcEscape.ToString(), Key.Escape.ToString()},
		{KeyCode.VcF1.ToString(), Key.F1.ToString()},
		{KeyCode.VcF2.ToString(), Key.F2.ToString()},
		{KeyCode.VcF3.ToString(), Key.F3.ToString()},
		{KeyCode.VcF4.ToString(), Key.F4.ToString()},
		{KeyCode.VcF5.ToString(), Key.F5.ToString()},
		{KeyCode.VcF6.ToString(), Key.F6.ToString()},
		{KeyCode.VcF7.ToString(), Key.F7.ToString()},
		{KeyCode.VcF8.ToString(), Key.F8.ToString()},
		{KeyCode.VcF9.ToString(), Key.F9.ToString()},
		{KeyCode.VcF10.ToString(), Key.F10.ToString()},
		{KeyCode.VcF11.ToString(), Key.F11.ToString()},
		{KeyCode.VcF12.ToString(), Key.F12.ToString()},
		{KeyCode.VcF13.ToString(), Key.F13.ToString()},
		{KeyCode.VcF14.ToString(), Key.F14.ToString()},
		{KeyCode.VcF15.ToString(), Key.F15.ToString()},
		{KeyCode.VcF16.ToString(), Key.F16.ToString()},
		{KeyCode.VcF17.ToString(), Key.F17.ToString()},
		{KeyCode.VcF18.ToString(), Key.F18.ToString()},
		{KeyCode.VcF19.ToString(), Key.F19.ToString()},
		{KeyCode.VcF20.ToString(), Key.F20.ToString()},
		{KeyCode.VcF21.ToString(), Key.F21.ToString()},
		{KeyCode.VcF22.ToString(), Key.F22.ToString()},
		{KeyCode.VcF23.ToString(), Key.F23.ToString()},
		{KeyCode.VcF24.ToString(), Key.F24.ToString()},

		{KeyCode.VcBackQuote.ToString(), Key.Quoteleft.ToString()},
		{KeyCode.Vc0.ToString(), Key.Key0.ToString()},
		{KeyCode.Vc1.ToString(), Key.Key1.ToString()},
		{KeyCode.Vc2.ToString(), Key.Key2.ToString()},
		{KeyCode.Vc3.ToString(), Key.Key3.ToString()},
		{KeyCode.Vc4.ToString(), Key.Key4.ToString()},
		{KeyCode.Vc5.ToString(), Key.Key5.ToString()},
		{KeyCode.Vc6.ToString(), Key.Key6.ToString()},
		{KeyCode.Vc7.ToString(), Key.Key7.ToString()},
		{KeyCode.Vc8.ToString(), Key.Key8.ToString()},
		{KeyCode.Vc9.ToString(), Key.Key9.ToString()},
		{KeyCode.VcMinus.ToString(), Key.Minus.ToString()},
		{KeyCode.VcEquals.ToString(), Key.Equal.ToString()},
		{KeyCode.VcBackspace.ToString(), Key.Backspace.ToString()},

		{KeyCode.VcTab.ToString(), Key.Tab.ToString()},
		{KeyCode.VcCapsLock.ToString(), Key.Capslock.ToString()},

		{KeyCode.VcA.ToString(), Key.A.ToString()},
		{KeyCode.VcB.ToString(), Key.B.ToString()},
		{KeyCode.VcC.ToString(), Key.C.ToString()},
		{KeyCode.VcD.ToString(), Key.D.ToString()},
		{KeyCode.VcE.ToString(), Key.E.ToString()},
		{KeyCode.VcF.ToString(), Key.F.ToString()},
		{KeyCode.VcG.ToString(), Key.G.ToString()},
		{KeyCode.VcH.ToString(), Key.H.ToString()},
		{KeyCode.VcI.ToString(), Key.I.ToString()},
		{KeyCode.VcJ.ToString(), Key.J.ToString()},
		{KeyCode.VcK.ToString(), Key.K.ToString()},
		{KeyCode.VcL.ToString(), Key.L.ToString()},
		{KeyCode.VcM.ToString(), Key.M.ToString()},
		{KeyCode.VcN.ToString(), Key.N.ToString()},
		{KeyCode.VcO.ToString(), Key.O.ToString()},
		{KeyCode.VcP.ToString(), Key.P.ToString()},
		{KeyCode.VcQ.ToString(), Key.Q.ToString()},
		{KeyCode.VcR.ToString(), Key.R.ToString()},
		{KeyCode.VcS.ToString(), Key.S.ToString()},
		{KeyCode.VcT.ToString(), Key.T.ToString()},
		{KeyCode.VcU.ToString(), Key.U.ToString()},
		{KeyCode.VcV.ToString(), Key.V.ToString()},
		{KeyCode.VcW.ToString(), Key.W.ToString()},
		{KeyCode.VcX.ToString(), Key.X.ToString()},
		{KeyCode.VcY.ToString(), Key.Y.ToString()},
		{KeyCode.VcZ.ToString(), Key.Z.ToString()},

		{KeyCode.VcOpenBracket.ToString(), Key.Bracketleft.ToString()},
		{KeyCode.VcCloseBracket.ToString(), Key.Bracketright.ToString()},
		{KeyCode.VcBackslash.ToString(), Key.Backslash.ToString()},
		{KeyCode.VcSemicolon.ToString(), Key.Semicolon.ToString()},
		{KeyCode.VcQuote.ToString(), Key.Apostrophe.ToString()},
		{KeyCode.VcEnter.ToString(), Key.Enter.ToString()},
		{KeyCode.VcComma.ToString(), Key.Comma.ToString()},
		{KeyCode.VcPeriod.ToString(), Key.Period.ToString()},
		{KeyCode.VcSlash.ToString(), Key.Slash.ToString()},
		{KeyCode.VcSpace.ToString(), Key.Space.ToString()},

		{KeyCode.VcPrintScreen.ToString(), Key.Print.ToString()},
		{KeyCode.VcScrollLock.ToString(), Key.Scrolllock.ToString()},
		{KeyCode.VcPause.ToString(), Key.Pause.ToString()},
		{KeyCode.VcInsert.ToString(), Key.Insert.ToString()},
		{KeyCode.VcHome.ToString(), Key.Home.ToString()},
		{KeyCode.VcPageUp.ToString(), Key.Pageup.ToString()},
		{KeyCode.VcDelete.ToString(), Key.Delete.ToString()},
		{KeyCode.VcEnd.ToString(), Key.End.ToString()},
		{KeyCode.VcPageDown.ToString(), Key.Pagedown.ToString()},

		{KeyCode.VcNumLock.ToString(), Key.Numlock.ToString()},
		{KeyCode.VcNumPad0.ToString(), Key.Kp0.ToString()},
		{KeyCode.VcNumPad1.ToString(), Key.Kp1.ToString()},
		{KeyCode.VcNumPad2.ToString(), Key.Kp2.ToString()},
		{KeyCode.VcNumPad3.ToString(), Key.Kp3.ToString()},
		{KeyCode.VcNumPad4.ToString(), Key.Kp4.ToString()},
		{KeyCode.VcNumPad5.ToString(), Key.Kp5.ToString()},
		{KeyCode.VcNumPad6.ToString(), Key.Kp6.ToString()},
		{KeyCode.VcNumPad7.ToString(), Key.Kp7.ToString()},
		{KeyCode.VcNumPad8.ToString(), Key.Kp8.ToString()},
		{KeyCode.VcNumPad9.ToString(), Key.Kp9.ToString()},
		{KeyCode.VcNumPadDivide.ToString(), Key.KpDivide.ToString()},
		{KeyCode.VcNumPadMultiply.ToString(), Key.KpMultiply.ToString()},
		{KeyCode.VcNumPadSubtract.ToString(), Key.KpSubtract.ToString()},
		{KeyCode.VcNumPadAdd.ToString(), Key.KpAdd.ToString()},
		{KeyCode.VcNumPadDecimal.ToString(), Key.KpPeriod.ToString()},
		{KeyCode.VcNumPadEnter.ToString(), Key.KpEnter.ToString()},

		{KeyCode.VcLeftShift.ToString(), Key.Shift.ToString()},
		{KeyCode.VcRightShift.ToString(), Key.Shift.ToString()},
		{KeyCode.VcLeftControl.ToString(), Key.Ctrl.ToString()},
		{KeyCode.VcRightControl.ToString(), Key.Ctrl.ToString()},
		{KeyCode.VcLeftAlt.ToString(), Key.Alt.ToString()},
		{KeyCode.VcRightAlt.ToString(), Key.Alt.ToString()},
		{KeyCode.VcLeftMeta.ToString(), Key.Meta.ToString()},
		{KeyCode.VcRightMeta.ToString(), Key.Meta.ToString()},

		{SharpHook.Data.MouseButton.Button1.ToString(), "Mouse" + Godot.MouseButton.Left.ToString()},
		{SharpHook.Data.MouseButton.Button2.ToString(), "Mouse" + Godot.MouseButton.Right.ToString()},
		{SharpHook.Data.MouseButton.Button3.ToString(), "Mouse" + Godot.MouseButton.Middle.ToString()},
		{SharpHook.Data.MouseButton.Button4.ToString(), "Mouse" + Godot.MouseButton.Xbutton1.ToString()},
		{SharpHook.Data.MouseButton.Button5.ToString(), "Mouse" + Godot.MouseButton.Xbutton2.ToString()},

		{"WheelUp", "Mouse" + Godot.MouseButton.WheelUp.ToString()},
		{"WheelDown", "Mouse" + Godot.MouseButton.WheelDown.ToString()},
		{"WheelLeft", "Mouse" + Godot.MouseButton.WheelLeft.ToString()},
		{"WheelRight", "Mouse" + Godot.MouseButton.WheelRight.ToString()},

		{"VcMouseMotion", "MouseMotion"},
	};
	
	// Enums
	public enum EnumInputEventType
	{
		Key,
		MouseButton,
		MouseMotion,
		MouseWheel
	}
	public enum EnumMouseWheelRotation
	{
		Up = 360,
		Down = -360,
		Left = 360,
		Right = -360
	}
	
	// Input Event Fields
	EnumInputEventType type; public EnumInputEventType Type { get { return type; } set { type = value; } }
	bool pressed; public bool Pressed { get { return pressed; } set { pressed = value; } }

	#region Mouse Fields
	// General Mouse Fields
	// relative to top left of primary monitor
	Vector2 mouseGlobalPosition; public Vector2 MouseGlobalPosition { get { return mouseGlobalPosition; } set { mouseGlobalPosition = value; } }
	// relative to top left of primary window
	Vector2 mousePosition; public Vector2 MousePosition { get { return mousePosition; } set { mousePosition = value; } }

	// Godot Mouse Fields
	Vector2 mouseRelative; public Vector2 MouseRelative { get { return mouseRelative; } set { mouseRelative = value; } }
	Vector2 mouseScreenRelative; public Vector2 MouseScreenRelative { get { return mouseScreenRelative; } set { mouseScreenRelative = value; } }
	Vector2 mouseVelocity; public Vector2 MouseVelocity { get { return mouseVelocity; } set { mouseVelocity = value; } }
	Vector2 mouseScreenVelocity; public Vector2 MouseScreenVelocity { get { return mouseScreenVelocity; } set { mouseScreenVelocity = value; } }

	// Mouse Wheel Fields
	bool isMouseWheelPressed; public bool IsMouseWheelPressed { get { return isMouseWheelPressed; } set { isMouseWheelPressed = value; } }
	MouseWheelScrollDirection mouseWheelDirection; public MouseWheelScrollDirection MouseWheelDirection { get { return mouseWheelDirection; } set { mouseWheelDirection = value; } }
	// 360 or -360
	short mouseWheelRotation; public short MouseWheelRotation { get { return mouseWheelRotation; } set { mouseWheelRotation = value; } }
	#endregion

	// General Fields
	// last index will be for keycode/mousebutton name [^1] all others will be for modifiers
	string[] godotInputEventNames; public string[] GodotInputEventNames { get { return godotInputEventNames; } set { godotInputEventNames = value; } }
	// one godot input event can be mapped to multiple sharp hook events
	string[] sharpHookEventNames; public string[] SharpHookEventNames { get { return sharpHookEventNames; } set { sharpHookEventNames = value; } }
	// used for godot input events
	InputEvent godotInputEvent; public InputEvent GodotInputEvent { get { return godotInputEvent; } set { godotInputEvent = value; } }
	// used to keep track of events pressed; is an array incase of Shift+Ctrl+Alt+Meta in godot input events
	HookEventArgs[] sharpHookEvents; public HookEventArgs[] SharpHookEvents { get { return sharpHookEvents; } set { sharpHookEvents = value; } }

	#region Constructors
	public InputEventData() { }
	public InputEventData(EnumInputEventType type, bool pressed, InputEvent inputEvent)
	{
		// input event fields
		Type = type;
		Pressed = pressed;
		// general mouse fields
		// TODO: add general mouse fields and mouse wheel fields

		// general fields
		GodotInputEventNames = GetGodotInputEventNames(inputEvent);
		SharpHookEventNames = GodotInputEventNameToSharpHookEventNames(GodotInputEventNames[^1]); // more than one sharp hook event can map to one godot input event
		GodotInputEvent = inputEvent;
		SharpHookEvents = GodotInputEventToSharpHookEvents(GodotInputEvent); // more than one sharp hook event can map to one godot input event
	}
	public InputEventData(EnumInputEventType type, bool pressed, HookEventArgs sharpHookEvent)
	{
		// input event fields
		Type = type;
		Pressed = pressed;
		// general mouse fields
		// TODO: add general mouse fields and mouse wheel fields
		
		// general fields
		GodotInputEventNames = [SharpHookEventNameToGodotInputEventName(GetSharpHookEventName(sharpHookEvent))];
		SharpHookEventNames = GodotInputEventNameToSharpHookEventNames(GodotInputEventNames[^1]); // more than one sharp hook event can map to one godot input event
		GodotInputEvent = SharpHookEventToGodotInputEvent(sharpHookEvent);
		SharpHookEvents = GodotInputEventToSharpHookEvents(GodotInputEvent); // more than one sharp hook event can map to one godot input event
	}
	
	~InputEventData()
	{
		godotInputEvent?.Dispose();
	}
	#endregion

	public override string ToString()
	{
		string result = "";
		result += "Type: " + Type + "\n";
		result += "Pressed: " + Pressed + "\n";
		result += "GodotInputEventNames: [";
		foreach (string godotInputEventName in GodotInputEventNames)
		{
			result += godotInputEventName + ", ";
		}
		result += "]\n";
		result += "SharpHookEventNames: [";
		foreach (string sharpHookEventName in SharpHookEventNames)
		{
			result += sharpHookEventName + ", ";
		}
		result += "]\n";
		result += "GodotInputEvent: ";
		foreach (string godotInputEventName in GetGodotInputEventNames(GodotInputEvent))
		{
			result += godotInputEventName + ", ";
		}
		result += "SharpHookEvents: [";
		foreach (HookEventArgs sharpHookEvent in SharpHookEvents)
		{
			result += GetSharpHookEventName(sharpHookEvent) + ", ";
		}
		result += "]\n";
		return result;
	}
	#region Helper Functions
	// Convert Godot InputEvent to SharpHook HookEventArgs and Vice Versa
	public InputEvent SharpHookEventToGodotInputEvent(HookEventArgs sharpHookEvent)
	{
		string sharpHookEventName = GetSharpHookEventName(sharpHookEvent);
		string godotInputEventName = SharpHookEventNameToGodotInputEventName(sharpHookEventName);
		InputEvent godotInputEvent = null;
		if (sharpHookEvent is KeyboardHookEventArgs)
		{
			godotInputEvent = new InputEventKey()
			{
				Keycode = Enum.Parse<Key>(godotInputEventName),
				PhysicalKeycode = Enum.Parse<Key>(godotInputEventName),
			};
		}
		else if (sharpHookEvent is MouseHookEventArgs mouseHookEventArgs)
		{
			if (mouseHookEventArgs.Data.Button == SharpHook.Data.MouseButton.NoButton)
			{
				godotInputEvent = new InputEventMouseMotion()
				{
					Position = MousePosition,
					Relative = MouseRelative,
					ScreenRelative = MouseScreenRelative,
					Velocity = MouseVelocity,
					ScreenVelocity = MouseScreenVelocity
				};
			}
			else
			{
				godotInputEvent = new InputEventMouseButton()
				{
					ButtonIndex = Enum.Parse<Godot.MouseButton>(godotInputEventName.TrimPrefix("Mouse"))
				};
			}
		}
		else if (sharpHookEvent is MouseWheelHookEventArgs mouseWheelHookEventArgs)
		{
			switch (mouseWheelHookEventArgs.Data.Direction)
			{
				case MouseWheelScrollDirection.Vertical:
					if (mouseWheelHookEventArgs.Data.Rotation == (short)EnumMouseWheelRotation.Up) godotInputEvent = new InputEventMouseButton() { ButtonIndex = Godot.MouseButton.WheelUp };
					if (mouseWheelHookEventArgs.Data.Rotation == (short)EnumMouseWheelRotation.Down) godotInputEvent = new InputEventMouseButton() { ButtonIndex = Godot.MouseButton.WheelDown };
					break;
				case MouseWheelScrollDirection.Horizontal:
					if (mouseWheelHookEventArgs.Data.Rotation == (short)EnumMouseWheelRotation.Left) godotInputEvent = new InputEventMouseButton() { ButtonIndex = Godot.MouseButton.WheelLeft };
					if (mouseWheelHookEventArgs.Data.Rotation == (short)EnumMouseWheelRotation.Right) godotInputEvent = new InputEventMouseButton() { ButtonIndex = Godot.MouseButton.WheelRight };
					break;
			}
		}
		return godotInputEvent;
	}
	public HookEventArgs[] GodotInputEventToSharpHookEvents(InputEvent inputEvent)
	{
		string[] godotInputEventNames = GetGodotInputEventNames(inputEvent);
		string godotInputEventName = GetGodotInputEventName(godotInputEventNames);
		HookEventArgs[] sharpHookEvents = new HookEventArgs[godotInputEventNames.Length];

		// add modifiers
		for (int i = 0; i < godotInputEventNames.Length - 2; i++) // only loop through the modifiers
		{
			string[] sharpHookEventNames = GodotInputEventNameToSharpHookEventNames(godotInputEventNames[i]);
			foreach (string sharpHookEventName in sharpHookEventNames) // modifiers will always be keys
			{
				UioHookEvent rawData = new();
				rawData.Keyboard.KeyCode = Enum.Parse<KeyCode>(sharpHookEventName);
				KeyboardHookEventArgs sharpHookEvent = new(rawData);
				sharpHookEvents[i] = sharpHookEvent;
			}
		}

		// add event by type
		if (inputEvent is InputEventKey inputEventKey)
		{
			string[] sharpHookEventNames = GodotInputEventNameToSharpHookEventNames(godotInputEventName);
			foreach (string sharpHookEventName in sharpHookEventNames)
			{
				UioHookEvent rawData = new();
				rawData.Keyboard.KeyCode = Enum.Parse<KeyCode>(sharpHookEventName);
				KeyboardHookEventArgs sharpHookEvent = new(rawData);
				sharpHookEvents[^1] = sharpHookEvent;
			}
		}
		else if (inputEvent is InputEventMouseButton inputEventMouseButton)
		{
			string[] sharpHookEventNames = GodotInputEventNameToSharpHookEventNames(godotInputEventName);
			foreach (string sharpHookEventName in sharpHookEventNames)
			{
				UioHookEvent rawData = new();
				switch (sharpHookEventName)
				{
					case "WheelUp":
						rawData.Wheel.Direction = MouseWheelScrollDirection.Vertical;
						rawData.Wheel.Rotation = (short)EnumMouseWheelRotation.Up;
						MouseWheelHookEventArgs sharpHookEvent = new(rawData);
						sharpHookEvents[^1] = sharpHookEvent;
						break;
					case "WheelDown":
						rawData.Wheel.Direction = MouseWheelScrollDirection.Vertical;
						rawData.Wheel.Rotation = (short)EnumMouseWheelRotation.Down;
						MouseWheelHookEventArgs sharpHookEvent2 = new(rawData);
						sharpHookEvents[^1] = sharpHookEvent2;
						break;
					case "WheelLeft":
						rawData.Wheel.Direction = MouseWheelScrollDirection.Horizontal;
						rawData.Wheel.Rotation = (short)EnumMouseWheelRotation.Left;
						MouseWheelHookEventArgs sharpHookEvent3 = new(rawData);
						sharpHookEvents[^1] = sharpHookEvent3;
						break;
					case "WheelRight":
						rawData.Wheel.Direction = MouseWheelScrollDirection.Horizontal;
						rawData.Wheel.Rotation = (short)EnumMouseWheelRotation.Right;
						MouseWheelHookEventArgs sharpHookEvent4 = new(rawData);
						sharpHookEvents[^1] = sharpHookEvent4;
						break;
					default:
						rawData.Mouse.Button = Enum.Parse<SharpHook.Data.MouseButton>(sharpHookEventName);
						MouseHookEventArgs sharpHookEvent5 = new(rawData);
						sharpHookEvents[^1] = sharpHookEvent5;
						break;
				}
			}
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion)
		{
			UioHookEvent rawData = new();
			MouseWheelHookEventArgs sharpHookEvent = new(rawData);
			sharpHookEvents[^1] = sharpHookEvent;
		}
		return sharpHookEvents;
	}
	public static string SharpHookEventNameToGodotInputEventName(string sharpHookEventName)
	{
		return SharpHookEventToGodotInputEventMap[sharpHookEventName];
	}
	public static string[] GodotInputEventNameToSharpHookEventNames(string godotInputEventName)
	{
		return GodotInputEventToSharpHookEventMap[godotInputEventName];
	}
	// Get Names From InputEvent and HookEventArgs
	public static string GetGodotInputEventName(string[] godotInputEventNames) => godotInputEventNames[^1];
	public static string[] GetGodotInputEventNames(InputEvent inputEvent)
	{
		string[] godotInputEventNames = [Key.None.ToString()];
		if (inputEvent is InputEventKey inputEventKey)
		{
			godotInputEventNames = inputEventKey.AsText().Split('+');
			godotInputEventNames[^1] = (inputEventKey.Keycode != Key.None) ? inputEventKey.Keycode.ToString() : inputEventKey.PhysicalKeycode.ToString();
		}
		else if (inputEvent is InputEventMouseButton inputEventMouseButton) // includes buttons and wheel
		{
			godotInputEventNames = inputEventMouseButton.AsText().Split('+');
			godotInputEventNames[^1] = "Mouse" + inputEventMouseButton.ButtonIndex.ToString();
		}
		else if (inputEvent is InputEventMouseMotion inputEventMouseMotion)
		{
			godotInputEventNames = ["MouseMotion"];
		}
		return godotInputEventNames;
	}
	public static string GetSharpHookEventName(HookEventArgs sharpHookEvent)
	{
		if (sharpHookEvent is KeyboardHookEventArgs keyboardHookEventArgs)
		{
			return keyboardHookEventArgs.Data.KeyCode.ToString();
		}
		else if (sharpHookEvent is MouseHookEventArgs mouseHookEventArgs)
		{
			return (mouseHookEventArgs.Data.Button != SharpHook.Data.MouseButton.NoButton) ? mouseHookEventArgs.Data.Button.ToString() : "VcMouseMotion";
		}
		else if (sharpHookEvent is MouseWheelHookEventArgs mouseWheelHookEventArgs)
		{
			switch (mouseWheelHookEventArgs.Data.Direction)
			{
				case MouseWheelScrollDirection.Vertical:
					if (mouseWheelHookEventArgs.Data.Rotation == (short)EnumMouseWheelRotation.Up) return "WheelUp";
					if (mouseWheelHookEventArgs.Data.Rotation == (short)EnumMouseWheelRotation.Down) return "WheelDown";
					break;
				case MouseWheelScrollDirection.Horizontal:
					if (mouseWheelHookEventArgs.Data.Rotation == (short)EnumMouseWheelRotation.Left) return "WheelLeft";
					if (mouseWheelHookEventArgs.Data.Rotation == (short)EnumMouseWheelRotation.Right) return "WheelRight";
					break;
			}
		}
		return KeyCode.VcUndefined.ToString();
	}
	
	public static Vector2 GlobalMousePositionToMousePosition(Vector2 globalMousePosition)
	{
		Vector2 windowPosition = DisplayServer.WindowGetPosition();
		Vector2 screenPosition = DisplayServer.ScreenGetPosition();
		Vector2 mousePosition = globalMousePosition - windowPosition + screenPosition;
		return mousePosition;
	}
	public static Vector2 MousePositionToGlobalMousePosition(Vector2 mousePosition)
	{
		Vector2 windowPosition = DisplayServer.WindowGetPosition();
		Vector2 screenPosition = DisplayServer.ScreenGetPosition();
		Vector2 globalMousePosition = mousePosition + windowPosition - screenPosition;
		return globalMousePosition;
	}
	#endregion
}
