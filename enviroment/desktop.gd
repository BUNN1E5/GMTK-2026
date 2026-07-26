extends Node

@export var screen_size: Vector2i = DisplayServer.screen_get_size()

func _init() -> void:
	pass
	
func _ready() -> void:
	get_window().mouse_passthrough_polygon = [0,0,0,0]
	pass

func update_player_clickables():
	get_window().mouse_passthrough_polygon = [0,0,0,0]
	for player in PlayerManager.players.values() as Array[Player]:
		get_window().mouse_passthrough_polygon += player.get_collision_polygon()
	pass
