using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    public GameObject bulletPrefab;
    public int poolSize = 20;

    private List<GameObject> pool =
        new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Pre-spawn all bullets
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            pool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        // Find an inactive bullet
        foreach (GameObject bullet in pool)
        {
            if (!bullet.activeInHierarchy)
                return bullet;
        }

        // Pool exhausted — create extra
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.SetActive(false);
        pool.Add(newBullet);
        return newBullet;
    }
}