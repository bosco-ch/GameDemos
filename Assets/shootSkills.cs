using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class shootSkills : MonoBehaviour
{

    public float damage = 10;
    private void OnTriggerStay2D(Collider2D other)
    {
        // other.
        if (other.gameObject.layer == LayerMask.NameToLayer("emeny"))
        {
            other.GetComponent<Character>().taskDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
