using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class FadeConnector : Connector
{
    internal override Playable rootPlayable => _fadePlayable;
    public Outlet<Playable> fromOutlet => _fromOutlet;
    public Outlet<Playable> toOutlet => _toOutlet;

    private Outlet<Playable> _fromOutlet;
    private Outlet<Playable> _toOutlet;
    private AnimationMixerPlayable _mixer;
    private ScriptPlayable<FadeBehaviour> _fadePlayable;

    public FadeConnector(PlayableGraph graph, float fadeDuration = 0.25f)
    {
        _fadePlayable = ScriptPlayable<FadeBehaviour>.Create(graph, 1);
        _mixer = AnimationMixerPlayable.Create(graph, 2);
        _fadePlayable.ConnectInput(0, _mixer, 0, 1f);

        _fromOutlet = new PlayableOutlet(_mixer, 0);
        _toOutlet = new PlayableOutlet(_mixer, 1);

        _fadePlayable.GetBehaviour().Init(_mixer, _fromOutlet, _toOutlet, fadeDuration);
    }

    internal override void Destroy()
    {
        if (_toOutlet.isValid) _toOutlet.Destroy();
        if (_fromOutlet.isValid) _fromOutlet.Destroy();
        if (_mixer.IsValid()) _mixer.Destroy();
        if (_fadePlayable.IsValid()) _fadePlayable.Destroy();
    }

    private class FadeBehaviour : PlayableBehaviour
    {
        private Outlet<Playable> _fromOutlet;
        private Outlet<Playable> _toOutlet;
        private AnimationMixerPlayable _mixer;
        private float _fadeSpeed;
        private float _fadeWeight;

        public void Init(AnimationMixerPlayable mixer, Outlet<Playable> fromOutlet, Outlet<Playable> toOutlet, float fadeDuration = 0.25f)
        {
            _mixer = mixer;
            _fromOutlet = fromOutlet;
            _toOutlet = toOutlet;
            _fadeSpeed = 1f / fadeDuration;
            _fadeWeight = 0f;
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (_fadeWeight >= 1f) return;

            _fadeWeight = Mathf.MoveTowards(_fadeWeight, 1f, _fadeSpeed * info.deltaTime);
            _mixer.SetInputWeight(0, 1f - _fadeWeight);
            _mixer.SetInputWeight(1, _fadeWeight);

            if (_fadeWeight < 1f) return;

            //Flatten if fade finished
            playable.DisconnectInput(0);
            _toOutlet.UpdateOwner(playable);
            _fromOutlet.Destroy();
            if (_mixer.IsValid()) _mixer.Destroy();
        }
    }
}