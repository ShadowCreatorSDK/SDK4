using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardPrompt : MonoBehaviour
{
    private const int PROMPT_PER_PAGE = 5;

    private int totalPageCount;
    private int currentPageCount;
    private int wordCount;

    public PromptTurnBtn turnLeftBtn;
    public PromptTurnBtn turnRightBtn;
    public PromptItem[] promptItems;
    public RectTransform enteredRect;
    public Text alreadyInput;
    private Action<string> onPromptItemClick;

    public void Init(Action<string> onPromptItemClick)
    {
        for (int i = 0; i < promptItems.Length; i++)
        {
            PromptItem promptItem = promptItems[i];
            promptItem.onPromptItemClick = OnPromptItemClickCallback;
        }

        enteredRect.GetComponent<KeyboardKeydownBtn>().onClick.RemoveAllListeners();
        enteredRect.GetComponent<KeyboardKeydownBtn>().onClick.AddListener(OnEnteredPromptItemClickCallback);

        turnLeftBtn.onClick.RemoveAllListeners();
        turnLeftBtn.onClick.AddListener(TurnLeft);
        turnRightBtn.onClick.RemoveAllListeners();
        turnRightBtn.onClick.AddListener(TurnRight);

        this.onPromptItemClick = onPromptItemClick;
    }

    public int GetChinese(string englishWords)
    {
        wordCount = ChineseIMEManager.Instance.GetWordCount(englishWords);

        currentPageCount = 0;
        totalPageCount = wordCount / PROMPT_PER_PAGE;

        RefreshPromptItem();
        SetTrunBtnVisiable();
        return wordCount;
    }

    public void TurnLeft()
    {
        currentPageCount--;
        RefreshPromptItem();
        SetTrunBtnVisiable();
    }

    // Update is called once per frame
    public void TurnRight()
    {
        currentPageCount++;
        RefreshPromptItem();
        SetTrunBtnVisiable();
    }

    public string GetFirstChineseWord()
    {
        PromptItem firstPromptItem = promptItems[0];
        return firstPromptItem.chineseWord;
    }

    public void SetEnteredText(string textStr)
    {
        alreadyInput.text = textStr;
    }

    public void SetChineseFirstEnteredText()
    {
        string textStr =  GetFirstChineseWord();
        textStr = string.IsNullOrEmpty(textStr) ? string.Empty : textStr;
        Text chineseAlreadyInput = enteredRect.GetComponentInChildren<Text>();
        float sizeY = enteredRect.sizeDelta.y;
        float sizeX = KeyboardUtils.CaculateTextLength(textStr , enteredRect.GetComponentInChildren<Text>());
        enteredRect.sizeDelta = new Vector2(sizeX + 160, sizeY);
        LayoutRebuilder.ForceRebuildLayoutImmediate(enteredRect);
        chineseAlreadyInput.text = textStr;
    }

    private void RefreshPromptItem()
    {
        int startIndex = currentPageCount * PROMPT_PER_PAGE;
        int endIndex = totalPageCount != currentPageCount ? startIndex + PROMPT_PER_PAGE - 1 : startIndex + wordCount % PROMPT_PER_PAGE - 1;
        int loopWordCount = endIndex - startIndex;

        for (int i = 0; i < promptItems.Length; i++)
        {
            int wordIndex = startIndex + i;
            PromptItem promptItem = promptItems[i];
            if (i <= loopWordCount)
            {
                string chineseWord = ChineseIMEManager.Instance.GetWord(wordIndex);
                promptItem.SetChineseWord(chineseWord, wordIndex);
            }
            else
            {
                promptItem.SetChineseWord(null, -1);
            }
        }
        SetChineseFirstEnteredText();
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
    }

    private void SetTrunBtnVisiable()
    {
        if (currentPageCount == 0)
        {
            turnLeftBtn.gameObject.SetActive(false);
        }
        else
        {
            turnLeftBtn.gameObject.SetActive(true);
        }

        if (currentPageCount == totalPageCount)
        {
            turnRightBtn.gameObject.SetActive(false);
        }
        else
        {
            turnRightBtn.gameObject.SetActive(true);
        }
    }

    private void OnEnteredPromptItemClickCallback()
    {
        string firstChineseWord = GetFirstChineseWord();
        onPromptItemClick?.Invoke(firstChineseWord);
    }

    private void OnPromptItemClickCallback(string chineseWord)
    {
        onPromptItemClick?.Invoke(chineseWord);
    }
}
