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

static func load() -> PlayerData:
	return ResourceLoader.load("user://savedata.tres") as PlayerData

#TODO :: Potentially allow multiple characters?
static func save(player_data : PlayerData):
	ResourceSaver.save(player_data, "user://savedata.tres")
