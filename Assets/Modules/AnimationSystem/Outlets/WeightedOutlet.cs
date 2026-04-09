using UnityEngine.Playables;

public abstract class WeightedOutlet<T> : Outlet<T>
    where T : struct, IPlayable
{
    public float weight
    {
        get
        {
            return _weight;
        }
        set
        {
            _weight = value;
            owner.SetInputWeight(index, _weight);
        }
    }

    private float _weight = 0f;

    public WeightedOutlet(T owner, int index) : base(owner, index)
    {

    }

    internal protected override void OnConnect()
    {
        weight = _weight;
    }
}