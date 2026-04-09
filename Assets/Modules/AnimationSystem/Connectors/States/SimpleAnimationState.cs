using UnityEngine;
using UnityEngine.Playables;

public class SimpleAnimationStateDescriptor
{
    public AnimationClip animationClip => _animationClip;
    public float speed => _speed;
    public float motionTime => _motionTime;
    public bool playing => _playing;

    private AnimationClip _animationClip;
    private float _speed;
    private float _motionTime;
    private bool _playing;

    public SimpleAnimationStateDescriptor(AnimationClip animationClip, float speed = 1f,
        float motionTime = 0f, bool playing = true)
    {
        _animationClip = animationClip;
        _speed = speed;
        _motionTime = motionTime;
        _playing = playing;
    }
}

public class SimpleAnimationState : AnimationState
{
    public AnimationClip animationClip => _animationClipConnector.animationClip;
    public override float speed
    {
        get => _animationClipConnector.speed;
        set => _animationClipConnector.speed = value;
    }

    public override float motionTime
    {
        get => _animationClipConnector.motionTime;
        set => _animationClipConnector.motionTime = value;
    }
    public override bool playing
    {
        get => _animationClipConnector.playing;
        set => _animationClipConnector.playing = value;
    }

    internal override Playable rootPlayable => _animationClipConnector.rootPlayable;

    private AnimationClipConnector _animationClipConnector;

    public SimpleAnimationState(PlayableGraph graph, SimpleAnimationStateDescriptor descriptor)
    {
        _animationClipConnector = new AnimationClipConnector(graph, descriptor.animationClip) {
            speed = descriptor.speed,
            motionTime = descriptor.motionTime,
            playing = descriptor.playing
        };
    }

    internal override void Destroy()
    {
        if (_animationClipConnector.isValid)
            _animationClipConnector.Destroy();
    }
}