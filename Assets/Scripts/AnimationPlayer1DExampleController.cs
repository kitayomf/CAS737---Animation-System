using UnityEngine;


[RequireComponent(typeof(AnimationPlayer))]
public class AnimationPlayer1DExampleController : MonoBehaviour, IAnimation1DExample
{
    public enum BlendTreeType
    {
        Linear
    }

    public float referencePoint
    {
        get => _blendAnimationState.referencePoint;
        set => _blendAnimationState.referencePoint = value;
    }

    [SerializeField] private LinearBlendMap _blendPoints;
    [SerializeField] private BlendTreeType _blendTreeType = BlendTreeType.Linear;

    private AnimationPlayer _animationPlayer;
    private BlendAnimationState<float> _blendAnimationState;

    void Start()
    {
        _animationPlayer = GetComponent<AnimationPlayer>();
        switch (_blendTreeType)
        {
            case BlendTreeType.Linear:
                _blendAnimationState = _animationPlayer.Play(new LinearAnimationStateDescriptor(_blendPoints));
                break;
        }
    }
}
