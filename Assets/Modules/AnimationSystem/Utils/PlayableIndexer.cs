using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;

internal class PlayableIndexer<TPlayable, TOutlet> : IEnumerable<TOutlet>
    where TPlayable : struct, IPlayable
    where TOutlet : Outlet<TPlayable>
{
    internal TPlayable playable => _playable;
    internal int Count => _playable.GetInputCount();
    internal TOutlet this[int index] => _outletList[index];

    private List<TOutlet> _outletList;
    private TPlayable _playable;

    internal PlayableIndexer(TPlayable playable)
    {
        _playable = playable;
        _playable.SetInputCount(0);
        _outletList = new();
    }

    internal TOutlet Add(Func<TPlayable, int, TOutlet> outletFactory)
    {
        var inputCount = _playable.GetInputCount();
        _playable.SetInputCount(inputCount + 1);
        var outlet = outletFactory(_playable, inputCount);
        _outletList.Add(outlet);
        return outlet;
    }

    internal void Remove(int index)
    {
        var inputCount = _playable.GetInputCount();
        if (index < 0 || index >= inputCount) throw new IndexOutOfRangeException();

        _outletList[index].Destroy();
        for (int i = index + 1; i < inputCount; i++)
        {
            _outletList[i].UpdateOwner(_playable, i - 1);
        }
        _playable.SetInputCount(inputCount - 1);
        _outletList.RemoveAt(index);
    }

    internal void Destroy()
    {
        if (!_playable.IsValid()) return;

        foreach (var outlet in _outletList) outlet.Destroy();

        _playable.Destroy();
    }

    public IEnumerator<TOutlet> GetEnumerator()
    {
        return _outletList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}