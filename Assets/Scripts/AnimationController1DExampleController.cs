using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController1DExampleController : MonoBehaviour, IAnimation1DExample
{
    public enum BlendTreeType
    {
        Linear
    }

    public float referencePoint
    {
        get
        {
            return _animator.GetFloat("Blend");
        }
        set
        {
            _animator.SetFloat("Blend", value);
        }
    }

    [SerializeField] private BlendTreeType _blendTreeType = BlendTreeType.Linear;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        switch (_blendTreeType)
        {
            case BlendTreeType.Linear:
                _animator.Play("Linear");
                break;
        }
    }
}