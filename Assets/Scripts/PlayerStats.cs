using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour {

    public int maxHealth = 1000;
    public int health{
        get{
            return _health;
        }
        set{
            if(value <= 0){
                _health = 0;
                healthSlider.value = 0;
                Die();
                return;
            }
            if(value > maxHealth){
                _health = maxHealth;
                healthSlider.value = maxHealth;
                return;
            }
            healthSlider.value = value;
            fill.color = gradient.Evaluate(healthSlider.normalizedValue);
            _health = value;
        }
    }

    private int _health;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    public int money = 0;
    [SerializeField] private TMP_Text moneyText;

    // Start is called before the first frame update
    void Start() {
        _health = maxHealth;
        healthSlider.maxValue = maxHealth;
        fill.color = gradient.Evaluate(1f);
        healthSlider.value = maxHealth;
        moneyText.text = 0.ToString();
    }

    public void AddMoney(int val){
        money += val;
        moneyText.text = money.ToString();
    }

    public void Die() {
        healthSlider.value = 0;
        Destroy(gameObject);
    }

    public void Damage(int val){
        health -= val;
        healthSlider.value = health;
        fill.color = gradient.Evaluate(healthSlider.normalizedValue);
    }
}
