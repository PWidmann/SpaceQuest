using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnHelper: MonoBehaviour
{
    public Vector3 GetRandomPositionAroundPlanet()
    {
        // Generate a random direction around the planet
        Vector3 randomDirection = Random.onUnitSphere;

        // Scale the direction to a desired distance from the center of the planet
        float distanceFromCenter = 500f;
        randomDirection *= distanceFromCenter;

        // Return the final position
        return randomDirection;
    }

    public Vector3 RaycastToPlanetSurface(Vector3 position)
    {
        RaycastHit hit;
        Vector3 raycastDirection = (Vector3.zero - position).normalized;


        if (Physics.Raycast(position, raycastDirection, out hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.CompareTag("Lava"))
            {
                return Vector3.zero;
            }
            else
            {
                return hit.point;
            }
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 GetRandomSurfaceSpawnPoint()
    {
        Vector3 randomPosition = GetRandomPositionAroundPlanet();
        Vector3 raycastHitPosition = RaycastToPlanetSurface(randomPosition);
        return raycastHitPosition;
    }

}
