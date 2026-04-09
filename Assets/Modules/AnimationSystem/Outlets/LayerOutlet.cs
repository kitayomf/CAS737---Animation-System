using UnityEngine;
using UnityEngine.Animations;

public class LayerOutlet : WeightedOutlet<AnimationLayerMixerPlayable>
{
    public AvatarMask avatarMask
    {
        get
        {
            return _avatarMask;
        }
        set
        {
            _avatarMask = value;
            owner.SetLayerMaskFromAvatarMask((uint)index, _avatarMask != null ? _avatarMask : new AvatarMask());
        }
    }
    public bool additive
    {
        get
        {
            return _additive;
        }
        set
        {
            _additive = value;
            owner.SetLayerAdditive((uint)index, _additive);
        }
    }

    private AvatarMask _avatarMask = null;
    private bool _additive = false;

    public LayerOutlet(AnimationLayerMixerPlayable owner, int index) : base(owner, index)
    {

    }

    internal protected override sealed void OnConnect()
    {
        base.OnConnect();
        avatarMask = _avatarMask;
        additive = _additive;
    }
}