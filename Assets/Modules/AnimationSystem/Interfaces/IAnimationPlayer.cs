using System.Collections.Generic;
using UnityEngine;

public interface IAnimationPlayer
{
    public SimpleAnimationState Play(SimpleAnimationStateDescriptor descriptor, float fadeDuration = 0f);

    public LinearAnimationState Play(LinearAnimationStateDescriptor descriptor, float fadeDuration = 0f);

    public IDAnimationState Play(IDAnimationStateDescriptor descriptor, float fadeDuration = 0f);
    public GBIAnimationState Play(GBIAnimationStateDescriptor descriptor, float fadeDuration = 0f);
    public PGBIAnimationState Play(PGBIAnimationStateDescriptor descriptor, float fadeDuration = 0f);
}