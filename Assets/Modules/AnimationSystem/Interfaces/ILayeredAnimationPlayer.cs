using System.Collections.Generic;

public interface ILayeredAnimationPlayer : IAnimationPlayer
{
    public AnimationLayer AddLayer(AnimationLayerDescriptor descriptor);
    public void RemoveLayer(string layerName);
    public void RemoveLayer(int index);
    public AnimationLayer GetLayer(string layerName);
    public AnimationLayer GetLayer(int index);
    public IEnumerable<AnimationLayer> GetLayers();
}