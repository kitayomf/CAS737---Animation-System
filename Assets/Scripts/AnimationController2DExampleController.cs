using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController2DExampleController : MonoBehaviour, IAnimation2DExample
{
    public enum BlendTreeType
    {
        GBI,
        PGBI
    }

    public Vector2 referencePoint
    {
        get
        {
            return new(_animator.GetFloat("BlendX"), _animator.GetFloat("BlendY"));
        }
        set
        {
            _animator.SetFloat("BlendX", value.x);
            _animator.SetFloat("BlendY", value.y);
        }
    }

    [SerializeField] private BlendTreeType _blendTreeType = BlendTreeType.GBI;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        switch (_blendTreeType)
        {
            case BlendTreeType.GBI:
                _animator.Play("GBI");
                break;
            case BlendTreeType.PGBI:
                _animator.Play("PGBI");
                break;
        }
    }
}