extends AnimatedSprite3D

var player

enum animation_side {
	FACE,
	RIGHT,
	LEFT,
	BACK
}

func _ready() -> void:
	_play_idle(animation_side.FACE)

func _play_idle(side: animation_side) -> void:
	flip_h = false
	match side:
		animation_side.FACE:
			play("face_walk")
		animation_side.RIGHT:
			play("side_walk")
		animation_side.LEFT:
			flip_h = true
			play("side_walk")
		animation_side.BACK:
			play("back_walk")

func _play_walk(side: animation_side) -> void:
	flip_h = false
	match side:
		animation_side.FACE:
			play("face_walk")
		animation_side.RIGHT:
			play("side_walk")
		animation_side.LEFT:
			flip_h = true
			play("side_walk")
		animation_side.BACK:
			play("back_walk")

	
func _play_atk(side: animation_side) -> void:
	flip_h = false
	match side:
		animation_side.FACE:
			play("face_atk")
		animation_side.RIGHT:
			play("side_atk")
		animation_side.LEFT:
			flip_h = true
			play("side_atk")
		animation_side.BACK:
			play("back_atk")


func _play_shield(side: animation_side) -> void:
	flip_h = false
	match side:
		animation_side.FACE:
			#play("face_shield")
			pass
		animation_side.RIGHT:
			#play("side_shield")
			pass
		animation_side.LEFT:
			flip_h = true
			#play("side_shield")
			pass
		animation_side.BACK:
			#play("back_shield")
			pass

func _play_dash(side: animation_side) -> void:
	flip_h = false
	match side:
		animation_side.FACE:
			#play("face_dash")
			pass
		animation_side.RIGHT:
			#play("side_dash")
			pass
		animation_side.LEFT:
			flip_h = true
			#play("side_dash")
			pass
		animation_side.BACK:
			#play("back_dash")
			pass

func _play_jump(side: animation_side) -> void:
	flip_h = false
	match side:
		animation_side.FACE:
			#play("face_jump")
			pass
		animation_side.RIGHT:
			#play("side_jump")
			pass
		animation_side.LEFT:
			flip_h = true
			#play("side_jump")
			pass
		animation_side.BACK:
			#play("back_jump")
			pass

func _play_death() -> void:
	pass
