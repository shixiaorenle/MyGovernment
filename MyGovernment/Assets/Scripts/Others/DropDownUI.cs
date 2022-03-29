using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownUI : Define
{
    public List<string> listItem = new List<string>();
    public Dropdown dropdown;
    public Text label;//选定的下拉框
    public RectTransform template;
    void Start()
    {
        //UpdateDropDownItem( listItem[0], listItem);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateDropDownItem( string present, List<string> showNames, List<Sprite> sprite_list = null)
    {
        dropdown.options.Clear();
        template.sizeDelta = new Vector2(template.sizeDelta.x,128* (showNames.Count+1));
        Dropdown.OptionData temoData;
        for (int i = 0; i < showNames.Count; i++)
        {
            //给每一个option选项赋值
            temoData = new Dropdown.OptionData();
            temoData.text = showNames[i].ToString();

            if (sprite_list != null)
            { temoData.image = sprite_list[i]; }
            dropdown.options.Add(temoData);
            if (present == showNames[i].ToString())
            {
                //_dropdown.captionText.text = showNames[i].ToString();
                dropdown.value = i;
            }
        }

    }
    public void OnValueChange( )
    {
        dropdown.captionText = label;
        label.fontSize = 28;
        label.color = Color.white;
    }
    public void ReFresh()
    {
        //dropdown.captionText = null;
        //label.fontSize = 24;
        //label.color = new Color(184 / 255f, 185 / 255f, 194 / 255f, 1);
        //label.text = "报警等级筛选";
        dropdown.value = 0;
    }
}
