using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour {
    [SerializeField] private Transform player;

    public static AIManager I { get; private set; }
	// Use this for initialization
	void Awake () {
		if(I != null)
        {
            Debug.LogError("AIManager already exists!");
        }

        I = this;
	}
	
    public static Transform GetPlayerTransform()
    {
        return I.player;
    }
}
