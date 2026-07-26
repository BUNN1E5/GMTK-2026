extends Resource
class_name PlayerData

#Basic Player Stats
@export var name : String
@export var uuid : String
@export var total_clicks : int #64 bit int
@export var exp : int
@export var exp_req : int
@export var lvl : int

@export var str : float = 1 # Effects DPC/DPT (Damage per Click/Type)
@export var dex : float = 1 # Effects Dodge chance?
@export var wil : float = 1 # Effects Constitution (HP)
@export var luk : float = 1 # Crit Chance%
@export var foc : float = 1 # Crit Damage%

@export var class_data : ClassData
@export var costume_data : CostumeData

func _init():
	uuid = UUID.v4()
	if not class_data:
		class_data = ClassData.new()
	if not costume_data:
		costume_data = CostumeData.new()
	pass


func to_dict() -> Dictionary:
	return {
		"name": name,
		"uuid": uuid,
		"total_clicks": total_clicks,
		"exp": exp,
		"exp_req": exp_req,
		"lvl": lvl,
		"str": str,
		"dex": dex,
		"wil": wil,
		"luk": luk,
		"foc": foc,
		"class_data": class_data.to_dict() if class_data else null,
		"costume_data": costume_data.to_dict() if costume_data else null
	}


static func from_dict(dict: Dictionary) -> PlayerData:
	var pdata = PlayerData.new()
	if dict.is_empty():
		return pdata

	pdata.name = dict.get("name", "")
	pdata.uuid = dict.get("uuid", "")
	pdata.total_clicks = dict.get("total_clicks", 0)
	pdata.exp = dict.get("exp", 0)
	pdata.exp_req = dict.get("exp_req", 0)
	pdata.lvl = dict.get("lvl", 1)

	pdata.str = dict.get("str", 1.0)
	pdata.dex = dict.get("dex", 1.0)
	pdata.wil = dict.get("wil", 1.0)
	pdata.luk = dict.get("luk", 1.0)
	pdata.foc = dict.get("foc", 1.0)

	if dict.get("class_data") != null and dict["class_data"] is Dictionary:
		pdata.class_data = ClassData.from_dict(dict["class_data"])

	if dict.get("costume_data") != null and dict["costume_data"] is Dictionary:
		pdata.costume_data = CostumeData.from_dict(dict["costume_data"])
	return pdata
	
static func load() -> PlayerData:
	print("Loading Player Data")
	if(ResourceLoader.exists("user://savedata.tres")):
		print("Found Player Data")
		return ResourceLoader.load("user://savedata.tres") as PlayerData
	return null

#TODO :: Potentially allow multiple characters?
static func save(player_data : PlayerData):
	print("Saving Player Data")
	ResourceSaver.save(player_data, "user://savedata.tres")
