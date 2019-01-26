using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// set via drag-and-drop in the inspector
	public GameObject homeAreaObject;
    public GameObject bulletObject;
    public GameObject goalAreaObject;

    // Created somehow. These probably aren't going to be game object classes in the end
    [SerializeField]
    private GameObject _player;
    private GameObject _coparent;

    private float _gestationTimer;
    private bool _gestating;

    private float _bulletSpawnTimer;

    private Vector3 spawnPos;

    // Initialized via Start()
    private HomeArea _homeArea;
    private List<FamilyMember> _family;
    private List<GoalArea> _goals;

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

        _goals = new List<GoalArea>();

        GameObject area = Instantiate(goalAreaObject);
        area.transform.localScale = new Vector3(5, 0.5f, 5);
        area.GetComponent<GoalArea>().ActivateGoalArea(new Vector3(13.4f, 11.3f, 0), 100000, 3);
        area.GetComponent<GoalArea>().OnHealingTriggered += GoalAreaHeal;
        _goals.Add(area.GetComponent<GoalArea>());
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
        
        float _spawnDirection = Random.Range(0.0f, 1.0f);
        if(_spawnDirection <= 0.25f) //left
        {
            spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(-0.2f, Random.Range(0.0f, 1.0f), 10.0f));
        }
        else if (_spawnDirection <= 0.5f && _spawnDirection > 0.25f) //right
        {
            spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(1.2f, Random.Range(0.0f, 1.0f), 10.0f));
        }
        else if (_spawnDirection <= 0.75f && _spawnDirection > .5f) //up
        {
            spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.0f, 1.0f), 1.2f, 10.0f));
        }
        else if (_spawnDirection <= 1.0f && _spawnDirection > 0.75f) //down
        {
            spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.0f, 1.0f), -0.2f, 10.0f));
        }
        
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

    // Method to check goals
    private void CheckGoals()
    {
        foreach (GoalArea goal in _goals)
        {
            if (goal.IsAreaActive())
            {
                if (goal.Contains(_family[0].gameObject))
                {
                    if (!goal.IsHealingActive())
                    {
                        goal.ActivateHealing();
                    }
                }
                else
                {
                    if (goal.IsHealingActive())
                    {
                        goal.DeactivateHealing();
                    }
                }
            }
        }
    }

    private void GoalAreaHeal()
    {
        foreach (FamilyMember fam in _family)
        {
            fam.TakeDamage(-1);
        }
    }

    public void GameOver()
    {
        // Lose the game.
        SceneManager.LoadScene("GameOver");
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

        CheckGoals();
    }
}
