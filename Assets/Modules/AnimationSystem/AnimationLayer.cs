using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[Serializable]
public class AnimationLayerDescriptor
{
    public string name => _name;
    public float weight => _weight;
    public AvatarMask avatarMask => _avatarMask;
    public bool additive => _additive;

    [SerializeField] private string _name;
    [SerializeField] private float _weight = 0f;
    [SerializeField] private AvatarMask _avatarMask = null;
    [SerializeField] private bool _additive = false;

    public AnimationLayerDescriptor(string name, float weight = 1f, AvatarMask avatarMask = null, bool additive = false)
    {
        _name = name;
        _weight = weight;
        _avatarMask = avatarMask;
        _additive = additive;
    }
}

public class AnimationLayer : LayerOutlet, IAnimationPlayer
{
    public string name => _name;
    public AnimationState currentAnimationState
    {
        get
        {
            return _currentState;
        }
    }

    private string _name;
    private AnimationState _currentState;

    public AnimationLayer(AnimationLayerMixerPlayable owner, int index, AnimationLayerDescriptor descriptor)
        : base(owner, index)
    {
        _name = descriptor.name;
        weight = descriptor.weight;
        avatarMask = descriptor.avatarMask;
        additive = descriptor.additive;
    }

    public SimpleAnimationState Play(SimpleAnimationStateDescriptor descriptor, float fadeDuration = 0f)
    {
        return HandlePlay(new SimpleAnimationState(owner.GetGraph(), descriptor), fadeDuration);
    }

    public LinearAnimationState Play(LinearAnimationStateDescriptor descriptor, float fadeDuration = 0f)
    {
        return HandlePlay(new LinearAnimationState(owner.GetGraph(), descriptor), fadeDuration);
    }

    public IDAnimationState Play(IDAnimationStateDescriptor descriptor, float fadeDuration = 0)
    {
        return HandlePlay(new IDAnimationState(owner.GetGraph(), descriptor), fadeDuration);
    }

    public GBIAnimationState Play(GBIAnimationStateDescriptor descriptor, float fadeDuration = 0)
    {
        return HandlePlay(new GBIAnimationState(owner.GetGraph(), descriptor), fadeDuration);
    }

    public PGBIAnimationState Play(PGBIAnimationStateDescriptor descriptor, float fadeDuration = 0)
    {
        return HandlePlay(new PGBIAnimationState(owner.GetGraph(), descriptor), fadeDuration);
    }

    private T HandlePlay<T>(T animationState, float fadeDuration = 0f)
        where T : AnimationState
    {
        if (fadeDuration <= 0) Play(animationState);
        else CrossFade(animationState, fadeDuration);
        return animationState;
    }

    private void Play(AnimationState state)
    {
        var previousConnector = currentConnector;
        Disconnect();
        previousConnector.Destroy();
        Connect(state);

        _currentState = state;
    }

    private void CrossFade(AnimationState state, float fadeDuration)
    {
        var previousConnector = currentConnector;
        var newConnector = state;
        var fadeConnector = new FadeConnector(owner.GetGraph(), fadeDuration);


        Disconnect();
        Connect(fadeConnector);
        fadeConnector.fromOutlet.Connect(previousConnector);
        fadeConnector.toOutlet.Connect(newConnector);

        _currentState = state;
    }
}