using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalReposition : MonoBehaviour
{
    //public float minDistanceFromPlayer = 0f;
    public float maxDistanceFromPlayer = 0f;

    private void OnTriggerExit(Collider other)
    {
        //콜라이더를 벗어난 오브젝트가 Animal 태그가 아니라면 리턴
        if(!other.CompareTag("Animal"))
        {
            return;
        }

        //SphereCollider playerCollider = GetComponent<SphereCollider>();
        
        //플레이어의 위치
        Transform playerTransform = transform;
        Vector3 playerPos = playerTransform.position;
        

        //동물의 새로운 위치 설정
        Vector3 newAnimalPos = RepositioningAnimal(playerPos, maxDistanceFromPlayer);

        other.transform.position = newAnimalPos;
    }

    private Vector3 RepositioningAnimal(Vector3 playerPos, float distance)
    {
        //Vector3 randomDirection = Random.insideUnitSphere.normalized;
        Vector3 randomDirection = Random.onUnitSphere;
        //float randomDistance = Random.Range(minDistance, maxDistance);
        Vector3 randomPosition = playerPos + randomDirection * distance;

        randomPosition.y = playerPos.y;

        return randomPosition;
    }
}
