using System.Collections.Generic;
using UnityEngine;

public class Debug1DSlider : MonoBehaviour
{
    public List<GameObject> objectList;
    public float from = 0f;
    public float to = 1f;
    public float initial = 0f;
    private Rect _windowRect = new(20, 20, 120, 100);
    private float _value;
    
    void Awake()
    {
        _value = initial;
    }

    void OnGUI()
    {
        _windowRect = GUI.Window(0, _windowRect, WindowFunction, "My Window");
    }

    private void WindowFunction(int windowID)
    {
        GUI.DragWindow(new(0, 0, _windowRect.width, 20));

        _value = GUI.HorizontalSlider(new Rect(10, 25, 100, 15), _value, from, to);

        if (GUI.Button(new Rect(10, 50, 100, 30), "Reset"))
        {
            _value = initial;
        }

        foreach (var gameObject in objectList)
        {
            var example = gameObject.GetComponentInChildren<IAnimation1DExample>();
            if(example != null) example.referencePoint = _value;
        }
    }
}