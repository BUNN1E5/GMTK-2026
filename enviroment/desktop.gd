extends Node

func _init() -> void:
	pass
	
func _ready() -> void:
	get_window().mouse_passthrough = true
	#DisplayServer.window_set_flag(DisplayServer.WINDOW_FLAG_NO_FOCUS, true)
	pass

func _process(delta: float) -> void:
	pass
