using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSet : MonoBehaviour
{
    [Header("加载组件")]
    [Tooltip("窗口切换")]
    public Dropdown windowState;
    [Tooltip("分辨率切换")]
    public Dropdown ratioState;

    /// <summary>
    /// 分辨率连接符
    /// </summary>
    private char connectFlag = 'x';

    void Start()
    {
        List<string> windowStr = new List<string>() { "全屏", "窗口" };
        UpdateDropDownItem(windowState, Screen.fullScreen ? windowStr[0] : windowStr[1],
            windowStr);

        List<string> temp = new List<string>();
        
        foreach (var item in Screen.resolutions)
        {
            temp.Add(item.width+ connectFlag.ToString() + item.height);
        }
        string nowRation = Screen.width + connectFlag.ToString() + Screen.height;
        temp.Add(nowRation);
        UpdateDropDownItem(ratioState,nowRation,temp);
    }

    private void UpdateDropDownItem(Dropdown _dropdown, string present, List<string> showNames, List<Sprite> sprite_list = null)
    {
        _dropdown.options.Clear();
        Dropdown.OptionData temoData;
        for (int i = 0; i < showNames.Count; i++)
        {
            //给每一个option选项赋值
            temoData = new Dropdown.OptionData();
            temoData.text = showNames[i].ToString();

            if (sprite_list != null)
            {
                temoData.image = sprite_list[i];
            }

            _dropdown.options.Add(temoData);

            if (present == showNames[i].ToString())
            {
                _dropdown.value = i;
            }
        }
        _dropdown.RefreshShownValue();
    }

    public void OnClickSave()  //确认按钮
    {
        if (ratioState!=null)
        {
            string now = ratioState.options[ratioState.value].text;
            string[] ss = now.Split(new char[] { connectFlag },System.StringSplitOptions.RemoveEmptyEntries);
            int.TryParse(ss[0], out int width);
            int.TryParse(ss[1], out int height);
            Screen.SetResolution(width, height, windowState.value == 0);
            Debug.Log(width+"***"+height);
        }
    }

}
