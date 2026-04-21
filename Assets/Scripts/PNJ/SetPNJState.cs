using UnityEngine;

public class SetPNJState : MonoBehaviour
{
    public int state = 0;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetInteger("State", state);
    }
}