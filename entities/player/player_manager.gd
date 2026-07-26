extends MultiplayerSpawner

@export var player_scene: PackedScene = preload("res://entities/player/player.tscn")

var players: Dictionary[int, Player]
var local: Player

func _notification(what: int) -> void:
	if what == NOTIFICATION_WM_CLOSE_REQUEST:
			PlayerData.save(local.player_data, true)

func _ready() -> void:
	# 1. Register custom spawn function so server and clients instantiate nodes the same way
	spawn_function = _custom_spawn
	spawn_path = get_path()
	
	multiplayer.connected_to_server.connect(_on_connect_to_server)
	multiplayer.peer_disconnected.connect(_on_peer_disconnected)
	
	# If host starts, spawn the local host player right away
	if multiplayer.is_server():
		_send_local_player_spawn_request.call_deferred()

func _send_local_player_spawn_request() -> void:
	var player_data = PlayerData.load()
	var data_dict = player_data.to_dict()
	
	if multiplayer.is_server():
		# Host directly spawns their own player
		_request_spawn(data_dict)
	else:
		# Client requests the host (peer 1) to spawn them
		rpc_id(1, "_request_spawn", data_dict)

func _on_connect_to_server() -> void:
	# Client calls this after establishing a connection
	_send_local_player_spawn_request()

@rpc("any_peer", "call_local", "reliable")
func _request_spawn(data_dict: Dictionary) -> void:
	if not multiplayer.is_server():
		return
		
	var peer_id = multiplayer.get_remote_sender_id()
	if peer_id == 0:
		peer_id = multiplayer.get_unique_id()
	
	if players.has(peer_id):
		return 
	
	# Calling spawn() automatically syncs to all clients and calls `_custom_spawn` on everyone!
	var player_node = spawn({"peer_id": peer_id, "data": data_dict}) as Player
	players[peer_id] = player_node

## This runs automatically on SERVER and ALL CLIENTS when spawn() is called
func _custom_spawn(data: Variant) -> Node:
	var peer_id: int = data["peer_id"]
	var data_dict: Dictionary = data["data"]
	
	var player_instance = player_scene.instantiate() as Player
	
	# Set Node Name (Godot requires matching Node paths on all peers for network syncing!)
	player_instance.name = str(peer_id)
	
	# Give ownership of this node to the client peer
	player_instance.set_multiplayer_authority(peer_id)
	
	var player_data = PlayerData.from_dict(data_dict)
	(func(player_data):
		player_instance.apply_player_data(player_data)
		player_instance.position = Desktop.bl_screen_pos(player_instance.get_size()/2 + Vector2(0, 0))
		Desktop.update_player_clickables()
		pass
	).call_deferred(player_data)
	
	# Track local player instance for quick access
	if peer_id == multiplayer.get_unique_id():
		local = player_instance
	
	return player_instance

func _on_peer_disconnected(peer_id: int) -> void:
	if multiplayer.is_server() and players.has(peer_id):
		var player_node = players[peer_id]
		players.erase(peer_id)
		if is_instance_valid(player_node):
			player_node.queue_free()
