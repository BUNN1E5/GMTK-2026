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

var window : Window
var collision_polygon : PackedVector2Array

func _draw() -> void:
	if collision_polygon.is_empty():
		return
		
	# 1. Recreate the offset so we are drawing in local coordinate space
	var offset := Vector2.ZERO
	if body and body.texture and body.centered:
		offset = -Vector2(body.texture.get_size()) / 2.0

	# 2. Apply the offset to the base polygon
	var local_poly := PackedVector2Array()
	for point in collision_polygon:
		local_poly.append(point + offset)

	# 3. Close the loop (draw_polyline needs the first point at the end to close the shape)
	local_poly.append(local_poly[0])

	# 4. Draw a semi-transparent red fill and a solid red outline
	draw_polygon(local_poly, PackedColorArray([Color(1, 0, 0, 0.4)]))
	draw_polyline(local_poly, Color.RED, 2.0)

func _ready() -> void:
	#FIXME :: This will prob cause issues lol
	if window: # Window already exists, so lets close it first
		window.queue_free()
	#if multiplayer.get_unique_id() == get_multiplayer_authority():
	#	window = get_window()
	#else:	
	window = Window.new()
	window.position = get_window().position
	window.mode = Window.MODE_MAXIMIZED
	window.name = "Window  " + str(get_multiplayer_authority())
	
	
	self.get_parent().add_child(window)
	self.get_parent().remove_child(self)

	
	window.transparent = true
	#window.content_scale_stretch = Window.CONTENT_SCALE_STRETCH_FRACTIONAL
	window.content_scale_aspect = Window.CONTENT_SCALE_ASPECT_KEEP_HEIGHT
	window.content_scale_mode = Window.CONTENT_SCALE_MODE_CANVAS_ITEMS
	window.content_scale_size = DisplayServer.screen_get_size(DisplayServer.window_get_current_screen())/1.5
	#window.mouse_passthrough = true
	window.always_on_top = true
	window.borderless = true
	print(DisplayServer.get_name())
	
	window.add_child(self)
	
	self.position = Desktop.bl_screen_pos(window, get_size()/2 + Vector2(10 * (1 - get_multiplayer_authority()), 0))
	
	collision_polygon = get_collision_polygon()
	update_mouse_passthru()
	pass


# for debugging purposes only
func _input(event: InputEvent) -> void:
	if(event.is_action_pressed("ui_accept")):
		player_data.costume_data.randomize_all_costumes()
		update_costume(player_data.costume_data)
	pass

func update_mouse_passthru():
	var translated_polygon = translate_polygon_to_window(collision_polygon)
	window.mouse_passthrough_polygon = translated_polygon
	queue_redraw()
	pass

func update_costume(costume_data : CostumeData):
	#body.texture = player_data.costume_data.get_costume_texture("body")
	clothing.texture = costume_data.get_costume_texture("clothing")
	ears.texture = costume_data.get_costume_texture("ears")
	eyes.texture = costume_data.get_costume_texture("eyes")
	hair.texture = costume_data.get_costume_texture("hair")
	mouth.texture = costume_data.get_costume_texture("mouth")
	
	collision_polygon = get_collision_polygon()
	update_mouse_passthru()
	
	pass

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

	update_costume(player_data.costume_data)
	update_class(player_data.class_data)
	
	clothing.z_index = 10 * get_multiplayer_authority()
	ears.z_index = 10 * get_multiplayer_authority()
	eyes.z_index = 10 * get_multiplayer_authority()
	hair.z_index = 10 * get_multiplayer_authority()
	mouth.z_index = 10 * get_multiplayer_authority()
	arm.z_index = 10 * get_multiplayer_authority()
	

func get_size():
	return body.get_rect().size * transform.get_scale()

func get_collision_polygon():
	var active_textures: Array[Texture2D] = []
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
	print("%d | Total Clicks %d" % [multiplayer.get_unique_id(), player_data.total_clicks])
	PlayerData.save(player_data, false)
	if not player_data.class_data:
		return
	player_data.class_data.primary_action(self)
	pass
	
