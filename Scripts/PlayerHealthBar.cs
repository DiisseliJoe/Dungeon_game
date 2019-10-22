using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    private int Health;
    private int NumOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite threequarterHeart;
    public Sprite halfHeart;
    public Sprite onequarterHeart;
    public Sprite emptyHeart;


    // Start is called before the first frame update
    void Start()
    {
        Health = this.gameObject.GetComponent<HealthSystem>().Health;
        NumOfHearts = this.gameObject.GetComponent<HealthSystem>().HealthMax / 4;
        //modifies ammount of hearts visible to match player max health
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < NumOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void UpdateHealthBar()
    {
        Health = this.gameObject.GetComponent<HealthSystem>().Health;
        NumOfHearts = this.gameObject.GetComponent<HealthSystem>().HealthMax / 4;
        //modifies ammount of hearts visible to match player max health
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < NumOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        for (int i = 0; i < NumOfHearts; i++)
        {
            int x = i + 1;
            if (x * 4 < Health)
            {
                hearts[i].sprite = fullHeart;
            }
            if (x * 4 > Health && 3 == Health % 4)
            {
                hearts[i].sprite = threequarterHeart;
            }
            if (x * 4 > Health && 2 == Health % 4)
            {
                hearts[i].sprite = halfHeart;
            }
            if (x * 4 > Health && 1 == Health % 4)
            {
                hearts[i].sprite = onequarterHeart;
            }
            if (x * 4 > Health + 3.9)
            {
                hearts[i].sprite = emptyHeart;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<PlayerMovement>().EnemyAttacked == true )
        {
            Health = this.gameObject.GetComponent<HealthSystem>().Health;
            for (int i = 0; i < NumOfHearts; i++)
            {
                int x = i + 1;
                if (x * 4 < Health)
                {
                    hearts[i].sprite = fullHeart;
                }
                if (x * 4 > Health && 3 == Health % 4)
                {
                    hearts[i].sprite = threequarterHeart;
                }
                if (x * 4 > Health && 2 == Health % 4)
                {
                    hearts[i].sprite = halfHeart;
                }
                if (x * 4 > Health && 1 == Health % 4)
                {
                    hearts[i].sprite = onequarterHeart;
                }
                if(x * 4 > Health + 3.9)
                {
                    hearts[i].sprite = emptyHeart;
                }
            }
        }
    }
}
