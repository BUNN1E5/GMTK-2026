extends Node
#class_name PeerConnect

#P2P is direct party joining
var local_user_id = ""
var is_host = false
var local_lobby: HLobby

var peer : EOSGMultiplayerPeer = EOSGMultiplayerPeer.new()
var peer_user_ids : Array[int]

func _ready() -> void:
	pass

func create_lobby(lobby_id):
	var create_opts := EOS.Lobby.CreateLobbyOptions.new()
	create_opts.bucket_id = local_user_id
	create_opts.max_lobby_members = 64
	var lobby = await HLobbies.create_lobby_async(create_opts)
	if(not lobby):
		print("Lobby Creation Failed: " + EOS.result_str(lobby))
		return
	var result := peer.create_server(lobby_id)
	if result != OK:
		printerr("Failed to create client: " + EOS.result_str(result))
		return
	multiplayer.multiplayer_peer = peer
	is_host = true
	local_lobby = lobby
	pass
	
func join_lobby(lobby_id : String) -> bool:
	var lobbies = await HLobbies.search_by_bucket_id_async(lobby_id)
	if not lobbies:
		printerr("No lobbies found")
		return false
	var lobby : HLobby = lobbies[0] # This should be a unique ID from the player
	await HLobbies.join_by_id_async(lobby.lobby_id)

	var result := peer.create_client(lobby_id, lobby.owner_product_user_id)
	if not EOS.is_success(result):
		print("Failed to create client " + EOS.result_str(result))
		return false
	print("Connecting to " + lobby_id)
	multiplayer.multiplayer_peer = peer
	local_lobby = lobby
	return true

func leave_lobby():
	if peer:
		peer.close()
	
	if multiplayer.multiplayer_peer:
		multiplayer.multiplayer_peer = null
	if is_host and local_lobby:
		await local_lobby.destroy_async()
	elif local_lobby:
		await local_lobby.leave_async()

func _on_peer_connected(peer_id : int):
	print("Player %d connected", peer_id)
	peer_user_ids.append(peer_id)
	pass
	
func _on_peer_disconnected(peer_id : int):
	print("Player %d disconnected", peer_id)
	peer_user_ids.erase(peer_id)
	pass
