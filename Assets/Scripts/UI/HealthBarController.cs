using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private GameObject player;
    private Slider slider;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        slider.value = player.GetComponent<PlayerController>().Health;
        slider.maxValue = player.GetComponent<PlayerController>().MaxHealth;
    }
}
