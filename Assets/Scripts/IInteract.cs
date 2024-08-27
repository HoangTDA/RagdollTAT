using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    public void OnDrag(Vector3 vectorDrag);
    public void OnRelease();
    public void OnSelect();
}
