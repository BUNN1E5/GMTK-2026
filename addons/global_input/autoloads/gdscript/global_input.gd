extends Node2D

signal global_input_event(input_event: InputEvent)
signal global_input_event_ex(input_event_data: Resource)

var csharp_global_input_scene: PackedScene = preload("res://addons/global_input/autoloads/csharp/global_input.tscn")
static var csharp_global_input = null

func _enter_tree() -> void:
	csharp_global_input = csharp_global_input_scene.instantiate()

	csharp_global_input.connect("GlobalInputEvent", on_csharp_global_input_event)
	csharp_global_input.connect("GlobalInputEventEx", on_csharp_global_input_event_ex)

	add_child(csharp_global_input)
	pass


static func is_action_just_pressed(action: String) -> bool:
	var s = Engine.capture_script_backtraces(true)
	return csharp_global_input.call("IsActionJustPressed", action, s)

static func is_action_pressed(action: String) -> bool:
	var s = Engine.capture_script_backtraces(true)
	return csharp_global_input.call("IsActionPressed", action, s)

static func is_action_just_released(action: String) -> bool:
	var s = Engine.capture_script_backtraces(true)
	return csharp_global_input.call("IsActionJustReleased", action, s)


static func get_vector(negative_x: String, positive_x: String, negative_y: String, positive_y: String) -> Vector2:
	return csharp_global_input.call("GetVector", negative_x, positive_x, negative_y, positive_y)

static func is_anything_pressed() -> bool:
	return csharp_global_input.call("IsAnythingPressed")

static func is_mouse_button_pressed(button: int) -> bool:
	return csharp_global_input.call("IsMouseButtonPressed", button)

func on_csharp_global_input_event(input_event: InputEvent) -> void:
	global_input_event.emit(input_event)

func on_csharp_global_input_event_ex(input_event_data: Resource) -> void:
	global_input_event_ex.emit(input_event_data)
