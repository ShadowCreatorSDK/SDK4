using System;
using UnityEngine;
using UnityEngine.UI;

public class PromptItem : KeyboardKeydownBtn
{
    [HideInInspector]
    public Action<string> onPromptItemClick;

    [HideInInspector]
    public string chineseWord;

    public void SetChineseWord(string chineseWord, int index)
    {
        this.chineseWord = chineseWord;
        Text itemText = this.GetComponentInChildren<Text>();
        RectTransform itemRect = this.GetComponent<RectTransform>();
        float sizeY = itemRect.sizeDelta.y;
        if (string.IsNullOrEmpty(chineseWord))
        {
            itemText.text = "";
            SetRectSize(itemRect, 0, sizeY);
            return;
        }

        string displayWord = string.Format("{0}.{1}", (index + 1).ToString(), chineseWord);
        int wordLength = KeyboardUtils.CaculateTextLength(displayWord + 20, itemText);
        itemText.text = displayWord;
        SetRectSize(itemRect, wordLength, sizeY);

        this.onClick.RemoveAllListeners();
        this.onClick.AddListener(OnPromptItemClick);
    }

    private void SetRectSize(RectTransform rectTransform, float x, float y)
    {
        rectTransform.sizeDelta = new Vector2(x, y);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    private void OnPromptItemClick()
    {
        onPromptItemClick?.Invoke(this.chineseWord);
    }
}
