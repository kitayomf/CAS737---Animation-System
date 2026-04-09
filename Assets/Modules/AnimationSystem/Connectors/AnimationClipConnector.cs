using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimationClipConnector : Connector
{
    internal override Playable rootPlayable => _animationClipPlayable;
    internal AnimationClip animationClip => _animationClipPlayable.GetAnimationClip();
    internal float speed
    {
        get => (float)(_animationClipPlayable.GetSpeed() / animationClip.length);
        set => _animationClipPlayable.SetSpeed(value * animationClip.length);
    }
    internal float motionTime
    {
        get => (float)(_animationClipPlayable.GetTime() / animationClip.length);
        set => _animationClipPlayable.SetTime(value * animationClip.length);
    }
    internal bool playing
    {
        get => _animationClipPlayable.GetPlayState() == PlayState.Playing;
        set
        {
            if (value) _animationClipPlayable.Play();
            else _animationClipPlayable.Pause();
        }
    }

    private AnimationClipPlayable _animationClipPlayable;

    internal AnimationClipConnector(PlayableGraph graph, AnimationClip animationClip)
    {
        _animationClipPlayable = AnimationClipPlayable.Create(graph, animationClip);

        _animationClipPlayable.SetSpeed(animationClip.length);
        if (animationClip.isLooping) _animationClipPlayable.SetDuration(float.PositiveInfinity);
        else _animationClipPlayable.SetDuration(animationClip.length);
        _animationClipPlayable.SetTime(0);
        _animationClipPlayable.Play();
    }

    internal override void Destroy()
    {
        if (_animationClipPlayable.IsValid()) _animationClipPlayable.Destroy();
    }
}