using System;
using UnityEngine.Playables;

public abstract class Outlet<TOwner>
    where TOwner : struct, IPlayable
{
    internal Connector currentConnector => _currentConnector != null && _currentConnector.isValid ?
        _currentConnector : null;
    internal bool isValid => _owner.IsValid() && !_destroyed;
    internal bool isConnected => currentConnector != null;

    protected TOwner owner => _owner;
    protected int index => _index;

    private TOwner _owner;
    private int _index;
    private Connector _currentConnector;
    private bool _destroyed;

    internal Outlet(TOwner owner, int index)
    {
        _owner = owner;
        _index = index;
        _destroyed = false;
    }

    internal void Connect(Connector connector)
    {
        if (_destroyed)
            throw new InvalidOperationException("This outlet was destroyed.");
        if (currentConnector != null)
            throw new InvalidOperationException("There is already a connector connected. Disconnect it first before connecting another one.");
        _owner.ConnectInput(_index, connector.rootPlayable, 0, 1f);
        _currentConnector = connector;
        OnConnect();
    }

    internal void Disconnect()
    {
        if (_destroyed)
            throw new InvalidOperationException("This outlet was destroyed.");
        _owner.DisconnectInput(_index);
        _currentConnector = null;
        OnDisconnect();
    }

    internal void UpdateOwner(TOwner owner, int index = 0)
    {
        if (_destroyed)
            throw new InvalidOperationException("This outlet was destroyed.");
        if (_currentConnector != null)
        {
            var connector = _currentConnector;
            Disconnect();
            _owner = owner;
            _index = index;
            Connect(connector);
        }
        else
        {
            _owner = owner;
            _index = index;
        }
    }

    internal protected virtual void OnConnect() { }

    internal protected virtual void OnDisconnect() { }

    internal void Destroy()
    {
        if (_currentConnector != null)
        {
            _owner.DisconnectInput(_index);
            _currentConnector.Destroy();
        }
        _destroyed = true;
    }
}
