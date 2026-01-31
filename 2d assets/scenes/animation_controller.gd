extends AnimatedSprite3D

var player


func _ready() -> void:
	_play_idle()

func _play_idle() -> void:
	play("idle")

func _play_walk() -> void:
	play("walk")
	
func _play_atk() -> void:
	play("atk")

func _play_shield() -> void:
	pass

func _play_dash() -> void:
	pass

func _play_jump() -> void:
	pass

func _play_death() -> void:
	pass
