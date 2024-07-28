using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalReposition : MonoBehaviour
{
    public float minDistanceFromPlayer = 25f;
    public float maxDistanceFromPlayer = 28f;

    private void OnTriggerExit(Collider other)
    {
        //콜라이더를 벗어난 오브젝트가 Animal 태그가 아니라면 리턴
        if(!other.CompareTag("Animal"))
        {
            return;
        }

        //플레이어의 위치
        Transform playerTransform = transform;
        SphereCollider playerCollider = GetComponent<SphereCollider>();
        Vector3 playerPos = playerTransform.position;
        

        //동물의 새로운 위치 설정
        Vector3 newAnimalPos = RepositioningAnimal(playerPos, minDistanceFromPlayer, maxDistanceFromPlayer);

        other.transform.position = newAnimalPos;
    }

    private Vector3 RepositioningAnimal(Vector3 playerPos, float minDistance, float maxDistance)
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        float randomDistance = Random.Range(minDistance, maxDistance);
        Vector3 randomPosition = playerPos + randomDirection * randomDistance;

        randomPosition.y = playerPos.y;

        return randomPosition;
    }
}
