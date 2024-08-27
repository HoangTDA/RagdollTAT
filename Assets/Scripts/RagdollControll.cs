using UnityEngine;

public class RagdollControll : BaseInteract
{
    [SerializeField] private Rigidbody[] _rigidbodies;
    [SerializeField] private Animator _animations;
    [SerializeField] private GameObject _obstacle;
    public float Durability = 10f;

    private void Start()
    {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        SetStateRagdoll(true);
    }

    public void SetStateRagdoll(bool isOn)
    {
        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = isOn;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            SetStateRagdoll(false);
            // EnableRadoll();
            //Debug.Log("Destroy and win game");
            //if (CalculatorToDestroy(5))
            //{
            //    return;
            //}
        }
    }

    private bool CalculatorToDestroy(float forceReceiver)
    {
        Durability -= forceReceiver;
        return Durability <= 0;
    }

    public void AnimatorState()
    {
        _animations.enabled = true;
        //_obstacle.transform.position = _vector3Position;
        //_obstacle.transform.rotation = Quaternion.identity;
    }

    public override void OnDrag(Vector3 vectorDrag)
    {
        SetStateRagdoll(false);
        SetMass();
       // gameObject.transform.position = vectorDrag;
        Debug.Log("OnDrag " + vectorDrag);
    }

    public override void OnRelease()
    {
        base.OnRelease();
    }

    public override void OnSelect()
    {
        base.OnSelect();
    }

    private void SetMass()
    {
        foreach (var rigidbody in _rigidbodies)
        {
            
        }
    }
}