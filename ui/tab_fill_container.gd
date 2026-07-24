@tool
extends TabContainer

# AI GENERATED FIX CLEANUP Later

@export_tool_button("Stretch Tabs", "Callable") var stretch_tabs = _stretch_tabs_to_fill

func _ready() -> void:
	# Wait one frame for the layout boundary sizing to finish calculating
	await get_tree().process_frame
	_stretch_tabs_to_fill()
	
	# Optional: Recalculate if the window size changes
	get_viewport().size_changed.connect(_stretch_tabs_to_fill)

func _stretch_tabs_to_fill() -> void:
	print("Stretching Tabs")
	var num_tabs = get_tab_count()
	if num_tabs == 0:
		return
		
	# 1. Find out how much total horizontal space we have
	var total_width = size.x
	
	# 2. Measure the default width of all tabs combined (based on text)
	var accumulated_text_width = 0.0
	for i in range(num_tabs):
		# We use the font to see how wide the raw text string is
		var tab_title = get_tab_title(i)
		var font = get_theme_font("font")
		var font_size = get_theme_font_size("font_size")
		accumulated_text_width += font.get_string_size(tab_title, HORIZONTAL_ALIGNMENT_LEFT, -1, font_size).x

	# 3. Figure out how much extra empty space needs to be split up
	var remaining_space = total_width - accumulated_text_width
	
	if remaining_space > 0:
		# Divide the blank space by the number of tabs, then split into left/right margins
		var extra_padding_per_tab = (remaining_space / num_tabs) / 2.0
		
		# Apply this extra padding directly into the Theme Styleboxes
		var styles = ["tab_selected", "tab_unselected", "tab_hovered", "tab_disabled"]
		for style_name in styles:
			var style_box = get_theme_stylebox(style_name).duplicate() as StyleBoxFlat
			if style_box:
				style_box.content_margin_left = extra_padding_per_tab
				style_box.content_margin_right = extra_padding_per_tab
				add_theme_stylebox_override(style_name, style_box)
