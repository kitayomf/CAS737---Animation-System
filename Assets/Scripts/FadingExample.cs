using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationPlayer))]
public class FadingExample : MonoBehaviour
{
    [SerializeField] private List<AnimationClip> _animationClipList = new();
    [SerializeField] private float _animationSpeed = 0.5f;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private float _clipPlayDuration = 2f;

    private Coroutine _coroutine;
    private AnimationPlayer _animationPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animationPlayer = GetComponent<AnimationPlayer>();
        _coroutine = StartCoroutine(PlayLoop());
    }

    void OnDestroy()
    {
        StopCoroutine(_coroutine);
    }

    private IEnumerator PlayLoop()
    {
        if (_animationClipList.Count <= 0) yield break;

        var index = 0;

        while (true)
        {
            var animationClip = _animationClipList[index];
            var state = _animationPlayer.Play(new SimpleAnimationStateDescriptor(animationClip, _animationSpeed), _fadeDuration);

            while (state.motionTime <= _clipPlayDuration) yield return null;

            index = (index + 1) % _animationClipList.Count;
        }
    }
}
