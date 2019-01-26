using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// set via drag-and-drop in the inspector
	public GameObject homeAreaObject;
    public GameObject bulletObject;

    // Created somehow. These probably aren't going to be game object classes in the end
    [SerializeField]
    private GameObject _player;
    private GameObject _coparent;

    private float _gestationTimer;
    private bool _gestating;

    private float _bulletSpawnTimer;

    // Initialized via Start()
    private HomeArea _homeArea;
    private List<FamilyMember> _family;

	// Start is called before the first frame update
    void Start()
    {

        // placeholder initialization
        _coparent = new GameObject();

        _homeArea = homeAreaObject.GetComponent<HomeArea>();
        _family = new List<FamilyMember>();

        _player.GetComponent<FamilyMember>().OnFamilyMemberDeath += GameOver;
        _family.Add(_player.GetComponent<FamilyMember>());
    }

	void Gestate() {
	    // confirm that both parents are in the home area
        if (_homeArea.Contains(_player) && _homeArea.Contains(_coparent))
        {
            // start to gestate by increasing gestation timer
            if (!_gestating)
            {
                _gestating = true;
                _gestationTimer = Random.Range(Global.Instance.childSpawnTimeMin, Global.Instance.childSpawnTimeMax);
            }

            _gestationTimer -= Time.deltaTime;

            // are we ready to have a baby?
            if (_gestationTimer < 0.0f)
            {
                _gestating = false;
                SpawnChild();
            }
        }

        // reset gestation timer
        else
        {
            _gestating = false;
        }
        // 

	}

    // This should cause a new child to be spawned
    void SpawnChild()
    {


    }
    // Update is called once per frame
    void Update()
    {
        if (_bulletSpawnTimer > 0)
        {
            _bulletSpawnTimer -= Time.deltaTime;
        }
        if(_bulletSpawnTimer <= 0)
        {
            SpawnBullet();

            _bulletSpawnTimer = Random.Range(Global.Instance.NewBulletSpawnMin, Global.Instance.NewBulletSpawnMax);
        }
		// check if we should be gestating a baby
		Gestate();

    }

    // Method used to spawn a bullet
    private void SpawnBullet()
    {
        // create random spawn location that's not in the camera view
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(-0.2f, 1.2f), Random.Range(-0.2f, 1.2f), 10.0f));
        // create a random destination point that takes the bullet through the camera view
        Vector3 destination = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), 10.0f));
        // pick a random bullet speed in range
        float bulletSpeed = Random.Range(Global.Instance.bulletSpeedMin, Global.Instance.bulletSpeedMax);

        GameObject newBullet = Instantiate(bulletObject);
        newBullet.transform.localScale = new Vector3(Global.Instance.BulletScale, Global.Instance.BulletScale, 1);
        newBullet.GetComponent<Bullet>().Spawn(spawnPos, destination, bulletSpeed);
    }

    public void GameOver()
    {
        // Lose the game.
    }
}
