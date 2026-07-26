extends Node2D
const MAX_HP:int = 1000

@onready var cactus_health_bar = $cactusGuy/cactusHealthBar
@onready var slime_health_bar = $slimeGuy/slimeHealthBar

var cactusHP:int
var slimeHP:int
var slimeText:String
var cactusText:String

func _ready():
	cactusHP = cactus_health_bar.value
	slimeHP = slime_health_bar.value
	slimeText = "hp:%s/%s" % [slimeHP, MAX_HP]
	cactusText = "hp:%s/%s" % [cactusHP, MAX_HP]

	$cactusGuy/cactusHealthBar/Label2.text = cactusText
	$slimeGuy/slimeHealthBar/Label.text = slimeText
	$cactusGuy.play("wiggle")
	$slimeGuy.play("hop")

func _input(event: InputEvent) -> void:
	if event.is_pressed():
		damage()

func damage():
	
	cactus_health_bar.value -= 1
	slime_health_bar.value -= 1
	cactusHP = cactus_health_bar.value
	slimeHP = slime_health_bar.value
	$cactusGuy/cactusHealthBar/Label2.text = "hp:%s/%s" % [cactusHP, MAX_HP]
	$slimeGuy/slimeHealthBar/Label.text = "hp:%s/%s" % [slimeHP, MAX_HP]
