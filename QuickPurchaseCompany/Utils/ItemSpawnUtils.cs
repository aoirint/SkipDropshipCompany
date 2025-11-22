using System.Linq;
using BepInEx.Logging;
using Unity.Netcode;
using UnityEngine;

namespace QuickPurchaseCompany.Utils;

internal static class ItemSpawnUtils
{
    internal static ManualLogSource Logger => QuickPurchaseCompany.Logger;

    private static GameObject cachedShipGameObject;

    public static GameObject GetShipGameObject()
    {
        if (cachedShipGameObject != null) {
            return cachedShipGameObject;
        }

        var shipGameObject = GameObject.Find("/Environment/HangarShip");
        if (shipGameObject == null) {
            // Invalid state
            Logger.LogError("Failed to find Ship game object.");
            return null;
        }

        cachedShipGameObject = shipGameObject;

        return shipGameObject;
    }

    public static Vector3? GetBaseSpawnPosition()
    {
        var startOfRound = StartOfRound.Instance;
        if (startOfRound == null) {
            // Invalid state
            Logger.LogError("StartOfRound.Instance is null.");
            return null;
        }

        var shipPositions = startOfRound.insideShipPositions;
        if (shipPositions == null) {
            // Invalid state
            Logger.LogError("StartOfRound.Instance.insideShipPositions is null.");
            return null;
        }

        const int shipPositionId = 10;
        var shipPositionCount = shipPositions.Count();
        if (shipPositionCount <= shipPositionId) {
            // Invalid state
            Logger.LogError($"Ship position ID is out of range. shipPositionId={shipPositionId} count={shipPositionCount}");
            return null;
        }

        var shipPosition = shipPositions[shipPositionId];
        if (shipPosition == null) {
            // Invalid state
            Logger.LogError($"Ship position is null for ID. shipPositionId={shipPositionId}");
            return null;
        }

        var spawnPosition = shipPosition.position + new Vector3(0f, 0f, 1.5f);

        return spawnPosition;
    }

    public static bool SpawnItemInShip(Item item)
    {
        var startOfRound = StartOfRound.Instance;
        if (startOfRound == null) {
            // Invalid state
            Logger.LogError("StartOfRound.Instance is null.");
            return false;
        }

        var spawnPrefab = item.spawnPrefab;
        if (spawnPrefab == null) {
            // Invalid state
            Logger.LogError("Item.spawnPrefab is null.");
            return false;
        }

        var shipGameObject = GetShipGameObject();
        if (shipGameObject == null) {
            // Invalid state
            Logger.LogError("Failed to get Ship game object.");
            return false;
        }

        var shipTransform = shipGameObject.transform;
        if (shipTransform == null) {
            // Invalid state
            Logger.LogError("Failed to get Ship transform.");
            return false;
        }

        var baseSpawnPositionNullable = GetBaseSpawnPosition();
        if (baseSpawnPositionNullable == null) {
            // Invalid state
            Logger.LogError("Failed to get spawn position.");
            return false;
        }
        var baseSpawnPosition = baseSpawnPositionNullable.Value;

        const float offset = 0.2f;
        var spawnPosition = baseSpawnPosition + new Vector3(
            Random.Range(-offset, offset),
            0f,
            Random.Range(-offset, offset)
        );

        var gameObject = Object.Instantiate(
            spawnPrefab,
            spawnPosition,
            Quaternion.identity,
            startOfRound.propsContainer
        );

        var grabbableObject = gameObject.GetComponent<GrabbableObject>();
        if (grabbableObject == null) {
            // Invalid state
            Logger.LogError("Failed to get GrabbableObject component from spawned item.");
            Object.Destroy(gameObject);
            return false;
        }

        grabbableObject.fallTime = 0f;
        grabbableObject.hasHitGround = true; // Disable drop sound effect
        grabbableObject.isInShipRoom = true;
        grabbableObject.transform.parent = shipTransform;

        var networkObject = gameObject.GetComponent<NetworkObject>();
        if (networkObject == null) {
            // Invalid state
            Logger.LogError("Failed to get NetworkObject component from spawned item.");
            Object.Destroy(gameObject);
            return false;
        }

        networkObject.Spawn(false);

        return true;
    }
}
