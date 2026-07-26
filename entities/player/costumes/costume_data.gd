extends Resource
class_name CostumeData

@export var clothing : String = "template_clothing"
@export var hat : String = "template_hat"
@export var accessory : String = "template_accessory"
@export var ears : String = "template_ears"
@export var hair : String = "template_hair"
@export var eyes : String = "template_eyes"
@export var mouth : String = "template_mouth"

const COSTUME_PATH = "res://entities/player/costumes/"

func get_costume_texture(property : String) -> Texture2D:
	var path = "%s%s/%s.png" % [COSTUME_PATH, property, get(property)]
	if not ResourceLoader.exists(path):
		printerr("Uh oh we tried to access a costume directory that didnt exist")
		return load("%s%s/template_%s.png" % [COSTUME_PATH, property, property])
	return load(path)	

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
