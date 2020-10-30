using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpawnPlayEffect", menuName = 
    "CardData/PlayEffects/Spawn")]
public class SpawnPlayEffect : CardPlayEffect
{
    [SerializeField] GameObject prefabToSpawn = null;

    public override void Activate(ITargetable target)
    {
        //Test if target is damageable
        MonoBehaviour worldObject = target as MonoBehaviour;
        //If damageable, apply damage
        if(worldObject != null)
        {
            Vector3 spawnLocation = worldObject.transform.position;
            GameObject newGameObject = Instantiate(prefabToSpawn, spawnLocation,
                Quaternion.identity);
            Debug.Log("Spaw new object: " + newGameObject.name);
        }
        else
        {
            Debug.Log("Target does not have a transform...");
        }
    }
}
