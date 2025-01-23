using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public float zSpawn = 0;
    public float tileLength = 30;
    // Start is called before the first frame update

    public int numberOfTiles = 4;
    private List<GameObject> activeTiles = new List<GameObject>();
    public Transform playerTransform;
    void Start()
    {
       for(int i=0;i<numberOfTiles; i++){
              SpawnTile(Random.Range(0,tilePrefabs.Length));
       }
    }

    // Update is called once per frameS
    void Update()
    {
        if(playerTransform.position.z - 35>zSpawn-(numberOfTiles * tileLength)){
             SpawnTile(Random.Range(0,tilePrefabs.Length));
             DeleteTile();
        }
    }
    public void SpawnTile(int tileIndex){
            GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
            activeTiles.Add(go);
            zSpawn += tileLength;
        }

    private void DeleteTile(){
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
}
