extends Node
class_name Player

var player_data : PlayerData

func _ready() -> void:
	GlobalInput.global_input_event.connect(global_input)
	pass

static func create_payer(peer_id : int, is_local: bool, player_data : PlayerData) -> Player:
	var p = Player.new()
	p.player_data = player_data
	p.set_multiplayer_authority(peer_id)
	p.is_local = is_local
	p.name = player_data.uuid
	return p

func _notification(what: int) -> void:
	if what == NOTIFICATION_WM_CLOSE_REQUEST:
		PlayerData.save(player_data)
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
		primary_action.rpc()
	last_event = event
	pass

@rpc("any_peer", "call_local", "unreliable_ordered", 0)
func primary_action():
	print("%d Did Primary Action", multiplayer.get_unique_id())
	if player_data and player_data.class_data:
		player_data.class_data.primary_action(self)
	pass
	
