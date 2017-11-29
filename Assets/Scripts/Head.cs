using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Head : MonoBehaviour
{

    public Transform arrow;
    public GameObject tail;
    public List<Transform> tails;
    public float speed;
    public AudioClip eatSound;
    public int startLength;

    FoodSpawner foodSpawn;
    Grid grid;
    Vector3 direction;
    Vector3 previousPos;
    float timer;
    bool canTurn;
    GameController gameController;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        foodSpawn = GameObject.FindGameObjectWithTag("Grid").GetComponent<FoodSpawner>();
        grid = foodSpawn.gameObject.GetComponent<Grid>();
        direction = Vector3.up;
        for (int i = 0; i < startLength; i++){

            AddTail();

        }
    }

    // Update is called once per frame
    void Update()
    {

        if (canTurn)
        {
            //Input 
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Turn("Up");
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Turn("Down");
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Turn("Left");
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Turn("Right");
            }


        }

        timer += Time.deltaTime;
        if (timer >= speed)
        {

            Movement();
            timer = 0;

        }

        gameController.tails = new Transform[tails.Count()];
        for (int i = 0; i < tails.Count; i++)
        {

            gameController.tails[i] = tails[i];

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
                tails.RemoveRange(tails.IndexOf(t), tails.Count() - tails.IndexOf(t));
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
        this.enabled = false;

        StartCoroutine(gameController.GameOver());
    }


}
