@tool
extends TabContainer

@export_tool_button("Stretch Tabs", "Callable") var stretch_tabs = _stretch_tabs_to_fill

func _ready() -> void:
	# Set alignment to expand tabs across the width
	tab_alignment = TabBar.ALIGNMENT_LEFT
	
	await get_tree().process_frame
	_stretch_tabs_to_fill()
	get_viewport().size_changed.connect(_stretch_tabs_to_fill)

func _stretch_tabs_to_fill() -> void:
	var num_tabs = get_tab_count()
	if num_tabs == 0:
		return

	# Force tabs to expand evenly
	var tab_bar = get_tab_bar()
	if tab_bar:
		tab_bar.tab_alignment = TabBar.ALIGNMENT_MAX
		# Enable tab expansion
		for i in range(num_tabs):
			tab_bar.set_tab_title(i, get_tab_title(i)) # Refreshes layout
