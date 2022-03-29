using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Reset()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition =new Vector3(91.85f,0,100) - new Vector3(0, 0, 6 * i);
        }
    }
}
