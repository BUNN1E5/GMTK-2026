extends Node

var is_local : bool = true

func _ready() -> void:
	GlobalInput.global_input_event.connect(global_input)
	#GlobalInput.global_input_event_ex.connect(global_input_ex)
	pass

#The global input event does work
#however it does not have the attributes associated with an input event
#such as is pressed
#We dont actually care about that, so we can kinda just assume that
#we only care about every OTHER input
#OR we only care about unique inputs
var last_event : InputEvent
func global_input(event : InputEvent):
	if event is InputEventMouseMotion:
		return
	
	if not event.is_match(last_event):
		print("Unique Key Pressed")
	last_event = event
	pass
	
func _process(delta: float) -> void:
	#print("Test")
	pass
