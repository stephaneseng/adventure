using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private GameObject player;
    private Slider slider;

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        slider.value = player.GetComponent<PlayerController>().GetHealth();
        slider.maxValue = player.GetComponent<PlayerController>().GetMaxHealth();
    }
}
