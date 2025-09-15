using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;   // NavMeshSurface / NavMeshLink

public class LevelGenerator : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject playerPrefab;
    Transform ObjectsParent2D;

    [Header("Layout del nivel")]
    [SerializeField] private int platformCount = 14;
    [SerializeField] private Vector3 areaSize = new Vector3(40, 10, 40);
    [SerializeField] private Vector2 yRange = new Vector2(0f, 6f);
    [SerializeField] private float minPlatformGap = 4f;      // distancia mínima entre centros de plataformas
    [SerializeField] private float maxLinkDistance = 7f;     // máximo entre plataformas para crear un link
    [SerializeField] private float linkWidth = 2f;         // ancho del NavMeshLink

    [Header("Enemigos")]
    [SerializeField] private int enemiesToSpawn = 8;
    [Range(0, 1)] [SerializeField] private float enemyOnPlatformChance = 0.6f;
    [SerializeField] private float playerSpawnOffsetY = 1.2f;
    [SerializeField] private float enemySpawnOffsetY = 1.1f;
    [SerializeField] private float enemyPlatformMinDistanceToPlayer = 3.5f;

    [Header("Cambio de niveles")]
    [SerializeField] private float levelRestartDelay = 3f;   // pausa antes de regenerar

    // Inventario
    private readonly List<Transform> platforms = new();
    private readonly List<Transform> enemies = new();
    private int aliveEnemies;

    // Player "falso"
    private GameObject playerInstance;
    private NavMeshAgent playerAgent;

    // Variete
    private bool regenerating = false;

    void Start()
    {
        GenerateFreshLevelAndMaybePlayer();
    }

    // -------------------- API principal --------------------
    private void GenerateFreshLevelAndMaybePlayer()
    {
    
        ClearLevel();

        // Plataformas
        platforms.Clear();
        enemies.Clear();
        GeneratePlatforms();

        // Bake del NavMesh
        RequireSurfaceOrError();
        surface.BuildNavMesh();

        // Puentes virtuales entre plataformas
        CreateLinksBetweenPlatforms(maxLinkDistance);

        // Player 
        if (playerInstance == null)
            SpawnPlayer();
        else
            MovePlayerToNewStart();

        //Enemigos
        SpawnEnemies();

        // muertes, vifda, etc
        aliveEnemies = 0;
        foreach (var t in enemies)
        {
            if (t != null && t.TryGetComponent<Health>(out var hp))
            {
                hp.OnDeath += OnEnemyDeath;
                aliveEnemies++;
            }
        }
    }

    void GeneratePlatforms()
    {
        int tries = 0;
        while (platforms.Count < platformCount && tries < platformCount * 40)
        {
            tries++;
            Vector3 pos = RandomPointInBox(areaSize);
            pos.y = Random.Range(yRange.x, yRange.y);

            if (!IsFarEnough(pos)) continue;

            var plat = Instantiate(platformPrefab, pos, Quaternion.identity,  surface.transform);
            plat.name = $"Platform_{platforms.Count}";
            platforms.Add(plat.transform);
            if (plat.TryGetComponent<Entity2D>(out var e2D))
            {
                Generate2DCopy(e2D.transform);
            }
        }

        if (platforms.Count == 0)
            Debug.LogError("no se generaron plataformas.");
    }


    void SpawnPlayer()
    {
        Transform startPlat = platforms[Random.Range(0, platforms.Count)];
        Vector3 playerPos = startPlat.position + Vector3.up * playerSpawnOffsetY;
        playerInstance = Instantiate(playerPrefab, playerPos, Quaternion.identity);
        playerAgent = playerInstance.GetComponent<NavMeshAgent>();
        if (playerInstance.TryGetComponent<Entity2D>(out var e2D))
        {
            Generate2DCopy(e2D.transform);
        }
    }

    void MovePlayerToNewStart()
    {
        //TODO: Hacer smooth la transición
        if (playerInstance == null) { SpawnPlayer(); return; }

        // Deshabilitar el agente para evitar “Failed to create agent…” 
        if (playerAgent == null) playerAgent = playerInstance.GetComponent<NavMeshAgent>();
        if (playerAgent) playerAgent.enabled = false;

        //Posiciono en una plataforma random
        Transform startPlat = platforms[Random.Range(0, platforms.Count)];
        Vector3 playerPos = startPlat.position + Vector3.up * playerSpawnOffsetY;

        // Busco un punto válido del navMesh
        if (NavMesh.SamplePosition(playerPos, out var hit, 1.0f, NavMesh.AllAreas))
            playerPos = hit.position + Vector3.up * 0.02f;

        // Rehabilitar y Warp 
        //TODO: Smoooooooooth!!!
        if (playerAgent)
        {
            playerAgent.enabled = true;
            playerAgent.Warp(playerPos);
            playerAgent.ResetPath();
        }
        else
        {
            playerInstance.transform.position = playerPos;
        }
    }

    void SpawnEnemies()
    {
        int placed = 0;
        enemies.Clear();

        // Intentar distribuir
        Vector3 playerPos = playerInstance != null ? playerInstance.transform.position : Vector3.zero;

        foreach (var p in platforms)
        {
            if (placed >= enemiesToSpawn) break;
            if (Random.value <= enemyOnPlatformChance && Vector3.SqrMagnitude(p.position - playerPos) > enemyPlatformMinDistanceToPlayer * enemyPlatformMinDistanceToPlayer)
            {
                var e = Instantiate(enemyPrefab, p.position + Vector3.up * enemySpawnOffsetY, Quaternion.identity, transform);
                enemies.Add(e.transform);
                if (e.TryGetComponent<Entity2D>(out var e2D))
                {
                    Generate2DCopy(e2D.transform);
                }
                placed++;
            }
        }
        // Completar si faltan
        while (placed < enemiesToSpawn)
        {
            var p = platforms[Random.Range(0, platforms.Count)];
            if (Vector3.SqrMagnitude(p.position - playerPos) < enemyPlatformMinDistanceToPlayer * enemyPlatformMinDistanceToPlayer) continue;
            var e = Instantiate(enemyPrefab, p.position + Vector3.up * enemySpawnOffsetY, Quaternion.identity, transform);
            enemies.Add(e.transform);
            if (e.TryGetComponent<Entity2D>(out var e2D))
            {
                Generate2DCopy(e2D.transform);
            }
            placed++;
        }
    }


    void CreateLinksBetweenPlatforms(float maxDist)
    {
        //Chequeo distancia entre plataformas. Si la distancia es corta, creo un link
        float sqrMaxDist = maxDist * maxDist;
        for (int i = 0; i < platforms.Count; i++)
        {
            for (int j = i + 1; j < platforms.Count; j++)
            {
                var a = platforms[i];
                var b = platforms[j];
                float d = Vector3.SqrMagnitude(a.position - b.position);
                if (d <= sqrMaxDist)
                {
                    CreateLink(a.position, b.position);
                }
            }
        }
    }

    void CreateLink(Vector3 from, Vector3 to)
    {
        //UN PARTOOOO. Genera "puentes" entre plataformas para que salte el playerDetected falso
        //TODO: Usar pooling
        var holder = new GameObject("NavMeshLink").transform;
        holder.parent = transform;
        holder.position = (from + to) * 0.5f;

        // Empujar extremos hacia afuera + leve altura
        Vector3 dir = (to - from).normalized;
        Vector3 worldA = from + dir * 2.2f + Vector3.up * 0.10f;
        Vector3 worldB = to - dir * 2.2f + Vector3.up * 0.10f;

        // Snap al NavMesh
        if (NavMesh.SamplePosition(worldA, out var hitA, 1.0f, NavMesh.AllAreas))
            worldA = hitA.position + Vector3.up * 0.02f;
        if (NavMesh.SamplePosition(worldB, out var hitB, 1.0f, NavMesh.AllAreas))
            worldB = hitB.position + Vector3.up * 0.02f;

        // Local respecto al holder
        Vector3 localA = holder.InverseTransformPoint(worldA);
        Vector3 localB = holder.InverseTransformPoint(worldB);



        var link = holder.gameObject.AddComponent<NavMeshLink>();
        link.bidirectional = true;
        link.width = linkWidth;     // configurable
        link.costModifier = -1;
        link.area = 0;             // caminable
        link.startPoint = localA;
        link.endPoint = localB;
        link.UpdateLink();
    }

    void OnEnemyDeath(Health dead)
    {
        aliveEnemies--;
        if (aliveEnemies <= 0 && !regenerating)
        {
            Debug.Log("<color=lime>NIVEL TERMINADO!</color>");
            StartCoroutine(RegenerateAfterDelay());
        }
    }

    IEnumerator RegenerateAfterDelay()
    {
        //TODO: Hacer mas copado el reposicionamiento. Secuenciar, animar, etc
        regenerating = true;
        yield return new WaitForSeconds(levelRestartDelay);

        // Desactivo el agent del playerDetected durante el cambio de nivel
        if (playerAgent == null && playerInstance != null)
            playerAgent = playerInstance.GetComponent<NavMeshAgent>();

        if (playerAgent) playerAgent.enabled = false;

        GenerateFreshLevelAndMaybePlayer();

        regenerating = false;
    }

    void RequireSurfaceOrError()
    {
        if (surface == null)
            Debug.LogError("LevelGenerator: asigna la referencia a 'surface' (NavMeshSurface).");
    }

    Vector3 RandomPointInBox(Vector3 size)
    {
        return transform.position + new Vector3(Random.Range(-size.x * 0.5f, size.x * 0.5f), 0f, Random.Range(-size.z * 0.5f, size.z * 0.5f));
    }

    bool IsFarEnough(Vector3 pos)
    {
        //Agregado para chequear distancia mínima entre plataformas
        //TODO: optimizar considerando la vista en 3D del arcade
        foreach (var p in platforms)
            if (Vector3.SqrMagnitude(p.position - pos) < minPlatformGap * minPlatformGap)
                return false;
        return true;
    }

    void ClearLevel()
    {
        // Borra plataformas, links, enemigos
        //TODO: optimizar con pooling
        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);
        foreach (var t in platforms)
            if (t != null)
                DestroyImmediate(t.gameObject);

    }

    void Generate2DCopy(Transform assigned)
    {
        if (!ObjectsParent2D)
            ObjectsParent2D = new GameObject("ObjectParent2D").transform;
        GameObject go = Instantiate(assigned.GetComponent<Entity2D>().entitySprite,ObjectsParent2D);
        assigned.GetComponent<Entity2D>().InitiateMe(go.transform);
    }



    // Agrego para ver el area de juego y luego posicionar cámara de acuerdo a esta cosa
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(
            transform.position + new Vector3(0, (yRange.x + yRange.y) * 0.5f, 0),
            new Vector3(areaSize.x, yRange.y - yRange.x + 0.5f, areaSize.z)
        );
    }
}
