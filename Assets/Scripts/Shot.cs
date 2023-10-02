using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public Transform ShotPos;
    public GameObject projectile;

    public void ShotEvent()
    {
        var bullet = Instantiate(projectile, ShotPos.transform.position, ShotPos.transform.rotation);
    }
}
