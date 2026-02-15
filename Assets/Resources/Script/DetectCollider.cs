using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollider : MonoBehaviour
{
    [SerializeField] bool isDetected = false;
    [SerializeField] Transform meleePos;
    [SerializeField] Vector2 boxSize;
    [SerializeField] Collider2D currentTarget;
    [SerializeField] LayerMask layerMask;

    public Object DetectedEnemy { get; private set; }
    public IAttack AttackTarget { get; private set; }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(meleePos.position, boxSize);
    }

    private void Update()
    {
        SearchCollider();
    }

    private void SearchCollider()
    {
        if (isDetected == true) return;

        // OverlapBox 메서드를 이용해 boxSize 범위의 있는 콜라이더를 지속적으로 탐지함
        currentTarget = Physics2D.OverlapBox(meleePos.position, boxSize, 0, layerMask);

        if (currentTarget != null)
        {
            isDetected = true;
            DetectedEnemy = currentTarget.GetComponent<Object>();
            AttackTarget = currentTarget.GetComponent<IAttack>();
        }
    }

    public void EmptyDetectCollider2D()
    {
        currentTarget = null;
        DetectedEnemy = null;
        isDetected = false;
    }

    public bool IsDetectEnemy(string enemyTag)
    {
        return DetectedEnemy != null && DetectedEnemy.CompareTag(enemyTag);
    }
}
