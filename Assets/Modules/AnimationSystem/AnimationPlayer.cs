using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour, ILayeredAnimationPlayer
{
    public Animator animator;
    public bool applyRootMotion;
    public List<AnimationLayerDescriptor> additionalLayers;

    public int LayerCount => _animationSystem.LayerCount;
    public AnimationState currentAnimationState => _animationSystem.currentAnimationState;

    private AnimationSystem _animationSystem;

    void Awake()
    {
        if (animator == null)
        {
            if (!TryGetComponent(out animator))
            {
                animator = gameObject.AddComponent<Animator>();
                animator.applyRootMotion = applyRootMotion;
            }
        }

        if (animator.runtimeAnimatorController != null)
        {
            animator.runtimeAnimatorController = null;
        }

        if (animator.avatar == null)
        {
            animator.avatar = AvatarBuilder.BuildGenericAvatar(gameObject, name);
        }

        _animationSystem = new AnimationSystem(animator, additionalLayers);
    }

    public SimpleAnimationState Play(SimpleAnimationStateDescriptor parameter, float fadeDuration = 0f)
    {
        return _animationSystem.Play(parameter, fadeDuration);
    }

    public LinearAnimationState Play(LinearAnimationStateDescriptor parameter, float fadeDuration = 0f)
    {
        return _animationSystem.Play(parameter, fadeDuration);
    }

    public IDAnimationState Play(IDAnimationStateDescriptor parameter, float fadeDuration = 0)
    {
        return _animationSystem.Play(parameter, fadeDuration);
    }

    public GBIAnimationState Play(GBIAnimationStateDescriptor parameter, float fadeDuration = 0)
    {
        return _animationSystem.Play(parameter, fadeDuration);
    }

    public PGBIAnimationState Play(PGBIAnimationStateDescriptor parameter, float fadeDuration = 0)
    {
        return _animationSystem.Play(parameter, fadeDuration);
    }

    public AnimationLayer AddLayer(AnimationLayerDescriptor descriptor)
    {
        return _animationSystem.AddLayer(descriptor);
    }

    public void RemoveLayer(string layerName)
    {
        _animationSystem.RemoveLayer(layerName);
    }

    public void RemoveLayer(int index)
    {
        _animationSystem.RemoveLayer(index);
    }

    public AnimationLayer GetLayer(string layerName)
    {
        return _animationSystem.GetLayer(layerName);
    }

    public AnimationLayer GetLayer(int index)
    {
        return _animationSystem.GetLayer(index);
    }

    public IEnumerable<AnimationLayer> GetLayers()
    {
        return _animationSystem.GetLayers();
    }

    void OnDestroy()
    {
        _animationSystem.Destroy();
    }

}