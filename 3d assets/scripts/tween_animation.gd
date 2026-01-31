extends Node3D

@export var bpm: int = 130
@export var scale_smol: Vector3 = Vector3(.9, .9, .9)
@export var scale_big: Vector3 = Vector3(1.1, 1.1, 1.1)
@export var random_ratio: float = 0.015
var tween: Tween


func _ready() -> void:
	randomize()
	_update_scales()
	
	var delay: float = float(bpm) / 60.0
	var half_delay: float = delay / 2.0
	
	if tween:
		tween.kill() # Abort the previous animation.
	
	tween = create_tween()
	if randf() < 0.5:
		#await half_delay
		tween.tween_property(self, "scale", scale_smol, half_delay).set_trans(Tween.TRANS_QUINT).set_ease(Tween.EASE_IN_OUT)
		tween.tween_property(self, "scale", scale_big, half_delay).set_trans(Tween.TRANS_QUINT).set_ease(Tween.EASE_IN_OUT)
	else:
		tween.tween_property(self, "scale", scale_big, half_delay).set_trans(Tween.TRANS_QUINT).set_ease(Tween.EASE_IN_OUT)
		tween.tween_property(self, "scale", scale_smol, half_delay).set_trans(Tween.TRANS_QUINT).set_ease(Tween.EASE_IN_OUT)
	tween.set_loops()


func _update_scales() -> void:
		var random_offset_smol = Vector3(
			randf_range(-random_ratio, random_ratio),
			randf_range(-random_ratio, random_ratio),
			randf_range(-random_ratio, random_ratio)
		)
		var random_offset_big = Vector3(
			randf_range(-random_ratio, random_ratio),
			randf_range(-random_ratio, random_ratio),
			randf_range(-random_ratio, random_ratio)
		)
		scale_smol = scale_smol + random_offset_smol
		scale_big = scale_big + random_offset_big
