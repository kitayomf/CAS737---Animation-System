using UnityEngine;


[RequireComponent(typeof(AnimationPlayer))]
public class AnimationPlayer2DExampleController : MonoBehaviour, IAnimation2DExample
{
    public enum BlendTreeType
    {
        ID,
        GBI,
        PGBI
    }

    public Vector2 referencePoint
    {
        get => _blendAnimationState.referencePoint;
        set => _blendAnimationState.referencePoint = value;
    }

    [SerializeField] private PlanarBlendMap _blendPoints;
    [SerializeField] private BlendTreeType _blendTreeType = BlendTreeType.ID;

    private AnimationPlayer _animationPlayer;
    private BlendAnimationState<Vector2> _blendAnimationState;

    void Start()
    {
        _animationPlayer = GetComponent<AnimationPlayer>();
        switch (_blendTreeType)
        {
            case BlendTreeType.ID:
                _blendAnimationState = _animationPlayer.Play(new IDAnimationStateDescriptor(_blendPoints));
                break;
            case BlendTreeType.GBI:
                _blendAnimationState = _animationPlayer.Play(new GBIAnimationStateDescriptor(_blendPoints));
                break;
            case BlendTreeType.PGBI:
                _blendAnimationState = _animationPlayer.Play(new PGBIAnimationStateDescriptor(_blendPoints));
                break;
        }
    }
}
