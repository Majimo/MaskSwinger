extends Control

@onready var base_player_animations: PlayerAnimationController = $"../base_player_animations"


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass


func _on_idle_face_pressed() -> void:
	base_player_animations._play_idle(PlayerAnimationController.animation_side.FACE)
	pass # Replace with function body.


func _on_walk_left_pressed() -> void:
	base_player_animations._play_walk(PlayerAnimationController.animation_side.LEFT)
	pass # Replace with function body.


func _on_atk_back_pressed() -> void:
	base_player_animations._play_atk(PlayerAnimationController.animation_side.BACK)
	pass # Replace with function body.
