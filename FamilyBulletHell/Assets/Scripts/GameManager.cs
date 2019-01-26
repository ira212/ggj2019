using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SpawnPlayer();
       

        // Spawn a random number of bullets at the start of the game. +1 on the second argument for Random.Range because if you give it ints, the 2nd argument is exclusive.
        for (int i = Random.Range(Global.Instance.BulletStartSpawnMin, Global.Instance.BulletStartSpawnMax +1); i > 0; i--)
        {
            SpawnBullet();
        }
    }

	void Gestate() {
	    // confirm that both parents are in the home area
        if (_homeArea.Contains(_family[0].gameObject) && _homeArea.Contains(_coparent))
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

        // reset gestation timer if both parents aren't home
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

    private void SpawnPlayer()
    {
        Vector3 playerSpawnPos = new Vector3(0.0f, 0.0f, 0.0f);

        GameObject newPlayer = Instantiate(_player);
        newPlayer.transform.localScale = new Vector3(Global.Instance.SquareScale, Global.Instance.SquareScale, 1);
        newPlayer.GetComponent<Player>().Spawn(playerSpawnPos);
        newPlayer.GetComponent<FamilyMember>().OnFamilyMemberDeath += GameOver;
        _family.Add(newPlayer.GetComponent<FamilyMember>());
    }

    public void GameOver()
    {
        // Lose the game.
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn bullet
        if (_bulletSpawnTimer > 0)
        {
            _bulletSpawnTimer -= Time.deltaTime;
        }
        if (_bulletSpawnTimer <= 0)
        {
            SpawnBullet();

            _bulletSpawnTimer = Random.Range(Global.Instance.NewBulletSpawnMin, Global.Instance.NewBulletSpawnMax);
        }


        // check if we should be gestating a baby
        Gestate();

    }

}
