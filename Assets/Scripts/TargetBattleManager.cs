using UnityEngine;
using Vuforia;

public class TargetBattleManager : MonoBehaviour
{
    [SerializeField] private ObserverBehaviour targetCactus;
    [SerializeField] private ObserverBehaviour targetDragon;

    [SerializeField] private Transform cactusPivot;
    [SerializeField] private Transform dragonPivot;

    [SerializeField] private Animator cactusAnimator;
    [SerializeField] private Animator dragonAnimator;

    [SerializeField] private float combatDistance = 0.3f;
    [SerializeField] private float attackCooldown = 1.5f;

    private float lastAttackTime;

    void Update()
    {
        bool cactusTracked = IsTracked(targetCactus);
        bool dragonTracked = IsTracked(targetDragon);

        if (!cactusTracked || !dragonTracked)
        {
            return;
        }

        float distance = Vector3.Distance(targetCactus.transform.position, targetDragon.transform.position);

        RotateOnCardPlane(cactusPivot, targetCactus.transform, targetDragon.transform.position);
        RotateOnCardPlane(dragonPivot, targetDragon.transform, targetCactus.transform.position);

        if (distance <= combatDistance)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                TriggerAttack();
                lastAttackTime = Time.time;
            }
        }
    }

    private bool IsTracked(ObserverBehaviour target)
    {
        if (target == null) return false;
        var status = target.TargetStatus.Status;
        return status == Status.TRACKED || status == Status.EXTENDED_TRACKED;
    }

    private void RotateOnCardPlane(Transform pivot, Transform targetCard, Vector3 opponentWorldPos)
    {
        if (pivot == null || targetCard == null) return;

        Vector3 localOpponent = targetCard.InverseTransformPoint(opponentWorldPos);
        float angle = Mathf.Atan2(localOpponent.x, localOpponent.z) * Mathf.Rad2Deg;

        Quaternion targetRot = Quaternion.Euler(0f, angle, 0f);
        pivot.localRotation = Quaternion.Slerp(pivot.localRotation, targetRot, Time.deltaTime * 6f);
    }

    private void TriggerAttack()
    {
        if (cactusAnimator != null)
        {
            cactusAnimator.SetTrigger("Attack");
        }

        if (dragonAnimator != null)
        {
            dragonAnimator.SetTrigger("Attack");
        }
    }

    private void OnDrawGizmos()
    {
        if (targetCactus != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(targetCactus.transform.position, combatDistance);
        }
    }
}