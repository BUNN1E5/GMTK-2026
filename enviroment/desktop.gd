extends Node

func _init() -> void:
	pass
	
func _ready() -> void:
	var screen_size: Vector2i = DisplayServer.screen_get_size()
	
	get_window().mouse_passthrough_polygon = [0,0,0,0]
	pass

func add_player_clickable(player : Player):
	
	pass
