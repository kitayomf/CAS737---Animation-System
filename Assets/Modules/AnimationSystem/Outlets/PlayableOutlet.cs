using UnityEngine.Playables;

public class PlayableOutlet : Outlet<Playable>
{
    public PlayableOutlet(Playable owner, int index) : base(owner, index)
    {
    }
}