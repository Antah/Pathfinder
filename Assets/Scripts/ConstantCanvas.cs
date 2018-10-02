using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantCanvas : MonoBehaviour {

    public static ConstantCanvas instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
