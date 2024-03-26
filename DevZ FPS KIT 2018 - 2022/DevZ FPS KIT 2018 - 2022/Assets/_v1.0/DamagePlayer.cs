using UnityEngine;
using System.Collections;

[System.Serializable]
public class DamagePlayer : MonoBehaviour
{
    public void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Collider>().tag == "Player")
        {
            HealthControllerPlayer player = col.GetComponent<HealthControllerPlayer>();
            if (player)
            {
                player.ApplyDamage(10000);
            }
        }
    }

}