using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image hpImgRed;
    public Image hpImgWhite;

    public Character character;

    public float bufftime = 0.5f;//¿ÛÑªËÙ¶È

    private Coroutine updateCoroutine;

    private void Start()
    {
        character = gameObject.GetComponent<Character>();
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        hpImgRed.fillAmount = (float)character.currentHp / (float)character.maxHp;

        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }

        updateCoroutine = StartCoroutine(UpdateHpEffect());
    }

    private IEnumerator UpdateHpEffect()
    {
        float effectLength = hpImgWhite.fillAmount - hpImgRed.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < bufftime && effectLength != 0)
        {
            elapsedTime += Time.deltaTime;
            hpImgWhite.fillAmount = Mathf.Lerp(hpImgRed.fillAmount + effectLength, hpImgRed.fillAmount, elapsedTime / bufftime);
            yield return null;
        }

        hpImgWhite.fillAmount = hpImgRed.fillAmount;
    }
}
