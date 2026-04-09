using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class PGBIAnimationStateDescriptor
{
    public readonly PlanarBlendMap blendMap;
    public readonly Vector2 referencePoint;
    public readonly float alpha;
    public readonly float speed;
    public readonly float motionTime;
    public readonly bool playing;

    public PGBIAnimationStateDescriptor(PlanarBlendMap blendMap, Vector2 referencePoint = default, float speed = 1f,
        float motionTime = 0f, bool playing = true, float alpha = 2f)
    {
        this.blendMap = blendMap;
        this.referencePoint = referencePoint;
        this.speed = speed;
        this.motionTime = motionTime;
        this.playing = playing;
        this.alpha = alpha;
    }
}

public class PGBIAnimationState : BlendAnimationState<Vector2>
{
    private float _alpha = 2;

    public PGBIAnimationState(PlayableGraph graph, PGBIAnimationStateDescriptor descriptor)
        : base(graph, descriptor.blendMap, descriptor.referencePoint)
    {
        speed = descriptor.speed;
        motionTime = descriptor.motionTime;
        playing = descriptor.playing;
        _alpha = descriptor.alpha;
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
            var pim = pi.magnitude;
            var minWeight = float.PositiveInfinity;

            for (int j = 0; j < points.Count(); j++)
            {
                if (i == j) continue;

                var pj = points.ElementAt(j).position;
                var pjm = pj.magnitude;

                var mean_ij = (pjm + pim) / 2;

                var vij = new Vector2(
                    (pjm - pim) / mean_ij,
                    Mathf.Deg2Rad * Vector2.SignedAngle(pj, pi) * _alpha
                );

                var vis = new Vector2(
                    (referencePoint.magnitude - pim) / mean_ij,
                    Mathf.Deg2Rad * Vector2.SignedAngle(referencePoint, pi) * _alpha
                );

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