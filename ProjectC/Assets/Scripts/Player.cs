using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IDamagable
{
    // Movement related
    public LayerMask objectsLayer; // Things the player can walk on, collide with, etc. Go with "Default" layer for now (put player in "Player").
    [Range(0f, 10f)]
    public float movementSpeed; // 5f
    [Range(0f, 10f)]
    public float jumpPower; // 4f
    [Range(0f, 10f)]
    public float groundDistance; // 0.3f
    Transform feet; // good local position = new Vector3(0f, -1f, 0f);

    Rigidbody playerRbody;
    Collider playerCollider; // try capsule collider
    Vector3 playerMoveDir = Vector3.zero;
    public bool isGrounded;

    // Rotation related
    Transform eyes; // good local position = new Vector3(0f, 0.9f, 0f);
    float playerYaw;
    float eyesPitch;
    float eyesPitchThreshold = 89f;
    [Range(0f, 360f)]
    public float mouseSensitivity = 45f;

    // Inputs related
    bool isHoldingLMB;
    bool isPressedSpace;
    float mouseScrollWheel;

    // Pick ups related
    string tagPickup = "Pickup";

    // Weapon related
    List<Weapon> listWeapons;
    int iCurWeapon;
    Transform weaponSlot; // good local position = new Vector3(0.75f, -0.6f, 0.7f);

    void InitLocalTransforms()
    {
        eyes = new GameObject("eyes").transform;
        eyes.parent = this.transform;
        eyes.localPosition = new Vector3(0f, 0.9f, 0f); // magic
        Camera.main.transform.parent = eyes;
        Camera.main.transform.localPosition = Vector3.zero;

        feet = new GameObject("feet").transform;
        feet.parent = this.transform;
        feet.localPosition = new Vector3(0f, -1f, 0f); // majeek

        weaponSlot = new GameObject("weapon slot").transform;
        weaponSlot.parent = eyes;
        weaponSlot.localPosition = new Vector3(0.75f, -0.6f, 0.7f); // mahjong
    }

    void InitWeaponsList()
    {
        listWeapons = new List<Weapon>();
    }

    void HandleInputs()
    {
        isHoldingLMB = Input.GetMouseButton((int)MouseButton.LeftMouse);
        isPressedSpace = Input.GetKeyDown(KeyCode.Space);

        mouseScrollWheel = 0f;
        mouseScrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        playerYaw += mouseSensitivity * mouseX * Time.deltaTime;
        eyesPitch -= mouseSensitivity * mouseY * Time.deltaTime; // Because negative angle about X axis is up.

        eyesPitch = Mathf.Clamp(eyesPitch, -eyesPitchThreshold, eyesPitchThreshold);

        // First, reset all rotations.
        transform.rotation = Quaternion.identity;
        eyes.rotation = Quaternion.identity;

        // Then, rotate with the new angles.
        transform.Rotate(Vector3.up, playerYaw);
        eyes.Rotate(Vector3.right, eyesPitch);
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(feet.position, groundDistance, objectsLayer, QueryTriggerInteraction.Ignore);

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        playerMoveDir = Vector3.ClampMagnitude(new Vector3(moveX, 0, moveY), 1);

        playerMoveDir = transform.TransformDirection(playerMoveDir);

        if (isPressedSpace && isGrounded)
        {
            playerRbody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
        }
    }

    void HandleChangeWeapon()
    {
        // TODO: Don't change weapon if current weapon isFiring or isReloading. Otherwise it'll mess up the coroutines.

        if (mouseScrollWheel == 0f || listWeapons.Count == 0 /*TODO: || isFiring || isReloading*/)
            return;

        getCurrentWeapon()?.gameObject.SetActive(false);

        if (mouseScrollWheel > 0.0f)
            iCurWeapon++;
        else
            iCurWeapon--;

        if (iCurWeapon < 0)
            iCurWeapon = listWeapons.Count - 1;
        else
            iCurWeapon %= listWeapons.Count;

        getCurrentWeapon()?.gameObject.SetActive(true);
    }

    void HandleWeapon()
    {
        if (isHoldingLMB)
        {
            getCurrentWeapon()?.Fire();
        }
    }

    void Start()
    {
        playerRbody = this.gameObject.AddComponent<Rigidbody>();
        playerRbody.mass = 70.0f;
        playerRbody.constraints = RigidbodyConstraints.FreezeRotation;
        playerRbody.useGravity = true;

        playerCollider = GetComponent<CapsuleCollider>(); // yeah, use a capsule collider if you can.

        InitLocalTransforms();
        InitWeaponsList();
    }

    void Update()
    {
        HandleInputs();
        HandleRotation();
        HandleMovement();
        HandleChangeWeapon();
        HandleWeapon();
    }

    void FixedUpdate()
    {
        playerRbody.MovePosition(playerRbody.position + playerMoveDir * movementSpeed * Time.fixedDeltaTime);
    }

    public void GetDamaged(float amount)
    {
        Debug.Log("TODO");
    }
    public void EquipNewWeapon(Weapon weapon)
    {
        bool firstWeapon = listWeapons.Count == 0;

        listWeapons.Add(weapon);
        weapon.gameObject.transform.parent = weaponSlot;
        weapon.gameObject.transform.localPosition = Vector3.zero;
        weapon.gameObject.transform.localRotation = Quaternion.identity;

        weapon.gameObject.SetActive(firstWeapon);
    }

    Weapon getCurrentWeapon()
    {
        if (listWeapons.Count == 0)
            return null;
        else if (listWeapons.Count == 1)
            return listWeapons[0]; // If only 1 weapon, then always return THAT. Don't care about isActive etc.
        else
            return listWeapons[iCurWeapon];
    }
}