using UnityEngine;
using System.Collections;

public class DJ_LaserBehavior : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag(DJ_Tag.DJ_Player.ToString()))
        {
            if (col.gameObject != this)
            {
                col.gameObject.GetComponent<DJ_Damageable>().isAlive = false;
                col.gameObject.GetComponent<DJ_Damageable>().deathBy = DJ_Death.ELECTROCUTED;
                DJ_Animation.shockPlayedOnce = true;
            }
        }
    }

}