extends Node

func _init() -> void:
	pass
	
func _ready() -> void:
	get_window().mouse_passthrough_polygon = [0,0,0,0]
	#For Debugging
	#DisplayServer.window_set_flag(DisplayServer.WINDOW_FLAG_NO_FOCUS, true)
	pass

func _process(delta: float) -> void:
	pass

func _physics_process(delta: float) -> void:
	if(GlobalInput.is_mouse_button_pressed(0)):
		print("Mouse Click")
	pass
