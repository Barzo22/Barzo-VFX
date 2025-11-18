using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator anim;

    public void OpenDoor()
    {
        anim.SetTrigger("Open");
    }
}
