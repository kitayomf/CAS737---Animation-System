using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class LinearAnimationStateDescriptor
{
    public readonly LinearBlendMap blendMap;
    public readonly float referencePoint;
    public readonly float speed;
    public readonly float motionTime;
    public readonly bool playing;

    public LinearAnimationStateDescriptor(LinearBlendMap blendMap, float referencePoint = 0f, float speed = 1f,
        float motionTime = 0f, bool playing = true)
    {
        this.blendMap = blendMap;
        this.referencePoint = referencePoint;
        this.speed = speed;
        this.motionTime = motionTime;
        this.playing = playing;
    }
}

public class LinearAnimationState : BlendAnimationState<float>
{
    public LinearAnimationState(PlayableGraph graph, LinearAnimationStateDescriptor descriptor)
        : base(graph, descriptor.blendMap, descriptor.referencePoint)
    {
        speed = descriptor.speed;
        motionTime = descriptor.motionTime;
        playing = descriptor.playing;
    }

    protected override void UpdateWeights(float referencePoint, IEnumerable<BlendPoint<float>> points)
    {
        //If no points, return
        if (points.Count() <= 0) return;

        //Set every weight to zero
        for (int i = 0; i < points.Count(); i++)
        {
            SetWeight(i, 0f);
        }

        //Get the nearest two point on either side
        var (min, max) = GetAffectedPointIndexes(referencePoint, points);

        //If either min or max is invalid, set the other point to 1f weight
        if (min == max)
        {
            SetWeight(min, 1f);
            return;
        }
        if (min < 0)
        {
            SetWeight(max, 1f);
            return;
        }
        if (max >= points.Count())
        {
            SetWeight(min, 1f);
            return;
        }

        //Calculate weights
        var weight = Mathf.InverseLerp(points.ElementAt(min).position, points.ElementAt(max).position, referencePoint);
        for (int i = 0; i < points.Count(); i++)
        {
            if (i == min) SetWeight(i, 1 - weight);
            else if (i == max) SetWeight(i, weight);
            else SetWeight(i, 0f);
        }
    }

    private (int, int) GetAffectedPointIndexes(float referencePoint, IEnumerable<BlendPoint<float>> points)
    {
        int minIndex = -1;
        float minDiff = float.NegativeInfinity;
        int maxIndex = points.Count();
        float maxDiff = float.PositiveInfinity;

        for (int i = 0; i < points.Count(); i++)
        {
            var diff = referencePoint - points.ElementAt(i).position;
            if (diff <= 0 && diff > minDiff)
            {
                minDiff = diff;
                minIndex = i;
            }
            if (diff >= 0 && diff < maxDiff)
            {
                maxDiff = diff;
                maxIndex = i;
            }
        }

        return (minIndex, maxIndex);
    }
}