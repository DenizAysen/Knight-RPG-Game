using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMaterial : MonoBehaviour
{
    [SerializeField] private Material DisolveMat;
    private void Start()
    {
        GetComponent<Renderer>().material = DisolveMat;
        GetComponent<SpawnEffect>().enabled = true;
    }

}
