using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
public class BloodFloat : MonoBehaviour
{
    public static BloodFloat instance;

    public GameObject buffTextPrefab;
    public GameObject damageTextPrefab;
    public float yOffset;
    public float damageTextDuration = 1f;//持续时间                          
    public float initialVelocity = 5f;
    public float fadeSpeed = 1f;

    public void Awake()
    {
        instance = this;
    }

    public void ShowDamage(string damage, GameObject target)
    {
        GameObject damageTextGO = Instantiate(damageTextPrefab, target.transform);
        damageTextGO.transform.SetParent(GameObject.Find("Canvas").transform, true);
        Text damageText = damageTextGO.GetComponent<Text>();

        // 设置伤害数字文本内容
        damageText.text = damage;

        // 开始协程，使伤害数字进行斜抛运动和逐渐消失
        StartCoroutine(MoveAndFade(damageTextGO, damageTextDuration));
    }

    private IEnumerator MoveAndFade(GameObject obj, float duration)
    {
        float timer = 0f;
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        Vector2 startPos = rectTransform.anchoredPosition; // 使用anchoredPosition或localPosition  
        Vector2 endPos = new Vector2(startPos.x + 100f, startPos.y + 100f);

        while (timer < duration)
        {
            timer += Time.deltaTime;

            // 使用Lerp来平滑移动UI元素到目标位置  
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, timer / duration);

            // 计算逐渐消失
            Color textColor = obj.GetComponent<Text>().color;
            textColor.a = Mathf.Lerp(1f, 0f, timer / duration);
            obj.GetComponent<Text>().color = textColor;

            yield return null;
        }

        // 销毁伤害数字预制体
        Destroy(obj);
    }

    public void ShowBuff(BuffInfo buffInfo)
    {
        GameObject buffTextGO = Instantiate(buffTextPrefab, buffInfo.target.transform);
        buffTextGO.transform.SetParent(GameObject.Find("Canvas").transform, true);

        // 调整初始位置
        RectTransform rectTransform = buffTextGO.GetComponent<RectTransform>();
        rectTransform.anchoredPosition += Vector2.up * 100f; // 向上偏移50个单位


        Text buffText = buffTextGO.GetComponent<Text>();
        buffText.text = buffInfo.buffData.buffName;
        StartCoroutine(MoveUpAndFade(buffTextGO, damageTextDuration));
    }

    private IEnumerator MoveUpAndFade(GameObject obj, float duration)
    {
        float timer = 0f;
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        Vector2 startPos = rectTransform.anchoredPosition; // 使用anchoredPosition或localPosition  
        Vector2 endPos = startPos + Vector2.up * 100f; // 向上运动

        while (timer < duration)
        {
            timer += Time.deltaTime;

            // 使用Lerp来平滑移动UI元素到目标位置  
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, timer / duration);

            // 计算逐渐消失
            Color textColor = obj.GetComponent<Text>().color;
            textColor.a = Mathf.Lerp(1f, 0f, timer / duration);
            obj.GetComponent<Text>().color = textColor;

            yield return null;
        }

        // 销毁Buff名称预制体
        Destroy(obj);
    }
}
