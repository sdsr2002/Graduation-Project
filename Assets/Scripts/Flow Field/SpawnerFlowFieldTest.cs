using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FlowFields;

public class SpawnerFlowFieldTest : MonoBehaviour
{
    public static SpawnerFlowFieldTest Instance;

    [SerializeField]
    private bool _spawnerOn;
    [SerializeField]
    private GameObject _unitToSpawn;
    [SerializeField]
    private GameObject[] _spawnPoints;

    [SerializeField]
    private int _spawnedUnits;

    [SerializeField, Range(0.0001f,10f)]
    private float _spawnTimer = 0.1f;

    private float _timer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (_spawnerOn && _unitToSpawn != null)
            Tick();
    }

    private void Tick()
    {
        if (_timer <= 0)
        {
            SpawnUnit();
            _timer = _spawnTimer;
            return;
        }

        _timer -= Time.deltaTime;
        return;
    } 

    public void DespawnedUnit()
    {
        _spawnedUnits--;
    }

    private void SpawnUnit()
    {
        if (_spawnPoints != null && _spawnPoints.Length != 0)
        {
            Instantiate(_unitToSpawn, _spawnPoints[Random.Range(0,_spawnPoints.Length)].transform.position,Quaternion.identity);
            _spawnedUnits++;
        }
    }
}

