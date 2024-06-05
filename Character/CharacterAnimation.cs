using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        // ��ȡAnimator���
        animator = GetComponent<Animator>();
    }

    // �ͷż���
    public void CastAttack()
    {
        // ���ݼ�������������Ӧ�ļ��ܶ���
        animator.SetBool("Attack", true);
        animator.SetBool("Idle", false);
    }

    // �����¼������ܶ���������ϣ����ص�����״̬
    public void ReturnToIdleState()
    {
        // ���ô������򲼶���������Animator���ɵ�����״̬����
        animator.SetBool("Idle", true);
        animator.SetBool("Attack", false);
    }

    public float shakeDuration = 0.2f; // �𶯳���ʱ��  
    public float shakeMagnitude = 0.05f; // �𶯷���  
    public int shakeInterval = 2;
    private int shakeCount;
    private Vector3 originalPosition;
    private bool isShaking = false;

    // ������������ڵ����ܵ�����ʱ������  
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
        originalPosition = transform.localPosition; // ����ԭʼλ��  

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            if (shakeCount <= 0)
            {
                float xShake = Random.Range(-shakeMagnitude, shakeMagnitude);
                float yShake = Random.Range(-shakeMagnitude, shakeMagnitude);

                // ֱ���޸�λ����ģ����  
                transform.localPosition = new Vector3(originalPosition.x + xShake, originalPosition.y + yShake, originalPosition.z);
                shakeCount = shakeInterval;
            }
            yield return new WaitForEndOfFrame(); // ��ÿ֡����ʱ����λ��  
            elapsed += Time.deltaTime;
            shakeCount--;
        }

        // �𶯽�����ָ�ԭʼλ��  
        transform.localPosition = originalPosition;
        isShaking = false;
    }
}
