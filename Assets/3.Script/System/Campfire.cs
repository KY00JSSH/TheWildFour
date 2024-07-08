using System.Collections;
using UnityEngine;


public class Campfire : MonoBehaviour {
    private float HeatRange = 5f;

    private void Update() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, HeatRange);

        foreach (Collider collider in colliders) {
            if (collider.CompareTag("Player")) {
                if (collider.transform.parent.
                    TryGetComponent(out PlayerStatus playerStatus)) {
                    StatusControl.Instance.GiveStatus(Status.Heat, playerStatus);
                    break;
                }
            }
        }
    }
}
