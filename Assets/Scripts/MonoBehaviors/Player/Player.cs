﻿using UnityEngine;
public class Player : MonoBehaviour
{

    [SerializeField] Collected collectedPrephab = default;
    [SerializeField] PlayerInventory inventory = default;
    [SerializeField] Vector2 bounceHitForce = default;

    Rigidbody2D r2d;

    bool dead = false;
    Animator animator;

    void Start()
    {
        inventory = GetComponent<PlayerInventory>();
        r2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //the inventoryUI is not destroyed and keep track of player hearts
        //thats a simple not correct to keep the player and the inventory synchronized
        inventory.SetHearts(UIManager.Instance.PlayerInventory.GetHeartsInt());
        inventory.SetHealth(UIManager.Instance.PlayerInventory.GetHealth());

        EventsManager.Instance.TimeOut.AddListener(OnTimeOut);


    }

    private void InstantiateCollectedPrefab(Vector3 position)
    {
        var instance = Instantiate(collectedPrephab);
        instance.transform.position = position;
    }


    public void OnDesappearEnd()
    {
        UIManager.Instance.OnLevelReset();
        Die();
        Reset();
        if (inventory.GetHearts() <= 0)
        {
            EventsManager.Instance.Playerkilled?.Invoke();
        }
    }

    void OnTimeOut()
    {
        animator.SetTrigger("Desappear");
    }

    public void Reset()
    {
        dead = false;
        transform.position = GameObject.Find("/Level" + GameManager.Instance.GameSettings.currentLevel + "(Clone)/GamePoints/SpawnPoint").transform.position;
    }
    void Die()
    {
        inventory.SetHearts(inventory.GetHearts() - 1);
        inventory.SetHealth(inventory.maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.GetContact(0).collider.tag == "Enemies" && !dead)
        {
            OnEnemyHit(collision.gameObject.GetComponent<Enemy>());
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemies" && !dead)
        {
            OnEnemyHit(collision.gameObject.GetComponent<Enemy>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Collectables")
        {
            SoundManager.Instance.playSound("coin");
            InstantiateCollectedPrefab(collision.gameObject.transform.position);
            Destroy(collision.gameObject);
            inventory.CollectItem(collision.gameObject.GetComponent<Collectable>().item.id);
            if (inventory.CheckAllCollected())
            {
                EventsManager.Instance.AllItemsCollected.Invoke();
            }
        }
        else if (collision.gameObject.tag == "Heart")
        {
            SoundManager.Instance.playSound("coin");
            InstantiateCollectedPrefab(collision.gameObject.transform.position);
            Destroy(collision.gameObject);
            inventory.SetHearts(inventory.GetHearts() + 1);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            SoundManager.Instance.playSound("coin");
            InstantiateCollectedPrefab(collision.gameObject.transform.position);
            Destroy(collision.gameObject);
            inventory.UpdateBullets(10);
            EventsManager.Instance.OnBulletsAmountChanged.Invoke();
        }
    }

    void OnEnemyHit(Enemy enemy)
    {
        SoundManager.Instance.playSound("hit");
        inventory.SetHealth(inventory.GetHealth() - enemy.GetDamagePower());
        r2d.AddForce(
            new Vector2(
                -transform.localScale.x * bounceHitForce.x
                , r2d.velocity.y < bounceHitForce.y ? Mathf.Sign(r2d.velocity.y) * bounceHitForce.y : 0),
            ForceMode2D.Impulse
                );

        if (inventory.GetHealth() <= 0)
        {
            dead = true;
            animator.SetTrigger("Desappear");
        }
    }

}




