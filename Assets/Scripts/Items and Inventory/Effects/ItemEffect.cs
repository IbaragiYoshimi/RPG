using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform _enemyPosition)
    {
        Debug.Log("Execute item's effect and target is _enemy!");
    }

    public virtual void ExecuteEffect()
    {
        Debug.Log("Execute item's effect! Target is yourself.");
    }
}
