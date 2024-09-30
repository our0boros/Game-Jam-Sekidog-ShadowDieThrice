using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Player Settings")]
    public int playerNumber = 1;
    public KeyCode shootKey = KeyCode.Space;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float cooldownTime = 0.5f;
    [Header("Ϊrecorder��¼���")]
    public bool shootABullet = false;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip noAmmoSound;

    private float lastShootTime;
    private AudioSource audioSource;
    private RecordPlayerData recordPlayerData;
    private ReplayPlayerData replayPlayerData;
    private PlayerHealth playerHealth;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && (shootSound != null || noAmmoSound != null))
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        playerHealth = GetComponent<PlayerHealth>();
        recordPlayerData = GetComponent<RecordPlayerData>();
        replayPlayerData = GetComponent<ReplayPlayerData>();
    }

    private void Update()
    {
        if (replayPlayerData == null || !replayPlayerData.replay)
        {
            // ֻ�е�ǰ�غϵ���Ҳ�����Ӧ����
            if (Input.GetKeyDown(shootKey) && Time.time > lastShootTime + cooldownTime && recordPlayerData.gameManager.isGameStart)
            {
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        bool isShadow = replayPlayerData != null && replayPlayerData.replay;
        bool isDead = isShadow && !playerHealth.isAlive; // ����Ӱ�ӵ�����״̬�������ͬ

        if (isDead)
        {
            // ��������Ӱ�Ӳ������
            PlayNoAmmoSound();
            return;
        }

        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
            if (bulletBehavior != null)
            {
                bulletBehavior.playerNumber = playerNumber;
                bulletBehavior.SetShooterInfo(playerHealth.isAlive, isShadow, isDead);
            }

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = firePoint.forward * bulletSpeed;
            }

            PlayShootSound();
            shootABullet = true;
            lastShootTime = Time.time;
        }
        else
        {
            Debug.LogWarning("Bullet prefab or fire point is not set!");
        }
    }

    private void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    private void PlayNoAmmoSound()
    {
        if (audioSource != null && noAmmoSound != null)
        {
            audioSource.PlayOneShot(noAmmoSound);
        }
    }
}