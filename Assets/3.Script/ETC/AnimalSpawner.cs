using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] animalPrefabs; //동물 프리팹
    public Transform[] spawnPoints; //동물 스폰 지점
    public int maxAnimalsPertype = 3; //동물별 최대 마릿수

    private List<GameObject> animalPool = new List<GameObject>();

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();

        //초기 동물 오브젝트 풀 생성
        for(int i = 0; i < animalPrefabs.Length; i++)
        {
            for(int j = 0; j < maxAnimalsPertype; j++)
            {
                GameObject animal = Instantiate(animalPrefabs[i]);
                Transform spawnPoint = spawnPoints[Random.Range(1, spawnPoints.Length)];
                animal.transform.position = spawnPoint.position;
                animal.SetActive(false);
                animalPool.Add(animal);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(Check_AnimalSpawning());
    }
    
    private IEnumerator Check_AnimalSpawning() //부족한 개체수를 추가해 3마리 유지
    {
        while(true)
        {
            int[] animalCounts = new int[animalPrefabs.Length];
            
            //현재 활성화된 동물의 마릿수 확인
            foreach(GameObject animal in animalPool)
            {
                if(animal.activeSelf)
                {
                    if(animal.name.Contains("Rabbit"))
                    {
                        animalCounts[0]++;
                    }
                    else if(animal.name.Contains("Deer"))
                    {
                        animalCounts[1]++;
                    }
                }
            }

            //부족한 동물 스폰
            for(int i =0; i < animalCounts.Length; i++)
            {
                if(animalCounts[i] < maxAnimalsPertype)
                {
                    SpawnAnimals(i);
                }
            }
            yield return new WaitForSeconds(1f) ; //1초마다 활성화된 동물 개체수 확인
        }
    }

    private void SpawnAnimals(int animalIndex) //비활성화된 동물 오브젝트를 활성화, 스폰 지점에서 스폰
    {
        foreach (GameObject animal in animalPool)
        {
            if(!animal.activeSelf && animal.name.Contains(animalPrefabs[animalIndex].name))
            {
                Transform spawnPoint = spawnPoints[Random.Range(1, spawnPoints.Length)];
                animal.transform.position = spawnPoint.position;
                animal.SetActive(true);
                break;
            }
        }
    }
}
