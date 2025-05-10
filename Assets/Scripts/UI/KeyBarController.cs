using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyBarController : MonoBehaviour
{
    private GameObject player;

    private readonly Dictionary<int, GameObject> keys = new();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < GameConstants.GameMaxNumberOfKeys; i++) keys.Add(i, transform.Find("Key" + i).gameObject);

        keys.Values.ToList().ForEach(key => key.SetActive(false));
    }

    private void Update()
    {
        int numberOfKeys = player.GetComponent<PlayerController>().Keys;

        for (int i = 0; i < GameConstants.GameMaxNumberOfKeys; i++) keys[i].SetActive(i < numberOfKeys);
    }
}
