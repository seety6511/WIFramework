using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WIFramework;

public class MonsterSpawner : MonoBehaviour
{
    public Monster prefab_Monster;
    public List<Monster> monsters = new List<Monster>();
    public float speed;
    public TextMeshProUGUI monsterCount;
    float timer = 0f;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= speed)
        {
            timer = 0f;
            var nm = Instantiate(prefab_Monster);
            monsters.Add(nm);
            monsterCount.SetText($"{monsters.Count}");
        }
    }
}
