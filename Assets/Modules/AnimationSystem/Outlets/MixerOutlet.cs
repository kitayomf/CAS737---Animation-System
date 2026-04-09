using UnityEngine.Animations;

public class MixerOutlet : WeightedOutlet<AnimationMixerPlayable>
{
    public MixerOutlet(AnimationMixerPlayable owner, int index) : base(owner, index)
    {
    }
}