using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        anim.SetTrigger("Open");
    }
}
