public abstract class AnimationState : Connector
{
    public abstract float speed { get; set; }
    public abstract float motionTime { get; set; }
    public abstract bool playing { get; set; }
}