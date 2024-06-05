using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        // 获取Animator组件
        animator = GetComponent<Animator>();
    }

    // 释放技能
    public void CastAttack()
    {
        // 根据技能索引播放相应的技能动画
        animator.SetBool("Attack", true);
        animator.SetBool("Idle", false);
    }

    // 动画事件：技能动画播放完毕，返回到空闲状态
    public void ReturnToIdleState()
    {
        // 设置触发器或布尔参数，让Animator过渡到空闲状态动画
        animator.SetBool("Idle", true);
        animator.SetBool("Attack", false);
    }

    public float shakeDuration = 0.2f; // 震动持续时间  
    public float shakeMagnitude = 0.05f; // 震动幅度  
    public int shakeInterval = 2;
    private int shakeCount;
    private Vector3 originalPosition;
    private bool isShaking = false;

    // 假设这个方法在敌人受到攻击时被调用  
    public void OnHit()
    {
        if (!isShaking)
        {
            shakeCount = shakeInterval;
            StartCoroutine(ShakeEffect());
        }
    }

    IEnumerator ShakeEffect()
    {
        isShaking = true;
        originalPosition = transform.localPosition; // 保存原始位置  

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            if (shakeCount <= 0)
            {
                float xShake = Random.Range(-shakeMagnitude, shakeMagnitude);
                float yShake = Random.Range(-shakeMagnitude, shakeMagnitude);

                // 直接修改位置来模拟震动  
                transform.localPosition = new Vector3(originalPosition.x + xShake, originalPosition.y + yShake, originalPosition.z);
                shakeCount = shakeInterval;
            }
            yield return new WaitForEndOfFrame(); // 在每帧结束时更新位置  
            elapsed += Time.deltaTime;
            shakeCount--;
        }

        // 震动结束后恢复原始位置  
        transform.localPosition = originalPosition;
        isShaking = false;
    }
}
