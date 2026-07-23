extends Node

var is_local : bool = true

func _init() -> void:
	GlobalInput.global_input_event.connect(global_input)
	#GlobalInput.global_input_event_ex.connect(global_input_ex)
	pass

func global_input(event : InputEvent):
	if(event is InputEventMouse):
		return
	
	if(event.is_pressed()):
		print("Key Pressed")
	pass
