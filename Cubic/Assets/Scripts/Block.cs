using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Dirt,
    Stone,
    Ore
}

public class Block : MonoBehaviour
{

    public BlockType blockType;    

    public float BlockResistance
    {
        get
        {
            return _blockResistance;
        }
    }

    [SerializeField]
    private float _blockResistance = 30f;

    public int ID
    {
        get
        {
            return _id;
        }
    }

    [SerializeField]
    private int _id;

    public void IdSetter(int newId)
    {
        _id = newId;
    }
}
