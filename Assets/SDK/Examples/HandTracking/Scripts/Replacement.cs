using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replacement : MonoBehaviour
{
    [Tooltip("The distance from the GameObject's spawn position at which will trigger a respawn ")]
    public float DistanceThreshold = 20.0f;

    private Vector3 RespawnPoint;
    private Quaternion RespawnOrientation;

    private Rigidbody rigidBody;

    private void Start() {
        rigidBody = GetComponent<Rigidbody>();

        LockSpawnPoint();
    }

    private void LateUpdate() {
        float distanceSqr = (RespawnPoint - this.transform.position).sqrMagnitude;

        if(distanceSqr > DistanceThreshold * DistanceThreshold) {
            // Reset any velocity from falling or moving when respawning to original location
            if(rigidBody != null) {
                rigidBody.velocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;
            }

            this.transform.SetPositionAndRotation(RespawnPoint, RespawnOrientation);
        }
    }

    public void LockSpawnPoint() {
        RespawnPoint = this.transform.position;
        RespawnOrientation = this.transform.rotation;
    }
}
