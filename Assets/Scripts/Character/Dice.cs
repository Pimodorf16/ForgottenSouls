using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public Soul soul;

    public int Roll()
    {
        int result = Random.Range(1, 7);
        return result;
    }
}
