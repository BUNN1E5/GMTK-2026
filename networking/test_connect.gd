extends Node

@onready var local_user_id_display = $TextEdit2
@onready var lobby_id_display = $TextEdit
@onready var connect_button = $Button
@onready var create_lobby_button = $Button2

func _ready() -> void:
	connect_button.pressed.connect(_on_connect_button_pressed)
	create_lobby_button.pressed.connect(_on_create_lobby_button_pressed)

func _on_connect_button_pressed() -> void:
	var result = await PeerConnect.join_lobby(lobby_id_display.text)
	local_user_id_display.text = "Connected" if result else "Failed"
	pass

func _on_create_lobby_button_pressed() -> void:
	await PeerConnect.create_lobby()
	await get_tree().create_timer(1.0).timeout
	local_user_id_display.text = PeerConnect.local_user_id
	pass
