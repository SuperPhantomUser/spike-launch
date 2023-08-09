using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NormalButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public float change;

    private RectTransform Rect;
    private bool buttonPressed;
    private float sizeY;
    private GameObject OurText;
    private float textX;
    private float textY;

    // Start is called before the first frame update
    void Start()
    {
        Rect = GetComponent<RectTransform>();
        sizeY = Rect.sizeDelta.y;
        OurText = transform.Find("Text (TMP)").gameObject;
        textX = OurText.GetComponent<RectTransform>().anchoredPosition.x;
        textY = OurText.GetComponent<RectTransform>().anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPressed)
        {
            Rect.sizeDelta = new Vector2(Rect.sizeDelta.x, sizeY - 2f);
            OurText.GetComponent<RectTransform>().anchoredPosition = new Vector2(textX, textY - (1f + change));

        }
        else
        {
            Rect.sizeDelta = new Vector2(Rect.sizeDelta.x, sizeY);
            OurText.GetComponent<RectTransform>().anchoredPosition = new Vector2(textX, textY);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}
