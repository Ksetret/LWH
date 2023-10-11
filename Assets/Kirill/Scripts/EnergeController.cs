using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergeController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            EnergePlayer.instance.EnergeBuster();
        Destroy(this.gameObject);
    }
}
