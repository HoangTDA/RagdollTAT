using UnityEngine;

public abstract class BaseInteract : MonoBehaviour, IInteract
{
    public Animator animator;
    public float ImpactForce;
    public float ForePush=2;
    public virtual void OnDrag(Vector3 vectorDrag)
    {
        //RgBody.AddForce(vectorDrag*ForePush,ForceMode.Force);
    }

    public virtual void OnRelease()
    {
    }

    public virtual void OnSelect()
    {
    }
    
    public virtual void AnimState(string nameAnim)
    {
        if (animator != null)
        {
            animator.SetTrigger(nameAnim);
        }
    }
   
}