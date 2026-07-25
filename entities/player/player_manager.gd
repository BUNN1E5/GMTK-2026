extends MultiplayerSpawner

var players : Dictionary[int, Player]
var local : Player

func _ready() -> void:	
	#multiplayer.peer_connected.connect(_spawn_local_player) 
	#multiplayer.peer_disconnected.connect(remove_player)
	
	#Client Only
	multiplayer.connected_to_server.connect(_on_connect_to_server)
	pass

func _send_local_player_spawn_request() -> void:
	var player_data = PlayerData.load()
	if not player_data:
		player_data = load("res://entities/player/base_player_data.tres") as PlayerData
	
	var data_dict = player_data.to_dict()
	
	if multiplayer.is_server():
		# Host spawns directly (peer_id 1)
	else:
		# Client sends RPC request to host (peer_id 1)
		rpc_id(1, "request_spawn_player", data_dict)

func _on_connect_to_server():
	
	pass

@rpc("any_peer", "call_local", "reliable")
func _request_spawn(data_dict : Dictionary):
	if not multiplayer.is_server():
		return
		
	var peer_id = multiplayer.get_remote_sender_id()
	if peer_id == 0:
		peer_id = multiplayer.get_unique_id()
	
	if players.has(peer_id):
		return # prevent duplicates
	
	var player_node = spawn({"peer_id":peer_id, "data":data_dict})
	players[peer_id] = player_node
