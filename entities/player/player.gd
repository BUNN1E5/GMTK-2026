extends Node2D
class_name Player

var player_data : PlayerData
var window : Window

@onready var body: Sprite2D = $Body
@onready var clothing: Sprite2D = $Clothing
@onready var ears: Sprite2D = $Ears
@onready var eyes: Sprite2D = $Eyes
@onready var hair: Sprite2D = $Hair
@onready var mouth: Sprite2D = $Mouth

func _ready() -> void:
	if is_multiplayer_authority():
		GlobalInput.global_input_event.connect(primary_action)
	
func apply_player_data(player_data : PlayerData):
	if GlobalInput.global_input_event.is_connected(primary_action):
		GlobalInput.global_input_event.disconnect(primary_action)
	if is_multiplayer_authority():
		GlobalInput.global_input_event.connect(primary_action)
	
	if not player_data.costume_data:
		printerr("Player %s Does not have any costume data", player_data.name)
		return
	
	#body.texture = player_data.costume_data.get_costume_texture("body")
	clothing.texture = player_data.costume_data.get_costume_texture("clothing")
	ears.texture = player_data.costume_data.get_costume_texture("ears")
	eyes.texture = player_data.costume_data.get_costume_texture("eyes")
	hair.texture = player_data.costume_data.get_costume_texture("hair")
	mouth.texture = player_data.costume_data.get_costume_texture("mouth")
	
	clothing.z_index = get_multiplayer_authority()
	ears.z_index = get_multiplayer_authority()
	eyes.z_index = get_multiplayer_authority()
	hair.z_index = get_multiplayer_authority()
	mouth.z_index = get_multiplayer_authority()

func get_collision_polygon():
	var active_textures: Array[Texture2D] = []
	for sprite in [body, clothing, ears, eyes, hair, mouth]:
		active_textures.append(sprite.texture)
	
	if active_textures.is_empty():
		return []
	
	var base_size = active_textures[0].get_size()
	var master_image = Image.create(base_size.x, base_size.y, false, Image.FORMAT_RGBA8)
	var rect = Rect2i(Vector2i.ZERO, base_size)
	
	for tex in active_textures:
		var img = tex.get_image()
		if img.get_size() == base_size:
			master_image.blit_rect(img, rect, Vector2i.ZERO)
		else:
			img.resize(base_size.x, base_size.y)
			master_image.blit_rect(img, rect, Vector2i.ZERO)
	var bm = BitMap.new()
	bm.create_from_image_alpha(master_image)
	
	var polygons = bm.opaque_to_polygons(Rect2i(Vector2i.ZERO, base_size))
	return polygons

func _notification(what: int) -> void:
	if what == NOTIFICATION_WM_CLOSE_REQUEST:
		if is_multiplayer_authority(): #Only save local player
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
	player_data.total_clicks+=1
	print("%d | Total Clicks %d", multiplayer.get_unique_id(), player_data.total_clicks)
	if player_data and player_data.class_data:
		player_data.class_data.primary_action(self)
	pass
	
