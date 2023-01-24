using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public float startHP;
    public float maxHP;
    public float currentHP;

    private bool isGod;
    public float godDuration;
    public int godFlashes;

    private bool isDead;
    private Animator anim;
    private Movement move;

    public GameObject panL;

    private SpriteRenderer sprite;



    // Start is called before the first frame update
    void Start()
    {
        currentHP = startHP;
        sprite = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();
        move = GetComponent<Movement>();
    }

    public void Damage(float damage)
    {
        if (isGod || isDead)
            return;
        else
        {
            currentHP -= damage;

            if (currentHP <= 0)
            {
                isDead = true;
                Debug.Log("you are dead, not big surprise");
                //move.setState(Movement.MovementState.death);
                //move.smoothStop();
                anim.SetTrigger("DoDeath");
                Invoke("Failure", 1f);
            }
            else
                StartCoroutine(GodMode());
        }

    }
    public void AddHP(float HP)
    {
        HP = Mathf.Clamp(currentHP + HP, 0, maxHP);
    }

    private IEnumerator GodMode()
    {
        isGod = true;

        for (int i = 0; i < godFlashes; i++)
        {
            sprite.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(godDuration / (godFlashes * 2));
            sprite.color = Color.red;
            yield return new WaitForSeconds(godDuration / (godFlashes * 2));

        }
        isGod = false;
    }

   /* private void Failure()
    {
        panL.SetActive(true);
    }*/

}
