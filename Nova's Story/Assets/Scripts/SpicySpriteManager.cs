using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpicySpriteManager : MonoBehaviour {

    private List<Bullet> bulletList;
    private static SpicySpriteManager Instance;
    [SerializeField] private GameObject bulletPrefab;

	// Use this for initialization
	void Awake () {
        bulletList = new List<Bullet>();
        if(SpicySpriteManager.Instance != null)
        {
            Debug.LogError("2 or more SpicySpriteManager instances were found!");
        }
        SpicySpriteManager.Instance = this;
	}
	

    public static void AddBullet(Vector3 position, Quaternion rotation)
    {
        bool allBulletsActive = true;
        for(int i = 0; i < Instance.bulletList.Count; i++)
        {
            Bullet bullet = Instance.bulletList[i];
            if (!bullet.gameObject.activeInHierarchy)
            {
                bullet.transform.position = position;
                bullet.transform.rotation = rotation;
                bullet.gameObject.SetActive(true);
                bullet.transform.Rotate(new Vector3(0, -90, 0));
                allBulletsActive = false;
                break;
            }
        }

        if (allBulletsActive)
        {
            GameObject bullet = Instantiate(Instance.bulletPrefab, position, rotation);
            bullet.transform.Rotate(0, -90, 0);
            Instance.bulletList.Add(bullet.GetComponent<Bullet>());
        }
        
    }
}
