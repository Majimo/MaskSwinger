extends Sprite3D

@export var bpm: int = 65
var tween: Tween

func _on_texture_changed() -> void:
	scale = Vector3(0.2, 0.2, 0.2)

func _ready() -> void:
	
	var delay: float = float(bpm) / 60.0
	
	if tween:
		tween.kill() # Abort the previous animation.
	
	tween = create_tween()
	tween.tween_property(self, "position", Vector3(0,3,0), delay)
	tween.tween_property(self, "position", Vector3(0,1,0), delay)
	tween.set_loops()
