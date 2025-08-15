using UnityEngine;
using System.Collections.Generic;

public class GrassEncounter : MonoBehaviour
{
    [System.Serializable]
    public class WildEncounter
    {
        public CreatureSO creature;
        [Range(1, 100)] public int weight = 50; // probabilidad relativa
        public int minLevel = 2;
        public int maxLevel = 5;
    }

    [Header("Probabilidad (por segundo) mientras se mueve en hierba")]
    [Range(0f, 1f)] public float chancePerSecond = 0.20f;

    [Header("Tabla de encuentros")]
    public List<WildEncounter> encounters = new List<WildEncounter>();

    private PlayerMovement player;
    private bool playerInside;
    private bool locked; // para no disparar varias veces

    void Reset()
    {
        // Asegura trigger
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerMovement>();
            playerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;
        }
    }

    void Update()
    {
        if (locked || !playerInside || player == null) return;
        if (!player.IsMoving) return;

        // Aproximación: probabilidad por segundo escalada por dt
        if (Random.value < chancePerSecond * Time.deltaTime)
        {
            locked = true;
            var wi = GenerateWildInstance();
            GameManager.Instance.StartBattle(wi);
        }
    }

    WildInstance GenerateWildInstance()
    {
        if (encounters == null || encounters.Count == 0)
        {
            // Fallback: crea algo vacío si no configuraste nada
            return new WildInstance { creature = null, level = 3 };
        }

        int total = 0;
        foreach (var e in encounters) total += Mathf.Max(1, e.weight);
        int roll = Random.Range(0, total);
        foreach (var e in encounters)
        {
            roll -= Mathf.Max(1, e.weight);
            if (roll < 0)
            {
                int lvl = Random.Range(e.minLevel, e.maxLevel + 1);
                return new WildInstance { creature = e.creature, level = lvl };
            }
        }
        // Nunca debería llegar aquí, pero por seguridad:
        var last = encounters[encounters.Count - 1];
        int lv = Random.Range(last.minLevel, last.maxLevel + 1);
        return new WildInstance { creature = last.creature, level = lv };
    }
}
