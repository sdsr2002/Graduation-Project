using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlowFields;

[RequireComponent(typeof(Rigidbody))]
public class FlowFieldFollow : MonoBehaviour
{
    [SerializeField]
    private FlowFieldGridData grid;
    [SerializeField]
    private Rigidbody body;
    private Vector3 direction => grid.GetCellDirectionFromWorldPosition(transform.position, target.position);

    [SerializeField]
    private float _speed = 5f;

    [SerializeField]
    private Transform target;

    private Vector3 moveDirection = Vector3.zero;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();

        target = GameObject.Find("Target").transform;
    }

    private void Update()
    {
        if (target != null && grid != null)
        {
            //Debug.Log(grid.Grid[0].name);
            moveDirection = direction.normalized;
            body.velocity = moveDirection * _speed + Physics.gravity * 0.4f;
        }
    }

    private void FixedUpdate()
    {
        //transform.position += moveDirection * Time.fixedDeltaTime * 5f;
        if (Vector3.Distance(target.position, transform.position) < 3f)
        {
            SpawnerFlowFieldTest.Instance.DespawnedUnit();
            Destroy(gameObject);
        }
    }

}
