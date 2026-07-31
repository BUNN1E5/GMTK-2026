extends Control  # or whatever this node's type is

@onready var stats_button: TextureButton = $Stats
@onready var class_button: TextureButton = $Class
@onready var social_button: TextureButton = $Social
@onready var settings_button: TextureButton = $Settings

var buttons: Array[TextureButton] = []

func _ready() -> void:
	buttons = [stats_button, class_button, social_button, settings_button]

	for button in buttons:
		button.gui_input.connect(_on_button_gui_input.bind(button))

	_select_button(stats_button)


func _on_button_gui_input(event: InputEvent, button: TextureButton) -> void:
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT and event.pressed:
			print("Clicked: ", button.name)
			_select_button(button)


func _select_button(selected: TextureButton) -> void:
	for button in buttons:
		button.disabled = (button != selected)
