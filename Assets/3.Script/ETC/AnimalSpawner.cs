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
        spawnPoints = GetComponentsInChildren<Transform>();

        //�ʱ� ���� ������Ʈ Ǯ ����
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
    
    private IEnumerator Check_AnimalSpawning() //������ ��ü���� �߰��� 3���� ����
    {
        while(true)
        {
            int[] animalCounts = new int[animalPrefabs.Length];
            
            //���� Ȱ��ȭ�� ������ ������ Ȯ��
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

            //������ ���� ����
            for(int i =0; i < animalCounts.Length; i++)
            {
                if(animalCounts[i] < maxAnimalsPertype)
                {
                    SpawnAnimals(i);
                }
            }
            yield return new WaitForSeconds(1f) ; //1�ʸ��� Ȱ��ȭ�� ���� ��ü�� Ȯ��
        }
    }

    private void SpawnAnimals(int animalIndex) //��Ȱ��ȭ�� ���� ������Ʈ�� Ȱ��ȭ, ���� �������� ����
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
