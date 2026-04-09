using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    public float angleClampEuler = 85f;

    private Camera currentCamera;

    void Awake()
    {
        currentCamera = GetComponent<Camera>();
    }

    public void HandleMovement(Vector3 delta)
    {
        var forwardVec = currentCamera.transform.forward;
        var upwardVec = currentCamera.transform.up;
        var sidewardVec = currentCamera.transform.right;

        transform.position += sidewardVec * delta.x + upwardVec * delta.y + forwardVec * delta.z;
    }

    public void HandleRotation(Vector2 deltaEuler)
    {
        var newXRotation = transform.localEulerAngles.x - deltaEuler.y;
        var newYRotation = transform.localEulerAngles.y + deltaEuler.x;

        transform.localEulerAngles = new Vector3(
            Mathf.Clamp(NormalizeAngle(newXRotation), -angleClampEuler, angleClampEuler),
            newYRotation,
            0
        );
    }

    private float NormalizeAngle(float angle)
    {
        if (Mathf.Abs(angle) < 180) return angle;
        while (Mathf.Abs(angle) > 180)
        {
            angle -= Mathf.Sign(angle) * 360;
        }
        return angle;
    }
}