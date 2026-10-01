using UnityEngine;
using UnityEngine.InputSystem;

public class CactusControllerScript : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            TriggerAttack();
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            TriggerAttack();
        }
    }

    public void TriggerAttack()
    {
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }
    }
}