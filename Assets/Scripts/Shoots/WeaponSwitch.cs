using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TP2 - Fernando Claro

public class WeaponSwitch : MonoBehaviour
{
    public string weapon1Tag = "Weapon1"; // Tag de la pistola
    public string weapon2Tag = "Weapon2"; // Tag del rifle

    private GameObject[] weapon1Objects; // Objetos asociados al tag de la pistola
    private GameObject[] weapon2Objects; // Objetos asociados al tag del rifle

    private Shot weapon1Shot; // Script de disparo de la pistola
    private Shot weapon2Shot; // Script de disparo del rifle

    private bool isWeapon1Active = true;

    private void Start()
    {
        weapon1Objects = GameObject.FindGameObjectsWithTag(weapon1Tag);
        weapon2Objects = GameObject.FindGameObjectsWithTag(weapon2Tag);

        weapon1Shot = GetShotScript(weapon1Objects);
        weapon2Shot = GetShotScript(weapon2Objects);

        SetWeaponActive(weapon1Objects, true);
        SetWeaponActive(weapon2Objects, false);
    }

    private void Update()
    {
        // Detectar las teclas para cambiar de arma
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(true); // Cambiar a la pistola
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(false); // Cambiar al rifle
        }

        // Detectar el botón para disparar
        if (Input.GetMouseButtonDown(0))
        {
            if (isWeapon1Active)
            {
                if (!weapon1Shot.IsOnCooldown())
                {
                    weapon1Shot.ThrowBullet();
                    weapon1Shot.StartCooldown();
                }
            }
            else
            {
                if (!weapon2Shot.IsOnCooldown())
                {
                    weapon2Shot.ThrowBullet();
                    weapon2Shot.StartCooldown();
                }
            }
        }
    }

    private void SwitchWeapon(bool toWeapon1)
    {
        isWeapon1Active = toWeapon1; // Cambiar el estado del arma activa

        SetWeaponActive(weapon1Objects, isWeapon1Active); // Activar o desactivar la pistola según el estado
        SetWeaponActive(weapon2Objects, !isWeapon1Active); // Activar o desactivar el rifle según el estado
    }

    private void SetWeaponActive(GameObject[] weapons, bool isActive)
    {
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(isActive);
        }
    }

    private Shot GetShotScript(GameObject[] weapons)
    {
        foreach (GameObject weapon in weapons)
        {
            Shot shot = weapon.GetComponent<Shot>();
            if (shot != null)
            {
                return shot;
            }
        }
        return null;
    }
}






