using UnityEngine;

public class PlayerMoveForDEBUGGING : MonoBehaviour {
    PlayerMove move;

    public Quaternion playerRigidRotation;
    public Quaternion playerSpineRotation;
    public Quaternion playerDirectionRotation;
    public Quaternion cameraRotation;
    public Quaternion targetRotation;
    public Vector3 inputXZ;
    private void Awake() {
        move = GetComponent<PlayerMove>();
    }

    private void Update() {
        playerRigidRotation = move.GetRigid();
        playerSpineRotation = move.GetSpine();
        playerDirectionRotation = move.GetDirection();
        cameraRotation = move.GetCameraRotation();
        targetRotation = move.GetTargetRotation();
        inputXZ = move.GetInputXZ();
    }

}