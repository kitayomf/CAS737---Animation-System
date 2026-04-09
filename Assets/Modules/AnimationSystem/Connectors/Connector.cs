using UnityEngine.Playables;

public abstract class Connector {
    internal abstract Playable rootPlayable { get; }
    internal bool isValid => rootPlayable.IsValid();
    internal abstract void Destroy();
}