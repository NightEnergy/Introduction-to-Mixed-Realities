using UnityEngine;
using Vuforia;

public class TargetBattleManager : MonoBehaviour
{
    [Header("Vuforia Targets")]
    [SerializeField] private ObserverBehaviour targetCactus;
    [SerializeField] private ObserverBehaviour targetDragon;

    [Header("Character Models")]
    [SerializeField] private Transform cactusTransform;
    [SerializeField] private Transform dragonTransform;

    [Header("Animators")]
    [SerializeField] private Animator cactusAnimator;
    [SerializeField] private Animator dragonAnimator;

    [Header("Combat Settings")]
    [SerializeField] private float combatDistance = 0.5f;
    [SerializeField] private string attackBoolName = "IsAttacking";

    void Update()
    {
        if (targetCactus == null || targetDragon == null || cactusTransform == null || dragonTransform == null)
            return;

        bool cactusTracked = IsTracked(targetCactus);
        bool dragonTracked = IsTracked(targetDragon);

        if (!cactusTracked || !dragonTracked)
        {
            SetAttack(false);
            return;
        }

        cactusTransform.LookAt(dragonTransform.position, cactusTransform.up);
        dragonTransform.LookAt(cactusTransform.position, dragonTransform.up);

        // Distance & continuous attack
        float distance = Vector3.Distance(cactusTransform.position, dragonTransform.position);
        SetAttack(distance <= combatDistance);
    }

    private void SetAttack(bool active)
    {
        if (cactusAnimator != null && cactusAnimator.GetBool(attackBoolName) != active)
            cactusAnimator.SetBool(attackBoolName, active);

        if (dragonAnimator != null && dragonAnimator.GetBool(attackBoolName) != active)
            dragonAnimator.SetBool(attackBoolName, active);
    }

    private bool IsTracked(ObserverBehaviour target)
    {
        if (target == null) return false;
        var status = target.TargetStatus.Status;
        return status == Status.TRACKED || status == Status.EXTENDED_TRACKED;
    }
}
