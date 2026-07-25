extends Resource
class_name ClassData


const MOVES_PATH := "res://entities/player/moves/"
static var _move_scripts : Dictionary[String, Script]

#Class accessories
var move_id : String

func primary_action(player):
	if move_id != "": # If we arent the basic move do this
		execute_move(move_id, player)
		return
	# Basic default implementation of a move
	pass
	
func to_dict() -> Dictionary:
	return {
		"move_id": move_id,
	}

static func from_dict(dict: Dictionary) -> ClassData:
	var cdata = ClassData.new()
	if dict.is_empty():
		return cdata		
	cdata.move_id = dict.get("move_id", "")
	return cdata

#We only need to load this once every instance
static func _load_all_moves():
	var dir = DirAccess.open(MOVES_PATH)
	dir.list_dir_begin()
	var file_name := dir.get_next()
	
	while file_name != "":
		if not dir.current_is_dir() and file_name.ends_with(".gd"):
			var move_id := file_name.trim_suffix(".gd")
			var script := load(MOVES_PATH + file_name) as Script
			if script:
				_move_scripts.set(move_id, script)
		file_name = dir.get_next()

func execute_move(move_id : String, player: Player):
	if not _move_scripts.has(move_id):
		printerr("Move %s was not found", move_id)
		return
	_move_scripts[move_id].perform()
