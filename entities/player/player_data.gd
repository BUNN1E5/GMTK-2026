extends Resource

#Basic Player Stats
@export var name : String
@export var total_clicks : int #64 bit int

@export var str : float = 1 # Effects DPC/DPT (Damage per Click/Type)
@export var dex : float = 1 # Effects Dodge chance?
@export var wil : float = 1 # Effects Constitution (HP)
@export var luk : float = 1 # Crit Chance%
@export var foc : float = 1 # Crit Damage%

var char_class : ClassData

#Accesories


func _init():
	pass
