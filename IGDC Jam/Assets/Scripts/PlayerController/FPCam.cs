using UnityEngine;
using static PlayerPrefStatics;

public class FPCam : AbilityUpdate
{
    public bool hasHorizontalLook;
    public bool hasVerticalLook;

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    public Transform orientation;

    private float xRotation;
    private float yRotation;

    private float originalYRotation;

    [SerializeField] private float fovChangeSpeed;
    [SerializeField] private float maxFOVChange;
    [SerializeField] private Vector2 fovRange;
    [SerializeField] private Vector2 speedRange;
    private Camera cam;
    private float currentFOVVelocity;
    private Rigidbody characterRB;

    private void Start()
    {
        cam = GetComponent<Camera>();
        characterRB = orientation.parent.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SetUpRestrictions();
        transform.rotation = orientation.rotation;

        originalYRotation = orientation.transform.localEulerAngles.y;

        sensX *= Mathf.Max(PlayerPrefs.GetFloat(MouseSens, 1f), 0.1f);
        sensY *= Mathf.Max(PlayerPrefs.GetFloat(MouseSens, 1f), 0.1f);
    }

    [ContextMenu("Take Away Look Abilities")]
    private void TakeAbilities()
    {
        PlayerPrefs.SetInt(LookHorizontalRestriction, 0);
        PlayerPrefs.SetInt(LookVerticalRestriction, 0);
        SetUpRestrictions();
    }

    [ContextMenu("Give Look Abilities")]
    private void GiveAbilities()
    {
        PlayerPrefs.SetInt(LookHorizontalRestriction, 1);
        PlayerPrefs.SetInt(LookVerticalRestriction, 1);
        SetUpRestrictions();
    }


    public override void SetUpRestrictions()
    {
        hasHorizontalLook = PlayerPrefs.GetInt(LookHorizontalRestriction, 0) == 1;
        hasVerticalLook = PlayerPrefs.GetInt(LookVerticalRestriction, 0) == 1;
    }

    private void Update()
    {
        float mouseX = 0, mouseY = 0;
        if(hasHorizontalLook)
            mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;

        if(hasVerticalLook)
            mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        if (!hasHorizontalLook && !hasVerticalLook) return;
        
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRotation, yRotation+originalYRotation, 0);
        orientation.localRotation = Quaternion.Euler(0, yRotation+originalYRotation, 0);


        FOVHandling();
    }

    private void FOVHandling()
    {
        Vector3 flatVel = new(characterRB.velocity.x, 0, characterRB.velocity.z);
        float invLerpedSpeed = Mathf.InverseLerp(speedRange.x, speedRange.y, flatVel.magnitude);
        float fovTarget = Mathf.Lerp(fovRange.x, fovRange.y, invLerpedSpeed);

        cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, fovTarget, ref currentFOVVelocity, fovChangeSpeed*Time.deltaTime, maxFOVChange);
    }
}
