using System.Collections.Generic;
using UnityEngine;

public class Debug2DSlider : MonoBehaviour
{
    public List<GameObject> objectList;
    public float from = -1f;
    public float to = 1f;
    public float initial = 0f;

    private Rect _windowRect = new(20, 20, 120, 120);
    private float _xValue;
    private float _yValue;
    
    void Awake()
    {
        _xValue = initial;
        _yValue = initial;
    }

    void OnGUI()
    {
        _windowRect = GUI.Window(0, _windowRect, WindowFunction, "My Window");
    }

    private void WindowFunction(int windowID)
    {
        GUI.DragWindow(new(0, 0, _windowRect.width, 20));

        _xValue = GUI.HorizontalSlider(new Rect(10, 25, 100, 15), _xValue, from, to);
        _yValue = GUI.HorizontalSlider(new Rect(10, 50, 100, 15), _yValue, from, to);

        if (GUI.Button(new Rect(10, 75, 100, 30), "Reset"))
        {
            _xValue = initial;
            _yValue = initial;
        }

        foreach (var gameObject in objectList)
        {
            var example = gameObject.GetComponentInChildren<IAnimation2DExample>();
            if(example != null) example.referencePoint = new(_xValue, _yValue);
        }
    }
}