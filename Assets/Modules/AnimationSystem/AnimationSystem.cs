using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AnimationSystem : ILayeredAnimationPlayer
{
    private const string BASE_LAYER_NAME = "Base Layer";

    public int LayerCount { get; }
    public AnimationState currentAnimationState => GetLayer(0).currentAnimationState;

    private PlayableGraph _graph;
    private AnimationPlayableOutput _output;
    private PlayableIndexer<AnimationLayerMixerPlayable, AnimationLayer> _layers;

    public AnimationSystem(Animator animator)
    {
        var name = $"{animator.gameObject.name}-{animator.gameObject.GetEntityId()}.AnimationSystem";

        _graph = PlayableGraph.Create(name);
        _output = AnimationPlayableOutput.Create(_graph, name + ".Output", animator);
        _layers = new(AnimationLayerMixerPlayable.Create(_graph));
        _output.SetSourcePlayable(_layers.playable);

        _graph.Play();

        AddLayer(new(BASE_LAYER_NAME));
    }

    public AnimationSystem(Animator animator, IEnumerable<AnimationLayerDescriptor> additionalLayers) : this(animator)
    {
        foreach (var layer in additionalLayers) AddLayer(layer);
    }

    #region Animation Methods
    public SimpleAnimationState Play(SimpleAnimationStateDescriptor descriptor, float fadeSpeed = 0f)
    {
        return GetLayer(0).Play(descriptor, fadeSpeed);
    }

    public LinearAnimationState Play(LinearAnimationStateDescriptor descriptor, float fadeSpeed = 0f)
    {
        return GetLayer(0).Play(descriptor, fadeSpeed);
    }

    public IDAnimationState Play(IDAnimationStateDescriptor descriptor, float fadeDuration = 0)
    {
        return GetLayer(0).Play(descriptor, fadeDuration);
    }

    public GBIAnimationState Play(GBIAnimationStateDescriptor descriptor, float fadeDuration = 0)
    {
        return GetLayer(0).Play(descriptor, fadeDuration);
    }

    public PGBIAnimationState Play(PGBIAnimationStateDescriptor descriptor, float fadeDuration = 0)
    {
        return GetLayer(0).Play(descriptor, fadeDuration);
    }
    #endregion

    #region Layer Methods
    public AnimationLayer AddLayer(AnimationLayerDescriptor descriptor)
    {
        if (LayerExists(descriptor.name))
            throw new InvalidOperationException($"There already is a layer with name {descriptor.name}");
        var layer = _layers.Add((owner, index) => new(owner, index, descriptor));
        //Connect with empty connector
        layer.Connect(new AnimationClipConnector(_graph, new()));

        return layer;
    }

    public void RemoveLayer(int index)
    {
        if (index == 0) throw new InvalidOperationException("Cannot remove the base layer.");
        _layers.Remove(index);
    }

    public void RemoveLayer(string name)
    {
        var index = FindIndexByName(name);
        if (index < 0) throw new KeyNotFoundException($"There is no layer with name {name}");
        RemoveLayer(index);
    }

    public AnimationLayer GetLayer(int index)
    {
        return _layers[index];
    }

    public AnimationLayer GetLayer(string name)
    {
        var index = FindIndexByName(name);
        if (index < 0) return null;
        return GetLayer(index);
    }

    public IEnumerable<AnimationLayer> GetLayers()
    {
        return _layers;
    }

    private int FindIndexByName(string name)
    {
        for (int i = 0; i < _layers.Count; i++)
        {
            var layer = _layers[i];
            if (layer.name == name) return i;
        }
        return -1;
    }

    private bool LayerExists(string name)
    {
        return FindIndexByName(name) >= 0;
    }
    #endregion

    public void Destroy()
    {
        if (_graph.IsValid())
            _graph.Destroy();
    }

}