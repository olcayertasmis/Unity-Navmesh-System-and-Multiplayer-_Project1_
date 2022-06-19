using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static bool Instance = false;
    void Start()
    {
        if(!Instance){
            DontDestroyOnLoad(this.gameObject);
            Instance = true;
        }else{
            Destroy(this.gameObject);
        }
    }
}
