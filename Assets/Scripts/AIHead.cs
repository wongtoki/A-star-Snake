using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIHead : MonoBehaviour
{


    public Transform arrow;
    public GameObject tail;
    public List<Transform> tails;
    public float speed;
    public bool canDie;
    public int startLength;
    public AudioClip eatSound;

    PlayerPathFinder pathFinder;
    FoodSpawner foodSpawn;
    Grid grid;
    Vector3 direction;
    Vector3 previousPos;
    float timer;
    bool canTurn;
    Vector3 NextPos;

    public Vector3[] waypoints;

    public Transform targetFood;
    GameController gameController;

    // Use this for initialization
    void Start()
    {

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        pathFinder = GetComponent<PlayerPathFinder>();
        foodSpawn = GameObject.FindGameObjectWithTag("Grid").GetComponent<FoodSpawner>();
        grid = foodSpawn.gameObject.GetComponent<Grid>();
        direction = Vector3.up;

        for (int i = 0; i < startLength; i++)
        {

            AddTail();

        }

        targetFood = FindFood();
        waypoints = pathFinder.FindPath(transform.position, targetFood.position, UnwalkableArea());

    }

  

    void AIInput(Vector3 pos){
        Vector3 dir = pos - transform.position;
        if(dir == Vector3.up){
            Turn("Up");
        }else if(dir == Vector3.down){
            Turn("Down");
        }else if (dir == Vector3.left){
            Turn("Left");
        }else if(dir == Vector3.right){
            Turn("Right");
        }

    }


    Transform FindFood(){

        Transform food = null;

        int r = UnityEngine.Random.Range(0, foodSpawn.foods.Count());
        food = foodSpawn.foods[r];
      
        int lowestCostRoute = int.MaxValue;
        foreach (Transform f in foodSpawn.foods)
        {
            pathFinder.FindPath(transform.position, f.position, UnwalkableArea());
            if(pathFinder.routeCost < lowestCostRoute){

                lowestCostRoute = pathFinder.routeCost;
                food = f;

            }
        }

        return food;

    }


    private void Update()
    {
        timer += Time.deltaTime;
        gameController.tails = tails.ToArray();
        if (timer >= speed)
        {



            if(waypoints!=null){

                NextPos = waypoints[1];
                AIInput(NextPos);

                LineRenderer line = GetComponent<LineRenderer>();
                line.positionCount = waypoints.Length;
                line.SetPositions(waypoints);
            }



            Movement();
            waypoints = pathFinder.FindPath(transform.position, targetFood.position, UnwalkableArea());
            timer = 0;

        }
    }


    void Turn(string dir)
    {

        if (dir == "Up")
        {
            if (direction != Vector3.down)
            {
                direction = Vector3.up;
                arrow.rotation = Quaternion.Euler(Vector3.zero);
                canTurn = false;
            }
        }

        if (dir == "Down")
        {
            if (direction != Vector3.up)
            {
                direction = Vector3.down;
                arrow.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                canTurn = false;
            }
        }

        if (dir == "Left")
        {
            if (direction != Vector3.right)
            {
                direction = Vector3.left;
                arrow.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                canTurn = false;
            }
        }

        if (dir == "Right")
        {
            if (direction != Vector3.left)
            {
                direction = Vector3.right;
                arrow.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                canTurn = false;
            }

        }

    }


    void Movement()
    {

      
        previousPos = transform.position;
        transform.position += direction;

        Vector3 pos = transform.position;
        if(pos.x >= grid.width || pos.x <= -grid.width || pos.y >= grid.height || pos.y <= -grid.height){

            Die();

        }


        canTurn = true;
        foreach (Transform f in foodSpawn.foods)
        {

            if (transform.position == f.position)
            {

                EatFruit(f);
                break;
            }

        }

        TailMovement();

        foreach (Transform t in tails)
        {

            if (transform.position == t.position)
            {
                
                for (int i = tails.IndexOf(t); i < tails.Count(); i++)
                {
                    Destroy(tails[i].gameObject);

                }
                tails.RemoveRange(tails.IndexOf(t),tails.Count()-tails.IndexOf(t));
                break;
            }

        }
               



    }

    void EatFruit(Transform food)
    {

        

        foreach (Transform f in foodSpawn.foods)
        {

            if (food == f)
            {

                foodSpawn.foods.Remove(f);
                Destroy(f.gameObject);
                break;
            }

        }

        AddTail();
        foodSpawn.SpawnFood();
        gameController.PlayEffectSound(eatSound);
        targetFood = FindFood();

    }

    void AddTail()
    {
        
        GameObject instance = Instantiate(tail);

        tails.Add(instance.transform);

    }

    void TailMovement()
    {

        if (tails.Count > 0)
        {

            tails.Last().position = previousPos;

            // Add to front of list, remove from the back
            tails.Insert(0, tails.Last());
            tails.RemoveAt(tails.Count - 1);

        }


    }

    void Die()
    {

        if(!canDie){

            return;

        }

        this.enabled = false;
        GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        StartCoroutine(gameController.GameOver());
    }

    Vector2[] UnwalkableArea(){

        List<Vector2> areas = new List<Vector2>();
        

        foreach(Transform t in tails){

            areas.Add(t.transform.position);

        }

        return areas.ToArray();

    }

    bool CheckPositionOnTail(Vector3 pos){


        foreach (Transform t in tails)
        {

            if (pos == t.position){

                return true;

            }

        }

        return false;
    }


}
