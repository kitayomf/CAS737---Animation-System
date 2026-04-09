using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[Serializable]
public struct BlendPoint<T>
    where T : struct
{
    public T position => _position;
    public AnimationClip animationClip => _animationClip;
    
    [SerializeField] private T _position;
    [SerializeField] private AnimationClip _animationClip;

    public BlendPoint(T position, AnimationClip animationClip)
    {
        _animationClip = animationClip;
        _position = position;
    }
}

[Serializable]
public abstract class BlendMap<T> : IEnumerable<BlendPoint<T>>
    where T : struct
{
    public int pointCount => _pointList.Count;

    public BlendPoint<T> this[int i]
    {
        get
        {
            return _pointList[i];
        }
        set
        {
            _pointList[i] = value;
        }
    }

    [SerializeField] private List<BlendPoint<T>> _pointList = new();

    public BlendMap() { }

    public BlendMap(IEnumerable<BlendPoint<T>> points)
    {
        _pointList = points.ToList();
    }

    public void AddPoint(BlendPoint<T> point)
    {
        _pointList.Add(point);
    }

    public void RemovePoint(int index)
    {
        _pointList.RemoveAt(index);
    }

    public IEnumerator<BlendPoint<T>> GetEnumerator()
    {
        return _pointList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

[Serializable]
public class LinearBlendMap : BlendMap<float> { }

[Serializable]
public class PlanarBlendMap : BlendMap<Vector2> { }

public abstract class BlendAnimationState<T> : AnimationState
    where T : struct
{
    internal sealed override Playable rootPlayable => _mixer.playable;

    public override float speed
    {
        get => (float)_mixer.playable.GetSpeed();
        set => _mixer.playable.SetSpeed(value);
    }
    public override float motionTime
    {
        get => (float)_mixer.playable.GetTime();
        set
        {
            _mixer.playable.SetTime(value);
            foreach (var outlet in _mixer)
            {
                if (outlet.currentConnector is AnimationClipConnector clipConnector)
                    clipConnector.motionTime = value;
            }
        }
    }
    public override bool playing
    {
        get => _mixer.playable.GetPlayState() == PlayState.Playing;
        set
        {
            if (value) _mixer.playable.Play();
            else _mixer.playable.Pause();
        }
    }
    public T referencePoint
    {
        get => _referencePoint;
        set
        {
            if (!_referencePoint.Equals(value))
            {
                _referencePoint = value;
                _UpdateWeights();
            }
        }
    }

    private PlayableIndexer<AnimationMixerPlayable, MixerOutlet> _mixer;
    private List<BlendPoint<T>> _points;
    private T _referencePoint;

    public BlendAnimationState(PlayableGraph graph, BlendMap<T> blendMap, T initialReferencePoint = default)
    {
        _mixer = new(AnimationMixerPlayable.Create(graph));
        _points = new();
        _referencePoint = initialReferencePoint;

        _Build(blendMap);

        _UpdateWeights();
    }

    protected void SetWeight(int index, float weight)
    {
        _mixer[index].weight = Mathf.Clamp01(weight);
    }

    protected abstract void UpdateWeights(T referencePoint, IEnumerable<BlendPoint<T>> points);

    internal override void Destroy()
    {
        _mixer.Destroy();
    }

    private void _Build(IEnumerable<BlendPoint<T>> points)
    {
        foreach (var point in points)
        {
            _points.Add(point);
            _mixer.Add((owner, index) => new(owner, index))
                .Connect(new AnimationClipConnector(_mixer.playable.GetGraph(), point.animationClip)
                {
                    speed = 1f,
                    motionTime = (float)_mixer.playable.GetTime(),
                    playing = true
                });
        }
    }

    private void _UpdateWeights()
    {
        UpdateWeights(_referencePoint, _points);
    }
}