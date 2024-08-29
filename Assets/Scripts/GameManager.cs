using Lean.Touch;
using RootMotion.Dynamics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button _resetButton;
    [SerializeField] private Button _ragdollBtn;
    [SerializeField] private RagdollControll _ragdollControll;
    [SerializeField] private Camera _camera;
    [SerializeField] private IInteract _objectSelected;
    private Vector3 offset;
    private Vector3 screenPoint;
    [SerializeField] private Transform _transformSelect;
    private RaycastHit hit;
    private bool _canDragging = false;
    private bool _isDragging = false;
    [SerializeField] private float _forceImpact;
    private Rigidbody _rb;
    private float mass;
    [SerializeField] private PuppetMaster _puppetMaster;
    private ClaimPointControll _pointAddforce;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameObject _sphere;
    [SerializeField] private Texture2D _cursorTexture;

    private void Start()
    {
        // _resetButton.onClick.AddListener(_ragdollControll.AnimatorState);
        _ragdollBtn.onClick.AddListener(() => { _ragdollControll.SetStateRagdoll(false); });
        _camera = Camera.main;
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += OnfingerUpdate;
        LeanTouch.OnFingerUp += OnfingerUp;
        LeanTouch.OnFingerDown += OnfingerDown;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= OnfingerUpdate;
        LeanTouch.OnFingerUp -= OnfingerUp;
        LeanTouch.OnFingerDown -= OnfingerDown;
    }

    private void OnfingerUpdate(LeanFinger finger)
    {
        if (finger.IsOverGui == true)
        {
            Debug.Log("OVer UI ");
            return;
        }
        Ray ray = _camera.ScreenPointToRay(finger.ScreenPosition);

        int layer = 1 << LayerMask.NameToLayer("Doll");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            _transformSelect = hit.collider.transform;
            _objectSelected = hit.transform.GetComponentInParent<IInteract>();
            // Debug.Log("Hit object" + hit.transform.name);
        }

        if (_canDragging)
        {
            OnfingerDrag(finger);
        }
    }

    private void OnfingerDown(LeanFinger finger)
    {
        //test
        // GameObject go = Instantiate(_sphere);
        //// go.transform.position = _camera.ScreenToWorldPoint(finger.ScreenPosition);
        // go.transform.position = _camera.ScreenToViewportPoint( Input.mousePosition);

        if (_transformSelect != null)
        {
            _puppetMaster.pinWeight = 0.2f;
            //   _puppetMaster.muscleWeight = 0f;
            screenPoint = _camera.WorldToScreenPoint(_transformSelect.transform.position);
            _pointAddforce = _transformSelect.GetComponentInChildren<ClaimPointControll>();
            _rb = _pointAddforce.GetComponent<Rigidbody>();
            offset = _transformSelect.transform.position - _camera.ScreenToWorldPoint(new Vector3(finger.ScreenPosition.x, finger.ScreenPosition.y, screenPoint.z));
        }
        if (_rb != null)
        {
            mass = _rb.mass;
        }
        _canDragging = true;
        _isDragging = true;
    }

    private void OnfingerDrag(LeanFinger finger)
    {
        if (_transformSelect != null)
        {
            // Vector3 currentPosition = _camera.ScreenToWorldPoint(finger.ScreenPosition) + offset;
            Vector3 curScreenPoint = new Vector3(finger.ScreenPosition.x, finger.ScreenPosition.y, screenPoint.z);
            Vector3 curPosition = _camera.ScreenToWorldPoint(curScreenPoint) + offset;
            _sphere.transform.position = curPosition;
            Vector3 direction = curPosition - _rb.transform.position;

            if (_isDragging == true && _rb != null)
            {
                _rb.mass = 75;
                _isDragging = false;
            }
            if (_pointAddforce != null)
            {
                Vector3 pointadd = _camera.ScreenToWorldPoint(_pointAddforce.transform.position);
                Vector3 newPosiont = _camera.ScreenToWorldPoint(finger.ScreenPosition);
                float distance = Vector3.Distance(newPosiont, pointadd);

                Vector3[] drawLinePoint = new Vector3[] { _pointAddforce.transform.position, curPosition };
                DrawLineRenderer(drawLinePoint);
                curPosition.z = 0;
                curPosition = Vector3.Normalize(curPosition);
                Debug.Log($"_rb.velocity.magnitude {_rb.velocity.magnitude} velocity {_rb.velocity} ");
                Debug.Log($"currentPosition {curPosition}  distance {distance}");
                if (_rb.velocity.magnitude < 3)
                {
                    _rb.velocity = direction*_forceImpact;
                    //_rb.AddForce(direction.normalized * (distance * _forceImpact), ForceMode.Acceleration);
                }
            }
            //_rb.mass = mass;
            //  _objectSelected.OnDrag(curPosition);
            // LeanDragCamera.
        }
    }

    private void OnfingerUp(LeanFinger finger)
    {
        _isDragging = false;
        _canDragging = false;
        if ((_rb != null))
        {
            _rb.mass = mass;

            _rb = null;
        }
        _objectSelected = null;
        _transformSelect = null;
        // _objectSelected.OnRelease();
    }

    private void Update()
    {
    }

    private void OnSelectObj(LeanFinger finger)
    {
        if (finger.IsOverGui)
        {
            return;
        }
    }

    private void DrawLineRenderer(Vector3[] pointLine)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = 0.2f;
        _lineRenderer.SetPositions(pointLine);
    }
}