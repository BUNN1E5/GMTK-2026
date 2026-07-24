extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	$cactusGuy.play("wiggle")
	$slimeGuy.play("hop")
	pass 

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
