using UnityEngine;

[RequireComponent(typeof(AnimationPlayer))]
public class LayeringExample : MonoBehaviour
{
    [SerializeField] private AvatarMask _avatarMask;
    [SerializeField] private AnimationClip _animation1;
    [SerializeField] private AnimationClip _animation2;
    [SerializeField] private float _animationSpeed1 = 1f;
    [SerializeField] private float _animationSpeed2 = 0.25f;
    private AnimationPlayer _animationPlayer;

    void Start()
    {
        _animationPlayer = GetComponent<AnimationPlayer>();
        var layer = _animationPlayer.AddLayer(new("UpperBody", avatarMask: _avatarMask));

        _animationPlayer.Play(new SimpleAnimationStateDescriptor(_animation1, _animationSpeed1));
        layer.Play(new SimpleAnimationStateDescriptor(_animation2, _animationSpeed2));
    }
}