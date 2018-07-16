using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksManager : Singleton<BlocksManager> {

    public List<GameObject> dropItems;
    //Dirt 0
    //Stone 1
    //Ore 2

    public void DropItem(BlockType blockType, Vector3 position, Quaternion rotation)
    {
        switch (blockType)
        {
            case BlockType.Dirt:
                Instantiate(dropItems[0], position, rotation);
                break;
            case BlockType.Stone:
                Instantiate(dropItems[0], position, rotation);
                break;
            case BlockType.Ore:
                Instantiate(dropItems[0], position, rotation);
                break;
            default:
                break;
        }
    }
}
