using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyCont : MonoBehaviour
{   // уничтожение ключа, если соприкоснулся с игроком
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
