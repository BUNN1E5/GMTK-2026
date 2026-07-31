@tool
extends Node2D
class_name Player

var player_data : PlayerData

@onready var body: Sprite2D = $Body
@onready var clothing: Sprite2D = $Clothing
@onready var ears: Sprite2D = $Ears
@onready var eyes: Sprite2D = $Eyes
@onready var hair: Sprite2D = $Hair
@onready var mouth: Sprite2D = $Mouth
@onready var arm : AnimatedSprite2D = $Arm

@export var randomize_costume = false
func _randomize_costume():
	player_data.costume_data.randomize_all_costumes()
	update_costume_dict.rpc(player_data.costume_data.to_dict())

var last_transform = self.transform
func _process(delta: float) -> void:
	if randomize_costume:
		_randomize_costume()
		randomize_costume = false
	if not last_transform == transform:
		last_transform = transform
		update_mouse_passthru()
		if is_multiplayer_authority():
			set_remote_transform.rpc(transform, window.content_scale_size)
		pass
	

var window : Window
var collision_polygon : PackedVector2Array

func _ready() -> void:
	self.position = Desktop.bl_screen_pos(window, get_size()/2 + Vector2(10 * (1 - get_multiplayer_authority() % 10), 0))

func initalize() -> void:
	body = $Body
	clothing = $Clothing
	ears = $Ears
	eyes = $Eyes
	hair = $Hair
	mouth = $Mouth
	arm = $Arm
	pass

func initalize_window(uuid) -> void:
	if window: # Window already exists, so lets close it first
		window.queue_free()
	#if multiplayer.get_unique_id() == get_multiplayer_authority():
	#	window = get_window()
	#else:	
	window = Window.new()
	if(get_window()):
		window.position = get_window().position
	window.mode = Window.MODE_MAXIMIZED
	
	window.transparent = true
	#window.content_scale_stretch = Window.CONTENT_SCALE_STRETCH_FRACTIONAL
	window.content_scale_aspect = Window.CONTENT_SCALE_ASPECT_KEEP_HEIGHT
	window.content_scale_mode = Window.CONTENT_SCALE_MODE_CANVAS_ITEMS
	window.content_scale_size = DisplayServer.screen_get_size(DisplayServer.window_get_current_screen())/1.5
	#window.mouse_passthrough = true
	window.always_on_top = true
	window.borderless = true
	window.name = "Window " + uuid
	window.add_child(self)
	
	if OS.has_feature("windows"):
		arm.frame_changed.connect(update_mouse_passthru)
	pass
	set_notify_transform(true)
	
@rpc("authority", "call_local", "unreliable", 0)
func set_remote_transform(transform : Transform2D, screen_scale):
	var scaling_factor =  Vector2(screen_scale) / Vector2(window.content_scale_size)
	print(scaling_factor)
	#var scaling_factor =  Vector2(3440, 1440) / Vector2(window.content_scale_size)
	transform.origin *= scaling_factor
	self.transform = transform
	pass

func update_mouse_passthru():
	var translated_polygon = translate_polygon_to_window(collision_polygon)
	window.mouse_passthrough_polygon = translated_polygon
	queue_redraw()
	pass


func update_costume(costume_data : CostumeData):
	print("Updating Costume")
	#body.texture = player_data.costume_data.get_costume_texture("body")
	clothing.texture = costume_data.get_costume_texture("clothing")
	ears.texture = costume_data.get_costume_texture("ears")
	eyes.texture = costume_data.get_costume_texture("eyes")
	hair.texture = costume_data.get_costume_texture("hair")
	mouth.texture = costume_data.get_costume_texture("mouth")
	
	collision_polygon = get_collision_polygon()
	update_mouse_passthru()
	pass

@rpc("authority", "call_local", "reliable", 0)
func update_costume_dict(costume_data : Dictionary):
	var _costume_data = CostumeData.from_dict(costume_data)
	update_costume(_costume_data)


func update_class(class_data : ClassData):
	arm.sprite_frames = class_data.get_move()
	pass

