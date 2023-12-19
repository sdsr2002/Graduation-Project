using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlowFields
{
    public class SpawnerFlowFieldTest : MonoBehaviour
    {
        [SerializeField]
        private bool _spawnerOn;
        [SerializeField]
        private GameObject _unitToSpawn;
        [SerializeField]
        private GameObject[] _spawnPoints;

        [SerializeField, Range(0.0001f,10f)]
        private float _spawnTimer = 0.1f;

        private float _timer;


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

        private void SpawnUnit()
        {
            if (_spawnPoints != null && _spawnPoints.Length != 0)
                Instantiate(_unitToSpawn, _spawnPoints[Random.Range(0,_spawnPoints.Length)].transform.position,Quaternion.identity);
        }
    }
}

