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
            // 根据玩家的移动情况设置动画的isRunning布尔值
            bool isRunning = playerMovement.movement != Vector3.zero;
            anim.SetBool("isRunning", isRunning);

            // 根据玩家编号和输入设置动画的isAttack布尔值
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