using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private ObstacleBase[] _obstaclePrefabs;

    [Header("Spawning")]
    [SerializeField] private Mouse _mouse;
    [SerializeField] private float _minXIndent = 1f;
    [SerializeField] private float _distanceBetweenObstacles = 3f;
    [SerializeField] private int _maxObstaclesOnScreen = 5;
    [SerializeField] private int _maxRepeatingObstacles = 2;
    [SerializeField] private float _absMaxPositionX = 1f;
    [SerializeField] private float _absMaxRotationY = 15f;

    [Header("Pool")]
    [SerializeField] private bool _collectionChecks;
    [SerializeField] private int _defaultCapacity = 5;
    [SerializeField] private int _maxSize = 20;

    private Dictionary<ObstacleBase, ObjectPool<ObstacleBase>> _pools;
    private List<ObstacleBase> _obstaclesOnScene;

    private void Awake()
    {
        _obstaclesOnScene = new List<ObstacleBase>();

        InitPools();
        SpawnStartObstacles();
    }

    private void Update()
    {
        if (Context.Instance.IsPlaying)
            MoveObstacles();
    }

    private void OnEnable()
    {
        Context.Instance.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        Context.Instance.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        if(state == GameState.Playing)
            StartCoroutine(SpawnObstaclesCoroutine());

        if(state == GameState.GameOver)
            StopAllCoroutines();

        if(state == GameState.Title)
        {
            foreach(var obstacle in _obstaclesOnScene.ToArray())
                obstacle.Release();

            SpawnStartObstacles();
        }
    }

    private void InitPools()
    {
        _pools = new Dictionary<ObstacleBase, ObjectPool<ObstacleBase>>();

        foreach(ObstacleBase obstacle in _obstaclePrefabs)
        {
            _pools.Add(obstacle, new ObjectPool<ObstacleBase>(() => CreateObstacleInstance(obstacle),
                OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, _collectionChecks, _defaultCapacity, _maxSize));
        }
    }

    private void MoveObstacles()
    {
        foreach (ObstacleBase obstacle in _obstaclesOnScene.ToArray())
        {
            obstacle.transform.Translate(Vector3.back * GetSpeed() * Time.deltaTime, Space.World);

            if (obstacle.transform.position.z < 0f)
                obstacle.Release();
        }
    }

    private void SpawnStartObstacles()
    {
        for(int i = 0; i < _maxObstaclesOnScreen; i++)
            SpawnRandomObstacle((_distanceBetweenObstacles * i) + GetStartPositionZ());
    }

    private ObstacleBase GetRandomObstacle()
    {
        int randomObstacle = Random.Range(0, _obstaclePrefabs.Length);
        ObstacleBase obstaclePrefab = _obstaclePrefabs[randomObstacle];

        if (CheckLastRepeatingObstacles(obstaclePrefab))
            obstaclePrefab = _obstaclePrefabs.Where(prefab => prefab.GetType() != obstaclePrefab.GetType()).FirstOrDefault();

        ObstacleBase obstacle = _pools[obstaclePrefab].Get();
        obstacle.SetPool(_pools[obstaclePrefab]);

        return obstacle;
    }

    private IEnumerator SpawnObstaclesCoroutine()
    {
        while(Context.Instance.IsPlaying)
        {
            SpawnRandomObstacle((_distanceBetweenObstacles * _maxObstaclesOnScreen) + GetStartPositionZ());
            yield return new WaitForSeconds(_distanceBetweenObstacles / GetSpeed());
        }
    }

    private void SpawnRandomObstacle(float z)
    {
        float x = Random.Range(-_absMaxPositionX, _absMaxPositionX);

        if(_obstaclesOnScene.Count > 0)
        {
            float previousX = _obstaclesOnScene.Last().transform.position.x;

            do
            {
                x = Random.Range(-_absMaxPositionX, _absMaxPositionX);
            }
            while (Mathf.Abs(x - previousX) < _minXIndent);
        }

        Vector3 position = new (x, 0, z);
        Quaternion rotation = Quaternion.Euler(0, Random.Range(-_absMaxRotationY, _absMaxRotationY), 0);

        ObstacleBase instance = GetRandomObstacle();
        instance.transform.SetPositionAndRotation(position, rotation);
    }

    private bool CheckLastRepeatingObstacles(ObstacleBase obstacle)
    {
        if (_obstaclesOnScene.Count < _maxRepeatingObstacles) return false;

        ObstacleBase[] lastObstacles = _obstaclesOnScene.Reverse<ObstacleBase>().Take(_maxRepeatingObstacles).ToArray();
        return System.Array.TrueForAll(lastObstacles, instance => instance.GetType() == obstacle.GetType());
    }

    private float GetStartPositionZ()
    {
        return _mouse.transform.position.z + _distanceBetweenObstacles;
    }

    private float GetSpeed()
    {
        return _mouse.Speed * Mathf.Sin(_mouse.Angle);
    }

    private ObstacleBase CreateObstacleInstance(ObstacleBase prefab)
    {
        return Instantiate(prefab, Vector3.zero, Quaternion.identity);
    }

    private void OnTakeFromPool(ObstacleBase obstacle)
    {
        obstacle.gameObject.SetActive(true);

        _obstaclesOnScene.Add(obstacle);
    }

    private void OnReturnedToPool(ObstacleBase obstacle)
    {
        obstacle.gameObject.SetActive(false);
        obstacle.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        _obstaclesOnScene.Remove(obstacle);
    }

    private void OnDestroyPoolObject(ObstacleBase obstacle)
    {
        Destroy(obstacle.gameObject);
    }
}
