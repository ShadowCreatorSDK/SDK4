using SC.XR.Unity;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class SCKeyboardMono : MonoBehaviour
{
    public GameObject keyboard_num;
    public GameObject keyboard_symbol;
    public GameObject keyboard_enUp;
    public GameObject keyboard_enLow;
    public GameObject keyboard_cn;

    public Action OnDoneButtonClick;
    public Action<string> OnTextChange;

    public StringBuilder stringBuilder;
    private StringBuilder preInputStringBuilder;

    public KeyboardPrompt keyboardPrompt;

    public void Initialize()
    {
        if (stringBuilder == null)
        {
            stringBuilder = new StringBuilder();
        }

        if (preInputStringBuilder == null)
        {
            preInputStringBuilder = new StringBuilder();
        }

        keyboardPrompt.Init(OnChineseCharacterClick);

        ShowNum();
    }

    public void OnChineseCharacterClick(string value)
    {
        preInputStringBuilder.Clear();
        stringBuilder.Append(value);
        HideChinesePrompt();
        SetTextString();
    }

    public void OnPinyinKeyClick(string value)
    {
        preInputStringBuilder.Append(value);
        int wordCount = GetChinese(preInputStringBuilder.ToString());
        if (wordCount != 0)
        {
            ShowChinesePrompt();
        }
        else
        {
            if (preInputStringBuilder.Length != 0)
            {
                stringBuilder.Append(preInputStringBuilder.ToString());
                preInputStringBuilder.Clear();
            }
        }
        SetTextString();
    }

    public void OnNormalKeyClick(string value)
    {
        if (preInputStringBuilder.Length != 0)
        {
            stringBuilder.Append(preInputStringBuilder.ToString());
            preInputStringBuilder.Clear();
            HideChinesePrompt();
        }
        stringBuilder.Append(value);
        SetTextString();
    }

    public void OnDoneClick()
    {
        stringBuilder.Append(preInputStringBuilder.ToString());
        preInputStringBuilder.Clear();
        HideChinesePrompt();
        OnDoneButtonClick?.Invoke();
    }

    public void OnClearKeyClick()
    {
        preInputStringBuilder.Clear();
        stringBuilder.Clear();
        HideChinesePrompt();
        SetTextString();
    }

    public void OnDeleteKeyClick()
    {
        if (preInputStringBuilder.Length > 0)
        {
            preInputStringBuilder.Remove(preInputStringBuilder.Length - 1, 1);
            if (preInputStringBuilder.Length != 0)
            {
                GetChinese(preInputStringBuilder.ToString());
            }
            else
            {
                HideChinesePrompt();
            }
            SetTextString();
            return;
        }

        HideChinesePrompt();
        if (stringBuilder.Length > 0)
        {
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            SetTextString();
        }
    }

    public void OnSpaceClick()
    {
        if (preInputStringBuilder.Length > 0)
        {
            preInputStringBuilder.Clear();
            string firstChineseWord = GetFirstChineseWord();
            if (!string.IsNullOrEmpty(firstChineseWord))
            {
                stringBuilder.Append(GetFirstChineseWord());
            }
            HideChinesePrompt();
            SetTextString();
            return;
        }

        stringBuilder.Append(" ");
        SetTextString();
    }

    //for CN IME
    public void OnShiftClick()
    {
        if (preInputStringBuilder.Length != 0)
        {
            stringBuilder.Append(preInputStringBuilder.ToString());
            preInputStringBuilder.Clear();
            HideChinesePrompt();
            SetTextString();
        }
    }

    public void ShowNum()
    {
        if (preInputStringBuilder.Length != 0)
        {
            stringBuilder.Append(preInputStringBuilder.ToString());
            preInputStringBuilder.Clear();
            HideChinesePrompt();
            SetTextString();
        }

        keyboard_num.SetActive(true);
        keyboard_symbol.SetActive(false);
        keyboard_enUp.SetActive(false);
        keyboard_enLow.SetActive(false);
        keyboard_cn.SetActive(false);
    }

    public void ShowSymbol()
    {
        keyboard_num.SetActive(false);
        keyboard_symbol.SetActive(true);
        keyboard_enUp.SetActive(false);
        keyboard_enLow.SetActive(false);
        keyboard_cn.SetActive(false);
    }

    //Shift
    public void ShowEnUp()
    {
        keyboard_num.SetActive(false);
        keyboard_symbol.SetActive(false);
        keyboard_enUp.SetActive(true);
        keyboard_enLow.SetActive(false);
        keyboard_cn.SetActive(false);
    }

    public void ShowEnLow()
    {
        if (preInputStringBuilder.Length != 0)
        {
            stringBuilder.Append(preInputStringBuilder.ToString());
            preInputStringBuilder.Clear();
            HideChinesePrompt();
            SetTextString();
        }

        keyboard_num.SetActive(false);
        keyboard_symbol.SetActive(false);
        keyboard_enUp.SetActive(false);
        keyboard_enLow.SetActive(true);
        keyboard_cn.SetActive(false);
    }

    public void ShowCN()
    {
        keyboard_num.SetActive(false);
        keyboard_symbol.SetActive(false);
        keyboard_enUp.SetActive(false);
        keyboard_enLow.SetActive(false);
        keyboard_cn.SetActive(true);
    }

    public virtual void SetTextString()
    {
        string result = stringBuilder.ToString() + preInputStringBuilder.ToString();
        OnTextChange?.Invoke(result);
    }

    protected virtual int GetChinese(string englishWord)
    {
        return keyboardPrompt.GetChinese(englishWord);
    }

    protected void ShowChinesePrompt()
    {
        keyboardPrompt.gameObject.SetActive(true);
    }

    protected void HideChinesePrompt()
    {
        keyboardPrompt.gameObject.SetActive(false);
    }

    protected string GetFirstChineseWord()
    {
        return keyboardPrompt.GetFirstChineseWord();
    }
}
