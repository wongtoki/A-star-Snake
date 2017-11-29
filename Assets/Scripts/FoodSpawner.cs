using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

    public GameObject food;
    public int initSpawn;
    public int margin;
    Grid grid;
    GameController gameController;
    public List<Transform> foods;



	// Use this for initialization
	void Start () {

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        grid = GetComponent<Grid>();

        int i = 0;
        while( i < initSpawn){

            SpawnFood();
            i++;
        }


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnFood(){


        int x = Random.Range(-grid.width + margin, grid.width - margin);
        int y = Random.Range(-grid.height + margin, grid.height - margin);

        Vector3 pos = new Vector3(x, y, 0);



        foreach (Transform f in foods) {
            
            if (f.position == pos){
                SpawnFood();
                return;
            }

            if(GameObject.FindGameObjectWithTag("Head")!=null){
                foreach (Transform t in gameController.tails)
                {

                    if (t.position == pos)
                    {

                        SpawnFood();
                        return;

                    }
                } 
            }
        }

        GameObject instance = Instantiate(food);
        instance.transform.position = pos;
        foods.Add(instance.transform);

    }

}
