using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    public GameObject[] animalPrefabs; //���� ������
    public Transform[] spawnPoints; //���� ���� ����
    public int maxAnimalsPertype = 3; //������ �ִ� ������

    private List<GameObject> animalPool = new List<GameObject>();

    private void Awake()
    {        
        //spawnPoints = GetComponentsInChildren<Transform>();
        spawnPoints = gameObject.transform.GetChild(0).GetComponentsInChildren<Transform>();
        
        //�ʱ� ���� ������Ʈ Ǯ ����
        for (int i = 0; i < animalPrefabs.Length; i++)
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

        //Transform[] allTransforms = GetComponentsInChildren<Transform>();
        //spawnPoints = new Transform[allTransforms.Length - 1];
        //for (int i = 1; i < allTransforms.Length; i++)
        //{
        //    spawnPoints[i - 1] = allTransforms[i];
        //}
        //
        //// 초기 동물 오브젝트 풀 생성
        //for (int i = 0; i < animalPrefabs.Length; i++)
        //{
        //    for (int j = 0; j < maxAnimalsPertype; j++)
        //    {
        //        GameObject animal = Instantiate(animalPrefabs[i]);
        //        Transform spawnPoint = spawnPoints[Random.Range(2, spawnPoints.Length)];
        //        animal.transform.position = spawnPoint.position;
        //        animal.SetActive(false);
        //        animalPool.Add(animal);
        //    }
        //}

    }

    private void Start()
    {
        StartCoroutine(Check_AnimalSpawning());
    }

    private IEnumerator Check_AnimalSpawning() //동물의 상태를 확인하고 3마리를 유지하는 코루틴
    {
        
        while (true)
        {
            int[] animalCounts = new int[animalPrefabs.Length];

            //활성화된 동물 마리수 확인
            foreach (GameObject animal in animalPool)
            {
                if (animal.activeSelf)
                {
                    if (animal.name.Contains("Rabbit"))
                    {
                        animalCounts[0]++;
                    }
                    else if (animal.name.Contains("Deer"))
                    {
                        animalCounts[1]++;
                    }
                }
            }

            //부족한 동물 스폰
            for (int i = 0; i < animalCounts.Length; i++)
            {
                if (animalCounts[i] < maxAnimalsPertype)
                {
                    SpawnAnimals(i);
                }
            }
            yield return new WaitForSeconds(1f); //1초마다 체크
        }
    }

    private void SpawnAnimals(int animalIndex) //비활성화된 동물 중 인덱스에 맞는 동물을 활성화하고 스폰 포인트에 배치
    {
        foreach (GameObject animal in animalPool)
        {
            if (!animal.activeSelf && animal.name.Contains(animalPrefabs[animalIndex].name))
            {
                Transform spawnPoint = spawnPoints[Random.Range(2, spawnPoints.Length)];
                animal.transform.position = spawnPoint.position;
                animal.SetActive(true);
                break;
            }
        }
    }
    
}
