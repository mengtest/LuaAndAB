using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LuaItemTest : MonoBehaviour
{
    [SerializeField]
    private Text m_btnText;

    private System.Action _onClick;
    public void Set(string _buttonText, System.Action _callBack)
    {
        //绑定按钮事件
        _onClick = _callBack;
        //绑定显示的文字
        m_btnText.text = _buttonText;
    }


    public void OnItemClick()
    {
        _onClick?.Invoke();
    }


}
