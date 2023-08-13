using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyBarController : MonoBehaviour
{
    private GameObject player;

    private Dictionary<int, GameObject> keys = new Dictionary<int, GameObject>();

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < PlayerController.MaxNumberOfKeys; i++)
        {
            keys.Add(i, transform.Find("Key" + i).gameObject);
        }

        keys.Values.ToList().ForEach(key => key.SetActive(false));
    }

    void Update()
    {
        int numberOfKeys = player.GetComponent<PlayerController>().GetKeys();

        for (int i = 0; i < PlayerController.MaxNumberOfKeys; i++)
        {
            keys[i].SetActive(i < numberOfKeys);
        }
    }
}
