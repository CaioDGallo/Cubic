using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWorld : Singleton<GenerateWorld> {

    public List<Block> blocks;

    private void Awake()
    {
        
    }

    private void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            blocks.Add(gameObject.transform.GetChild(i).GetComponent<Block>());
            blocks[i].IdSetter(i + 1);
        }
    }
}
