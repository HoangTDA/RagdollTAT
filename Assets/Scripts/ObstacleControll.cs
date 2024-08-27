using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class ObstacleControll : MonoBehaviour
{
    private LeanDragTranslate _dragTranslate;
    private void Start()
    {
        LeanTouch.OnFingerUp += OnFingerUp;
    }
    private void OnDestroy()
    {
        LeanTouch.OnFingerUp -= OnFingerUp;
    }
    private void OnFingerUp(LeanFinger finger)
    {
       // Debug.Log("OnFingerUp");
    }
}