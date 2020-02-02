using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnerForBoxes : MonoBehaviour {
    [SerializeField] NPCWithBoxes prefabNPC;
    [SerializeField] Transform parentOfRegions;
    int kNumPeopleToSpawn = 12;

    private void Start() {
        UnityEngine.Assertions.Assert.IsNotNull( prefabNPC );
        UnityEngine.Assertions.Assert.IsNotNull( parentOfRegions );
        Spawn();
    }

	//// A random part within, but far above a region
	//public static Vector3 RandomPointInRadius( Vector3 center, float radius ) {
	//    float xOffset = UnityEngine.Random.Range(-radius, radius);
	//    float zOffset = UnityEngine.Random.Range(-radius, radius);
	//    return center + new Vector3( xOffset, 999, zOffset );
	//}

	//// Find a point within the mesh, then return a point right on top of mesh
	//public static Vector3 GetRandomPointWithinMeshRegion(Collider collider ) {
	//    float range = 20;
	//    Vector3 randomPoint = RandomPointInRadius( collider.bounds.center, range);

	//    RaycastHit hit;
	//    if ( Physics.Raycast( randomPoint, -Vector3.up, out hit ) ) {
	//        randomPoint.y -= hit.distance;
	//    }

	//    return randomPoint;
	//}

	public static Vector3 RandomPointInBounds(BoxCollider box)
	{
		Vector3 p = box.center + new Vector3(
		   (Random.value - 0.5f) * box.size.x,
		   (Random.value - 0.5f) * box.size.y,
		   (Random.value - 0.5f) * box.size.z
		);
		p.Scale(box.transform.localScale);
		p = box.transform.rotation * p;
		return box.transform.position + p;
	}

	private void Spawn() {
		for (int i = 0; i < parentOfRegions.childCount; i++) {
			Transform side = parentOfRegions.GetChild(i);
			//currRegion.enabled = true;

			for (int j = 0; j < kNumPeopleToSpawn; j++) {
				// get a step for the meeple
				GameObject currentRegion = side.GetChild(Random.Range(0, side.childCount)).gameObject;
				MeshRenderer currRegionVisual = currentRegion.GetComponent<MeshRenderer>();
				UnityEngine.Assertions.Assert.IsNotNull(currRegionVisual);
				Material regionColor = currRegionVisual.material;
				BoxCollider currRegion = currentRegion.GetComponent<BoxCollider>();
				UnityEngine.Assertions.Assert.IsNotNull(currRegion);

				// Spawn meeple within region bounds
				Vector3 randomPositionInBounds = RandomPointInBounds(currRegion); //GetRandomPointWithinMeshRegion( currRegionBounds );
				NPCWithBoxes cloneNPC = Instantiate(prefabNPC);
				cloneNPC.transform.position = randomPositionInBounds;
				cloneNPC.BeginPathing(currRegion);

				// Set color to match region
				var cloneVisual = cloneNPC.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
				UnityEngine.Assertions.Assert.IsNotNull(cloneVisual);
				cloneVisual.material = regionColor;
			}
		}
		for (int i = 0; i < parentOfRegions.childCount; i++)
		{
			Transform side = parentOfRegions.GetChild(i);
			side.gameObject.SetActive(false);
			//	// Hide the region
			//	currRegionVisual.enabled = false;
			//	currRegion.enabled = false;
        }
    }

}
