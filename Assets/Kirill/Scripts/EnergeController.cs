using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergeController : MonoBehaviour
{
    [SerializeField] private GameObject _mesh;
    [SerializeField] private AudioSource _sphereSpound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _sphereSpound.Play();
            Destroy(_mesh);
            Destroy(this.gameObject, 1);
            EnergePlayer.instance.EnergeBuster();
        }
       
    }
}
