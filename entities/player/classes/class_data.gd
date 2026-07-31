extends Resource
class_name ClassData


const MOVES_PATH := "res://entities/player/moves/"

#Class accessories
@export var move_id : String
var move : Script

func primary_action(player : Player):
	player.arm.stop()
	player.arm.play("default")
	if not move == null: # If we arent the basic move do this
		move.perform(player)
		return
	# Basic default implementation of a move
	pass

func get_move() -> SpriteFrames:
	var path = "%s%s.tres" % [MOVES_PATH, move_id]
	if not ResourceLoader.exists(path):
		printerr("Uh oh we tried to access %s directory which doesnt exist" % path)
		return load("%stemplate_arm.tres" % [MOVES_PATH])
	var script_path = "%s%s.gd" % [MOVES_PATH, move_id]
	if ResourceLoader.exists(script_path):
		move = load(script_path) as Script
	else:
		printerr("We did not find a script for " % [move_id])
	return load(path)
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
