extends Node

@export var screen_size: Vector2i = DisplayServer.screen_get_size()

func _init() -> void:
	pass
	
func _ready() -> void:
	#For Debugging purposes only
	if OS.has_feature("windows"):
		var winfixscript = load("res://helpers/TransparentWindow.cs")
		get_tree().get_root().set_transparent_background(true)
		print("For Debugging Purposes Windows is set to only passthru")
		var winfix = winfixscript.new()
		winfix.Intialize()
		winfix.SetClickThrough(true)
		return
	get_window().mouse_passthrough_polygon = [0,0,0,0]
	pass
	
func bl_screen_pos(pos : Vector2):
	var viewport_height : float = get_viewport().get_visible_rect().size.y
	return Vector2(pos.x, viewport_height - pos.y)

func update_player_clickables():
	get_window().mouse_passthrough_polygon = [0,0,0,0]
	for player in PlayerManager.players.values() as Array[Player]:
		get_window().mouse_passthrough_polygon += player.get_collision_polygon()[0]
	pass
