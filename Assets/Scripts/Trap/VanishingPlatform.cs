using System.Collections;
using UnityEngine;

//TP2 - Fernando Claro

public class VanishingPlatform : MonoBehaviour, ITrap<GameObject>
{
    public float vanishTime = 2f;
    private bool isVanishing = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isVanishing)
        {
            isVanishing = true;
            TrapManager.Instance.TriggerTrap(this, gameObject);
            StartCoroutine(VanishPlatform());
        }
    }

    private IEnumerator VanishPlatform()
    {
        yield return new WaitForSeconds(vanishTime);
        TrapManager.Instance.ReleaseTrap(this, gameObject);
        gameObject.SetActive(false);
    }

    public void ActivateTrap(GameObject target)
    {

    }

    public void DeactivateTrap(GameObject target)
    {

    }
}


