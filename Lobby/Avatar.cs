using Godot;
using System;

[Tool]
public partial class Avatar : TextureRect
{
	[Export] public SpriteFrames AvatarFrames;
	[Export] public string CurrentAnimation = "default";
	[Export] public int FrameIndex = 0;
	[Export] public float AnimationSpeed = 1.0f;
	[Export] public bool IsPlaying = false;
	[Export] public bool AutoPlay = false;

	private float _refreshRate = 1.0f;
	private double _fps = 30.0d;
	private int _frameDelta = 0;
	
	public override void _Ready()
	{
		_fps = AvatarFrames.GetAnimationSpeed(CurrentAnimation);
		_refreshRate = AvatarFrames.GetFrameDuration(CurrentAnimation, FrameIndex);
		
		if (AutoPlay) Play();
	}

	public override void _Process(double delta)
	{
		if (AvatarFrames == null || !IsPlaying) return;
		if (AvatarFrames.HasAnimation(CurrentAnimation) == false) IsPlaying = false;
		GetAnimationData(CurrentAnimation);
		_frameDelta += (int)(AnimationSpeed * delta);
		GD.Print(CurrentAnimation);
		GD.Print("_frameDelta" + _frameDelta);
		GD.Print(_refreshRate / _fps);
		GD.Print("_refreshRate" + _refreshRate);
		GD.Print("_fps" + _fps);
		if (_frameDelta >= _refreshRate / _fps)
		{
			Texture = GetNextFrame();
			_frameDelta = 0;
		}
	}
	
	private void Play(string currentAnimation = "default")
	{
		FrameIndex = 0;
		_frameDelta = 0;
		CurrentAnimation = currentAnimation;
		GetAnimationData(currentAnimation);
		IsPlaying = true;
	}

	private void GetAnimationData(string currentAnimation)
	{
		_fps = AvatarFrames.GetAnimationSpeed(currentAnimation);
		_refreshRate = AvatarFrames.GetFrameDuration(currentAnimation, FrameIndex);
	}
	
	private void Resume() => IsPlaying = true;
	
	private void Pause() => IsPlaying = false;

	private void Stop()
	{
		FrameIndex = 0;
		IsPlaying = false;
	}
	
	private Texture2D GetNextFrame()
	{
		var frameCount = AvatarFrames.GetFrameCount(CurrentAnimation);
		FrameIndex++;
		if (FrameIndex >= frameCount)
		{
			FrameIndex = 0;
			if (!AvatarFrames.GetAnimationLoop(CurrentAnimation)) IsPlaying = false;
		}
		GetAnimationData(CurrentAnimation);
		return AvatarFrames.GetFrameTexture(CurrentAnimation, FrameIndex);
	}
}
