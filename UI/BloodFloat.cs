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
    public float damageTextDuration = 1f;//����ʱ��                          
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

        // �����˺������ı�����
        damageText.text = damage;

        // ��ʼЭ�̣�ʹ�˺����ֽ���б���˶�������ʧ
        StartCoroutine(MoveAndFade(damageTextGO, damageTextDuration));
    }

    private IEnumerator MoveAndFade(GameObject obj, float duration)
    {
        float timer = 0f;
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        Vector2 startPos = rectTransform.anchoredPosition; // ʹ��anchoredPosition��localPosition  
        Vector2 endPos = new Vector2(startPos.x + 100f, startPos.y + 100f);

        while (timer < duration)
        {
            timer += Time.deltaTime;

            // ʹ��Lerp��ƽ���ƶ�UIԪ�ص�Ŀ��λ��  
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, timer / duration);

            // ��������ʧ
            Color textColor = obj.GetComponent<Text>().color;
            textColor.a = Mathf.Lerp(1f, 0f, timer / duration);
            obj.GetComponent<Text>().color = textColor;

            yield return null;
        }

        // �����˺�����Ԥ����
        Destroy(obj);
    }

    public void ShowBuff(BuffInfo buffInfo)
    {
        GameObject buffTextGO = Instantiate(buffTextPrefab, buffInfo.target.transform);
        buffTextGO.transform.SetParent(GameObject.Find("Canvas").transform, true);

        // ������ʼλ��
        RectTransform rectTransform = buffTextGO.GetComponent<RectTransform>();
        rectTransform.anchoredPosition += Vector2.up * 100f; // ����ƫ��50����λ


        Text buffText = buffTextGO.GetComponent<Text>();
        buffText.text = buffInfo.buffData.buffName;
        StartCoroutine(MoveUpAndFade(buffTextGO, damageTextDuration));
    }

    private IEnumerator MoveUpAndFade(GameObject obj, float duration)
    {
        float timer = 0f;
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        Vector2 startPos = rectTransform.anchoredPosition; // ʹ��anchoredPosition��localPosition  
        Vector2 endPos = startPos + Vector2.up * 100f; // �����˶�

        while (timer < duration)
        {
            timer += Time.deltaTime;

            // ʹ��Lerp��ƽ���ƶ�UIԪ�ص�Ŀ��λ��  
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, timer / duration);

            // ��������ʧ
            Color textColor = obj.GetComponent<Text>().color;
            textColor.a = Mathf.Lerp(1f, 0f, timer / duration);
            obj.GetComponent<Text>().color = textColor;

            yield return null;
        }

        // ����Buff����Ԥ����
        Destroy(obj);
    }
}
