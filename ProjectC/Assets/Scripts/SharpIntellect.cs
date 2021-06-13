using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SharpIntellect : MonoBehaviour, IDamagable
{
    public int health;
    int maxHealth = 100;

    public Transform eyesTransform;
    public Transform faceTransform;
    public List<Weapon> possibleWeapons;
    public Transform weaponSlot;

    Weapon equippedWeapon;

    NavMeshAgent navMeshAgent;

    Player player;
    float playerHeight;

    public LayerMask visibilityAllowedLayers;
    float raycastTimer;
    float rayCastMaxTime = 1.0f;
    bool canSeePlayer;

    public float lookSlerpSpeed = 3.5f;

    Vector3 targetLookDir;

    public float maxFireAngle = 15.0f;

    void Start()
    {
        health = maxHealth;

        System.Random randy = new System.Random();
        int randomInt = randy.Next(possibleWeapons.Count);
        equippedWeapon = Instantiate(possibleWeapons[randomInt].gameObject).GetComponent<Weapon>();
        Destroy(equippedWeapon.GetComponent<Rotator>());

        equippedWeapon.transform.parent = weaponSlot;
        equippedWeapon.transform.localPosition = Vector3.zero;

        Destroy(equippedWeapon.GetComponent<Collider>());

        navMeshAgent = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player").GetComponent<Player>();
        playerHeight = player.gameObject.GetComponent<CapsuleCollider>().height;
    }
    void Update()
    {
        if (player.isDead)
        {
            this.enabled = false;
            return;
        }

        HandleMovementRaycasts();
        HandleRotationRaycasts();

        HandleMovement();
        HandleHeadRotation();

        HandleFiring();
    }

    void HandleMovementRaycasts()
    {
        raycastTimer += Time.deltaTime * Time.timeScale;
        if (raycastTimer >= rayCastMaxTime)
        {
            raycastTimer = 0;

            Vector3[] positions = getCriticalPlayerPositions();

            canSeePlayer = false;

            for (int i = 0; i < positions.Length; i++)
            {
                if (canSeePlayerPosition(positions[i]))
                {
                    canSeePlayer = true;
                    break;
                }
            }
        }
    }

    void HandleFiring()
    {
        if (canSeePlayer)
        {
            float angle = Vector3.Angle(eyesTransform.forward, targetLookDir);

            if (angle < maxFireAngle)
            {
                equippedWeapon.Fire();
            }
        }
    }

    void HandleRotationRaycasts()
    {
        targetLookDir = transform.forward; // Reset to default

        if (canSeePlayer)
        {
            foreach (var playerPos in getCriticalPlayerPositions())
            {
                if (canSeePlayerPosition(playerPos))
                {
                    targetLookDir = (playerPos - weaponSlot.position).normalized;
                    break;
                }
            }
        }
    }

    void HandleMovement()
    {
        if (canSeePlayer)
        {
            navMeshAgent.SetDestination(this.transform.position);
        }
        else
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    void HandleHeadRotation()
    {
        Quaternion lookDirQuat = Quaternion.LookRotation(targetLookDir, Vector3.up);
        Quaternion slerpedDir = Quaternion.Slerp(faceTransform.rotation, lookDirQuat, lookSlerpSpeed * Time.deltaTime * PlayerStats.GameDifficulty);
        transform.rotation = slerpedDir;
    }

    Vector3[] getCriticalPlayerPositions()
    {
        Vector3[] positions = new Vector3[3];
        float dirMulti = playerHeight * 0.5f * 0.8f;
        positions[0] = player.transform.position; // body
        positions[1] = positions[0] + Vector3.up * dirMulti; // eyes
        positions[2] = positions[0] - Vector3.up * dirMulti; // feet
        return positions;
    }

    bool canSeePlayerPosition(Vector3 playerPosition)
    {
        Vector3 dir = playerPosition - eyesTransform.position;
        float maxDistance = dir.magnitude + 1.0f;
        Ray r = new Ray(eyesTransform.position, dir);
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(r, out hitInfo, maxDistance, visibilityAllowedLayers);
        return hit && hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Player");
    }

    public void GetDamaged(float amount)
    {
        int amountInt = (int)(amount / PlayerStats.GameDifficulty);

        health -= amountInt;

        if (health <= 0)
        {
            PlayerStats.PlayerStatsSingleton.totalKills++;
            Destroy(this.gameObject);
        }
    }
}
