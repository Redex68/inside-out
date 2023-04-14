using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class QuizButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    TMP_Text text;

    bool blockEvents = false; 

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    public void setAnswer(string answer)
    {
        text.text = answer;
        text.color = Color.white;
        blockEvents = false;
        autoResize();
    }

    public void mark(bool correct)
    {
        text.color = correct ? Color.green : Color.red;
    }

    public void setBlockEvents(bool block) { blockEvents = block; }

    public void OnPointerExit(PointerEventData ped)
    {
        if(blockEvents) return;

        GetComponentInChildren<TMP_Text>().color = Color.white;
    }

    public void OnPointerEnter(PointerEventData ped)
    {
        if(blockEvents) return;

        GetComponentInChildren<TMP_Text>().color = Color.blue;
    }

    void autoResize()
    {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, text.preferredWidth);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, text.preferredHeight);
        text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, text.preferredWidth);
        text.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, text.preferredHeight);
    }
}
