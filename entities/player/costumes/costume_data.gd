extends Resource
class_name CostumeData

@export var clothing : String
@export var hat : String
@export var accessory : String
@export var ears : String
@export var hair : String
@export var mouth : String
@export var eyes : String

const COSTUME_PATH = "res://entities/player/costumes/"

func to_dict() -> Dictionary:
	return {
		"clothing": clothing,
		"hat": hat,
		"accessory": accessory,
		"ears": ears,
		"hair": hair,
		"mouth": mouth,
		"eyes": eyes
	}

static func from_dict(dict: Dictionary) -> CostumeData:
	var cdata = CostumeData.new()
	if dict.is_empty():
		return cdata
		
	cdata.clothing = dict.get("clothing", "")
	cdata.hat = dict.get("hat", "")
	cdata.accessory = dict.get("accessory", "")
	cdata.ears = dict.get("ears", "")
	cdata.hair = dict.get("hair", "")
	cdata.mouth = dict.get("mouth", "")
	cdata.eyes = dict.get("eyes", "")
	
	return cdata
