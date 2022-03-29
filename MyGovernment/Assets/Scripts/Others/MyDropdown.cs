using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MyDropdown : Dropdown
{
    protected override GameObject CreateDropdownList(GameObject template)
    {
        GameObject o = Instantiate(template);
        o.GetComponent<RectTransform>().sizeDelta = template.GetComponent<RectTransform>().sizeDelta;
        return o;
    }
}
