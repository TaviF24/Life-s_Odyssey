using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*--------Singleton--------*/

    public static GameManager instance = null;

    private void Awake()
    {
        instance = this;
    }

    public GameObject player;
}
