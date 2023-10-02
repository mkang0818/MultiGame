using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject InGameManager;

    public GameObject target;
    public Vector3 offset;
    void Start()
    {

    }
    void Update()
    {
        //print(InGameManager);
        //print(InGameManager.GetComponent<InGameManager>().PlayerObj);
        target = InGameManager.GetComponent<InGameManager>().PlayerObj;
        if (target != null)
        {
            transform.position = target.transform.position + offset;
        }
    }
}
