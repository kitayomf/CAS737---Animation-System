using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class GBIAnimationStateDescriptor
{
    public readonly PlanarBlendMap blendMap;
    public readonly Vector2 referencePoint;
    public readonly float speed;
    public readonly float motionTime;
    public readonly bool playing;

    public GBIAnimationStateDescriptor(PlanarBlendMap blendMap, Vector2 referencePoint = default, float speed = 1f,
        float motionTime = 0f, bool playing = true)
    {
        this.blendMap = blendMap;
        this.referencePoint = referencePoint;
        this.speed = speed;
        this.motionTime = motionTime;
        this.playing = playing;
    }
}

public class GBIAnimationState : BlendAnimationState<Vector2>
{
    public GBIAnimationState(PlayableGraph graph, GBIAnimationStateDescriptor descriptor)
        : base(graph, descriptor.blendMap, descriptor.referencePoint)
    {
        speed = descriptor.speed;
        motionTime = descriptor.motionTime;
        playing = descriptor.playing;
    }

    protected override void UpdateWeights(Vector2 referencePoint, IEnumerable<BlendPoint<Vector2>> points)
    {
        //Check if point count is zero or one
        if (points.Count() <= 0) return;

        if (points.Count() <= 1)
        {
            SetWeight(0, 1f);
            return;
        }

        //Check if any of the distance is zero
        var zeroIndex = FindIndexWithDistanceZero(referencePoint, points);
        if (zeroIndex >= 0)
        {
            for (int i = 0; i < points.Count(); i++)
            {
                if (i == zeroIndex) SetWeight(i, 1f);
                else SetWeight(i, 0f);
            }
            return;
        }

        //Calculate influences
        var influences = new float[points.Count()];

        for (int i = 0; i < points.Count(); i++)
        {
            var pi = points.ElementAt(i).position;
            var vis = referencePoint - pi;
            var minWeight = float.PositiveInfinity;

            for (int j = 0; j < points.Count(); j++)
            {
                if (i == j) continue;

                var pj = points.ElementAt(j).position;
                var vij = pj - pi;
                var new_weight = Mathf.Max(1 - (Vector2.Dot(vis, vij) / Vector2.Dot(vij, vij)), 0);
                minWeight = Mathf.Min(new_weight, minWeight);
            }

            influences[i] = minWeight;
        }

        //Calculate total influence
        var totalInfluence = influences.Sum();

        //Calculate weights
        for (int i = 0; i < points.Count(); i++)
        {
            SetWeight(i, influences[i] / totalInfluence);
        }
    }

    private int FindIndexWithDistanceZero(Vector2 referencePoint, IEnumerable<BlendPoint<Vector2>> points)
    {
        for (int i = 0; i < points.Count(); i++)
        {
            var point = points.ElementAt(i);
            var distance = Vector2.Distance(referencePoint, point.position);
            if (Mathf.Approximately(distance, 0f)) return i;
        }

        return -1;
    }
}