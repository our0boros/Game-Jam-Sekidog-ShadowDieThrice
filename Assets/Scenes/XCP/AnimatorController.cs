using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private PlayerMovementCC playerMovement;
    private PlayerShooting playerShooting;
    void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponentInParent<PlayerMovementCC>();
        playerShooting = GetComponentInParent<PlayerShooting>();
    }

    void Update()
    {
        if (playerMovement != null)
        {
            // ������ҵ��ƶ�������ö�����isRunning����ֵ
            bool isRunning = playerMovement.movement != Vector3.zero;
            anim.SetBool("isRunning", isRunning);

            // ������ұ�ź��������ö�����isAttack����ֵ
            bool isAttack = false;
            if (playerMovement.playerNumber == 1)
            {
                isAttack = Input.GetKeyDown(playerShooting.shootKey);
            }
            else if (playerMovement.playerNumber == 2)
            {
                isAttack = Input.GetKeyDown(playerShooting.shootKey);
            }
            anim.SetBool("isAttack", isAttack);
        }
    }
}