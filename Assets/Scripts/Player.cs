using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;

    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;

        base.Start();
    }

    private void OnDisable() {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playersTrun)
            return;
        
        int horizontal = 0, vertival = 0;
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertival = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertival = 0;

        if (horizontal != 0 || vertival != 0)
            AttamptMove<Wall> (horizontal, vertival);
    }

    protected override void AttamptMove <T> (int xDir, int yDir) {
        food--;

        base.AttamptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        CheckIfGameOver();

        GameManager.instance.playersTrun = false;
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Exit") {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food") {
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda") {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove <T> (T component) {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private void Restart() {
        SceneManager.GetActiveScene();
    }

    public void LoseFood(int loss) {
        animator.SetTrigger("playerHit");
        food -= loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver() {
        if (food <= 0)
            GameManager.instance.GameOver();
    }
}
