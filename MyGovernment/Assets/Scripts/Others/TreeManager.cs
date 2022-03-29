using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TreeManager : MonoBehaviour
{
    public GameObject[] trees;
    public Transform treess;
    int a = 0;
    void Start()
    {
        
    }

    private void Reset()
    {
        
    }
    void Update()
    {
        if (a<1)
        {
            for (int i = 0; i < treess.childCount; i++)
            {
                int x = Random.Range(0, trees.Length);
                GameObject o = Instantiate(trees[x], transform);
                o.transform.position = treess.GetChild(i).position;

            }
            a++;

        }
        
    }
}
