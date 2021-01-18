using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game3DInputField : MonoBehaviour
{
    [SerializeField]
    private TextMesh placeholderCompontent;
    [SerializeField]
    private TextMesh textCompontent; 
    private string _text;
    [SerializeField] 
    public int maxLength;//字符数量

    public bool isPass;
    public string text
    {
        set
        {
            _text = value;
            if (isPass)
            {
                textCompontent.text = "";
                for (int i = 0; i < _text.Length; i++)
                {
                    textCompontent.text += "*";
                }
                placeholderCompontent.gameObject.SetActive(_text == string.Empty);
            }
            else
            {
                textCompontent.text = _text;
                placeholderCompontent.gameObject.SetActive(_text == string.Empty);
            }
        }
        get
        {
            if (_text == null)
                _text = "";
            return _text;
        }
    }
}
