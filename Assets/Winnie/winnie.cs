using UnityEngine;
using System.Collections;

public class CharacterAutoWalk : MonoBehaviour
{
    public Animator animator;
    public Transform[] waypoints;         // 四個目標點
    public float[] speeds;                // 對應每段的速度（應該是 3 個）

    private int currentTargetIndex = 0;
    private bool walking = false;
    public float rotationSpeed = 5f;   // 轉向速度

    void Start()
    {
        if (waypoints.Length < 4 || speeds.Length < 3)
        {
            Debug.LogError("請設定四個目標點與三個對應速度");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (walking && currentTargetIndex < waypoints.Length)
        {
            Transform target = waypoints[currentTargetIndex];
            Vector3 direction = (target.position - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, target.position);

            float currentSpeed = GetSpeedForSegment(currentTargetIndex);
            animator.SetFloat("Speed", currentSpeed);

            if (distance > 0.1f)
            {
                transform.position += direction * currentSpeed * Time.deltaTime;
            }
            else
            {
                // 到達一個目標點
                animator.SetFloat("Speed", 0);
                walking = false;

                // 如果是第一個點，停留 2 秒後再繼續
                if (currentTargetIndex == 0)
                {
                    StartCoroutine(WaitAndContinue(2f)); // 等 2 秒
                }
                else
                {
                    currentTargetIndex++;
                    walking = true;
                }

                // 到達最後一點
                if (currentTargetIndex >= waypoints.Length)
                {
                    walking = false;
                    animator.SetTrigger("FinalPose");
                    StartCoroutine(RotateRight60());
                }
            }
        }
    }

    public void StartWalking()
    {
        walking = true;
        currentTargetIndex = 0;
    }

    private float GetSpeedForSegment(int index)
    {
        if (index >= speeds.Length) return speeds[speeds.Length - 1];
        return speeds[index];
    }

    private IEnumerator WaitAndContinue(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentTargetIndex++;
        walking = true;
    }

    private IEnumerator RotateRight60()
    {
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = startRot * Quaternion.Euler(0, 60, 0);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.rotation = targetRot;
    }
}