func apply_player_data(player_data : PlayerData):
	self.player_data = player_data
	if GlobalInput.global_input_event_ex.is_connected(global_input):
		GlobalInput.global_input_event_ex.disconnect(global_input)
	if is_multiplayer_authority():
		GlobalInput.global_input_event_ex.connect(global_input)
	
	if not player_data.costume_data:
		printerr("Player %s Does not have any costume data", player_data.name)
		return

	if window:
		window.name = "Window " + player_data.uuid

	update_costume(player_data.costume_data)
	update_class(player_data.class_data)
	
	var z_index = 4096
	if not is_multiplayer_authority():
		z_index = randi_range(0, 4095)
	clothing.z_index = z_index
	ears.z_index = z_index
	eyes.z_index = z_index
	hair.z_index = z_index
	mouth.z_index = z_index
	arm.z_index = z_index

func get_size():
	return body.get_rect().size * transform.get_scale()

func get_collision_polygon():
	var active_textures: Array[Texture2D] = []
	if arm.sprite_frames:
		var anim_tex = arm.sprite_frames.get_frame_texture(arm.animation, arm.frame)
		active_textures.append(anim_tex)
	for sprite in [body, clothing, ears, hair]:
		active_textures.append(sprite.texture)
	#active_textures.append(body.texture)
	#active_textures.append(hair.texture)
	
	if active_textures.is_empty():
		return []
	
	var base_size = active_textures[0].get_size() as Vector2i
	var master_image = Image.create_empty(base_size.x, base_size.y, false, Image.FORMAT_RGBA8)
	var rect = Rect2i(Vector2i.ZERO, base_size)
	
	for tex in active_textures:
		var img = tex.get_image()
		if img.get_size() == base_size:
			master_image.blend_rect(img, rect, Vector2i.ZERO)
		else:
			img.resize(base_size.x, base_size.y)
			master_image.blend_rect(img, rect, Vector2i.ZERO)
	var bm = BitMap.new()
	bm.create_from_image_alpha(master_image)
	
	var polygons = bm.opaque_to_polygons(Rect2i(Vector2i.ZERO, base_size))
	
	if polygons.is_empty():
		return PackedVector2Array()
	
	var biggest_poly = polygons[0]
	for poly in polygons:
		if poly.size() > biggest_poly.size():
			biggest_poly = poly
	
	return biggest_poly

## AI Generated slop, no idea if this works lmai
func translate_polygon_to_window(base_polygon: PackedVector2Array) -> PackedVector2Array:
	var offset := Vector2.ZERO
	if body and body.texture and body.centered:
		offset = -Vector2(body.texture.get_size()) / 2.0 

	var translated := PackedVector2Array()
	translated.resize(base_polygon.size())
	var local_to_window_pixels = get_viewport().get_final_transform() * get_global_transform()
	for i in range(base_polygon.size()):
		# 1. Shift from top-left texture origin to node center
		# 2. Apply Node2D transform (position, scale, rotation) to move into window space
		translated[i] = local_to_window_pixels * (base_polygon[i] + offset)

	return translated

#The global input event does work
#however it does not have the attributes associated with an input event
#such as is pressed
#We dont actually care about that, so we can kinda just assume that
#we only care about every OTHER input
#OR we only care about unique inputs
var last : Dictionary[String, bool]

func global_input(event):
	if event == null:
		printerr("event is null, somthing is really wrong")
		return
	
	var key = event.GodotInputEventNames[0]
	if(not last.has(key)):
		last.set(key, event.pressed)	
	if(last[key] == false && event.pressed == true):
		primary_action.rpc()
	last[key] = event.pressed
	#if event is InputEventMouseMotion:
	#	return
	pass

@rpc("any_peer", "call_local", "unreliable_ordered", 0)
func primary_action():
	if not player_data:
		print("No Player Data")
		return
	
	player_data.total_clicks+=1
	#print("%d | Total Clicks %d" % [multiplayer.get_unique_id(), player_data.total_clicks])
	if is_multiplayer_authority():
		PlayerData.save(player_data, false)
	if not player_data.class_data:
		return
	player_data.class_data.primary_action(self)
	pass
	
