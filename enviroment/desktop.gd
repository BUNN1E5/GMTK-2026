extends Node

@export var screen_size: Vector2i = DisplayServer.screen_get_size()

static var taskbar_height = null

func _init() -> void:
	pass

func _ready() -> void:
	#get_window().mouse_passthrough = true
	#get_tree().set_auto_accept_quit(false)
	#get_window().hide()
	pass

func bl_screen_pos(window : Window, pos : Vector2):
	var vheight : float = window.get_viewport().get_visible_rect().size.y
	#var vheight : int = DisplayServer.screen_get_size(DisplayServer.window_get_current_screen()).y
	return Vector2(pos.x, 20 + vheight - pos.y ) #HOTFIX :: GET THE POSITION CORRECT 

func update_player_clickables():
	if OS.has_feature("windows"):
		return
	for player in PlayerManager.players.values() as Array[Player]:
		get_window().mouse_passthrough_polygon += player.get_collision_polygon()[0]
	pass
