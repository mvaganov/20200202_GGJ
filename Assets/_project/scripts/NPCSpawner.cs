using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour {
    [SerializeField] NPC prefabNPC;
    [SerializeField] Transform parentOfRegions;
    int kNumPeopleToSpawn = 10;

    private void Start() {
        UnityEngine.Assertions.Assert.IsNotNull( prefabNPC );
        UnityEngine.Assertions.Assert.IsNotNull( parentOfRegions );
        Spawn();
    }

    public static Vector3 RandomPointInRadius( Vector3 center, float radius ) {
        float xOffset = UnityEngine.Random.Range(-radius, radius);
        float zOffset = UnityEngine.Random.Range(-radius, radius);
        return center + new Vector3( xOffset, 0, zOffset );
    }

    // Find a point within the mesh, then return a point right on top of mesh
    public static Vector3 GetRandomPointWithinMeshRegion( MeshCollider mesh ) {
        float range = 20;
        Vector3 randomPoint = RandomPointInRadius( mesh.bounds.center, range);
        randomPoint = mesh.ClosestPoint( randomPoint );
        randomPoint.y = mesh.bounds.max.y;
        return randomPoint;
    }

    private void Spawn() {
        for ( int i = 0; i< parentOfRegions.childCount; i++ ) {
            GameObject currentRegion = parentOfRegions.GetChild(i).GetChild(0).gameObject;

            MeshRenderer currRegionVisual = currentRegion.GetComponent<MeshRenderer>();
            UnityEngine.Assertions.Assert.IsNotNull( currRegionVisual );
            Material regionColor = currRegionVisual.material;

            MeshCollider currRegionBounds = currentRegion.GetComponent<MeshCollider>();
            UnityEngine.Assertions.Assert.IsNotNull( currRegionBounds );
            currRegionBounds.enabled = true;

            for ( int j = 0; j < kNumPeopleToSpawn; j++ ) {
                // Spawn people within region bounds
                Vector3 randomPositionInBounds = GetRandomPointWithinMeshRegion( currRegionBounds );
                NPC cloneNPC = Instantiate(prefabNPC);
                cloneNPC.transform.position = randomPositionInBounds;
                cloneNPC.BeginPathing( currRegionBounds );

                // Set color to match region
                var cloneVisual = cloneNPC.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
                UnityEngine.Assertions.Assert.IsNotNull( cloneVisual );
                cloneVisual.material = regionColor;
            }

            // Hide the region
            currRegionVisual.enabled = false;
            currRegionBounds.enabled = false;
        }
    }

}
