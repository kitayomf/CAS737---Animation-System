using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class IDAnimationStateDescriptor
{
    public readonly PlanarBlendMap blendMap;
    public readonly Vector2 referencePoint;
    public readonly float speed;
    public readonly float motionTime;
    public readonly bool playing;

    public IDAnimationStateDescriptor(PlanarBlendMap blendMap, Vector2 referencePoint = default, float speed = 1f,
        float motionTime = 0f, bool playing = true)
    {
        this.blendMap = blendMap;
        this.referencePoint = referencePoint;
        this.speed = speed;
        this.motionTime = motionTime;
        this.playing = playing;
    }
}

public class IDAnimationState : BlendAnimationState<Vector2>
{
    public IDAnimationState(PlayableGraph graph, IDAnimationStateDescriptor descriptor)
        : base(graph, descriptor.blendMap, descriptor.referencePoint)
    {
        speed = descriptor.speed;
        motionTime = descriptor.motionTime;
        playing = descriptor.playing;
    }

    protected override void UpdateWeights(Vector2 referencePoint, IEnumerable<BlendPoint<Vector2>> points)
    {
        var distances = new float[points.Count()];

        //Calculate distance
        for (int i = 0; i < points.Count(); i++)
        {
            distances[i] = Vector2.Distance(referencePoint, points.ElementAt(i).position);
        }

        //Check if any of the distance is zero
        var zeroIndex = FindIndexWithDistanceZero(distances);
        if (zeroIndex >= 0)
        {
            for (int i = 0; i < points.Count(); i++)
            {
                if (i == zeroIndex) SetWeight(i, 1f);
                else SetWeight(i, 0f);
            }
            return;
        }

        //If not, calculate inverted distances
        var influences = new float[points.Count()];
        for (int i = 0; i < points.Count(); i++)
        {
            influences[i] = 1 / distances[i];
        }

        //Calculate total influences
        var influencesSum = influences.Sum();

        //Calculate weights
        for (int i = 0; i < points.Count(); i++)
        {
            SetWeight(i, influences[i] / influencesSum);
        }
    }

    private int FindIndexWithDistanceZero(float[] distances)
    {
        for (int i = 0; i < distances.Length; i++)
        {
            if (Mathf.Approximately(distances[i], 0)) return i;
        }
        return -1;
    }
}