// SPDX-License-Identifier: MIT
#nullable enable

using System.Collections.Generic;
using System.Linq;
using SkipDropshipCompany.Core.Ports;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

namespace SkipDropshipCompany.Interop.Game.Adapters;

// ItemSpawnAdapter owns the Unity-side recreation of dropship delivery items.
// Core chooses item indexes; this adapter resolves prefabs, placement, and
// NetworkObject spawning inside the ship.
internal sealed class ItemSpawnAdapter
{
    private readonly IPluginLogger logger;
    private readonly Dictionary<int, float> cachedSpawnOffsetXByItemId = [];

    public ItemSpawnAdapter(IPluginLogger logger)
    {
        this.logger = logger;
    }

    public bool SpawnItemInShip(Item item)
    {
        var startOfRound = StartOfRound.Instance;
        if (startOfRound == null)
        {
            logger.LogError("StartOfRound.Instance is null.");
            return false;
        }

        var spawnPrefab = item.spawnPrefab;
        if (spawnPrefab == null)
        {
            logger.LogError("Item.spawnPrefab is null.");
            return false;
        }

        var elevatorTransform = startOfRound.elevatorTransform;
        if (elevatorTransform == null)
        {
            logger.LogError("StartOfRound.Instance.elevatorTransform is null.");
            return false;
        }

        var baseSpawnPositionNullable = GetBaseSpawnPosition(startOfRound);
        if (baseSpawnPositionNullable == null)
        {
            logger.LogError("Failed to get spawn position.");
            return false;
        }
        var baseSpawnPosition = baseSpawnPositionNullable.Value;

        var offsetX = GetSpawnOffsetXByItemId(item.itemId);

        // Base-game out-of-bounds delivery uses a small random X spread near
        // the ship spawn point. Keep deterministic per-item spread so repeated
        // direct deliveries do not overlap as badly.
        const float offsetZ = 1.0f;
        const float offsetY = 0.5f;

        var spawnPosition = baseSpawnPosition + new Vector3(offsetX, offsetY, offsetZ);

        var gameObject = Object.Instantiate(
            spawnPrefab,
            spawnPosition,
            Quaternion.identity,
            elevatorTransform
        );

        var grabbableObject = gameObject.GetComponent<GrabbableObject>();
        if (grabbableObject == null)
        {
            logger.LogError("Failed to get GrabbableObject component from spawned item.");
            Object.Destroy(gameObject);
            return false;
        }

        grabbableObject.fallTime = 0f;
        grabbableObject.isInElevator = true;
        grabbableObject.isInShipRoom = true;
        grabbableObject.hasHitGround = true;

        // The object must be spawned after its ship/elevator flags are set so
        // clients receive the direct-delivery state instead of a falling item.
        var networkObject = gameObject.GetComponent<NetworkObject>();
        if (networkObject == null)
        {
            logger.LogError("Failed to get NetworkObject component from spawned item.");
            Object.Destroy(gameObject);
            return false;
        }

        networkObject.Spawn(false);

        return true;
    }

    private Vector3? GetBaseSpawnPosition(StartOfRound startOfRound)
    {
        var playerSpawnPositions = startOfRound.playerSpawnPositions;
        if (playerSpawnPositions == null)
        {
            logger.LogError("StartOfRound.Instance.playerSpawnPositions is null.");
            return null;
        }

        var playerSpawnPositionTransform = playerSpawnPositions.ElementAtOrDefault(1);
        if (playerSpawnPositionTransform == null)
        {
            logger.LogError("Player spawn position is null for ID 1.");
            return null;
        }

        return playerSpawnPositionTransform.position;
    }

    private float GetSpawnOffsetXByItemId(int itemId)
    {
        if (cachedSpawnOffsetXByItemId.TryGetValue(itemId, out var cachedOffsetX))
        {
            return cachedOffsetX;
        }

        // Deterministic per-item spread makes validation and repeated purchases
        // easier to reason about than Unity random state.
        // Range for out-of-bounds items in the base game.
        const float offsetXRange = 0.7f;

        var itemIdUint = (uint)itemId;
        var randomSeed = math.hash(new uint4(itemIdUint, 0xDEADBEEFu, 0x12345678u, 0x87654321u));
        if (randomSeed == 0u)
        {
            randomSeed = 1u;
        }

        var random = new Unity.Mathematics.Random(randomSeed);
        var offsetX = random.NextFloat(-offsetXRange, offsetXRange);

        cachedSpawnOffsetXByItemId[itemId] = offsetX;

        logger.LogDebug($"Generated offset X for an item ID. itemId={itemId}, offsetX={offsetX}");

        return offsetX;
    }
}
