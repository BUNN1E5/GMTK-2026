@tool
extends EditorPlugin


func _enable_plugin() -> void:
	# Add autoloads here.
	add_autoload_singleton("GlobalInput", "res://addons/global_input/autoloads/gdscript/global_input.gd")
	pass


func _disable_plugin() -> void:
	# Remove autoloads here.
	remove_autoload_singleton("GlobalInput")
	pass


func _enter_tree() -> void:
	# Initialization of the plugin goes here.
	pass


func _exit_tree() -> void:
	# Clean-up of the plugin goes here.
	pass
