using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	// set via drag-and-drop in the inspector
	public GameObject homeAreaObject;
    public GameObject bulletObject;
    public GameObject goalAreaObject;
    public GameObject healthMeter;
    public GameObject scoreCounter;
    public Text _coparentHealthText;
    public GameObject[] familyMemberPrefabs;

    // Created somehow. These probably aren't going to be game object classes in the end
    [SerializeField]
    private GameObject _player;
    private GameObject _coparent;

    private GameObject newPlayer;
    private GameObject newChild;

    private float _gestationTimer;
    private bool _gestating;
    private bool _inGoalArea;

    private float _bulletSpawnTimer;

    private Vector3 spawnPos;

    private Text _healthText;
    private Text _scoreText;
    private float _score;
    private int _intScore;

    // Initialized via Start()
    private HomeArea _homeArea;
    private List<FamilyMember> _family;
    private List<GoalArea> _goals;
    private List<Vector3> _possiblePositions;

	// Start is called before the first frame update
    void Start()
    {
        _homeArea = homeAreaObject.GetComponent<HomeArea>();
        _family = new List<FamilyMember>();
        SpawnPlayer();
        _healthText = healthMeter.GetComponent<Text>();
        _scoreText = scoreCounter.GetComponent<Text>();
        _score = 0.0f;
        _intScore = 0;
        _inGoalArea = false;

        // Spawn a random number of bullets at the start of the game. +1 on the second argument for Random.Range because if you give it ints, the 2nd argument is exclusive.
        for (int i = Random.Range(Global.Instance.BulletStartSpawnMin, Global.Instance.BulletStartSpawnMax +1); i > 0; i--)
        {
            SpawnBullet();
        }

        SpawnSpouse();

        CreateGoalZonePositions();
        CreateGoalZones();

        StartCoroutine(ScoreCoroutine());
        
        AudioManager.Instance.PlayMusic("Bass-And-Chords");
        AudioManager.Instance.PlayMusic("Partner1-Happy");
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
        Debug.Log("Spawn child");
        Vector3 playerSpawnPos = newPlayer.transform.position;
        float childTypeChance;
        childTypeChance = Random.Range(0.0f, 1.0f);
        if (childTypeChance <= 0.5f)
        {
            newChild = Instantiate(familyMemberPrefabs[0]);
            newChild.transform.localScale = new Vector3(Global.Instance.SquareScale / 2, Global.Instance.SquareScale / 2, 1);
        }
        else
        {
            newChild = Instantiate(familyMemberPrefabs[1]);
            newChild.transform.localScale = new Vector3(Global.Instance.TriangleScale / 2, Global.Instance.TriangleScale / 2, 1);
        }
        
        newChild.GetComponent<FamilyMember>().SpawnFamilyMember(Global.Instance.StartHP, Global.Instance.TriangleSpeed, false);
        newChild.GetComponent<FamilyMember>().OnFamilyMemberDeath += GameOver;
        newChild.AddComponent<FamilyBehavior>();
        newChild.GetComponent<FamilyBehavior>().InitBehavior(_family[0].transform, Global.Instance.ChildAttenSpan, Global.Instance.ChildSpeed, -22, 22, -18, 18);
        newChild.transform.position = playerSpawnPos;
        _family.Add(newPlayer.GetComponent<FamilyMember>());

        Global.Instance.FinalScore += 100;

        AudioManager.Instance.PlaySFX("Child-Appears");
        AudioManager.Instance.PlayMusic("Child-Happy");
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

        newPlayer = Instantiate(_player);
        newPlayer.transform.localScale = new Vector3(Global.Instance.SquareScale, Global.Instance.SquareScale, 1);
        newPlayer.GetComponent<Player>().Spawn(playerSpawnPos);
        newPlayer.GetComponent<FamilyMember>().OnFamilyMemberDeath += GameOver;
        newPlayer.GetComponent<FamilyMember>().HealthMonitor(newPlayer.transform);
        _family.Add(newPlayer.GetComponent<FamilyMember>());
    }

    private void SpawnSpouse()
    {
        _coparent = Instantiate(familyMemberPrefabs[1]);
        Vector3 spawnPos = new Vector3(-22f, 14, 0);
        _coparent.transform.position = spawnPos;
        _coparent.GetComponent<FamilyMember>().SpawnFamilyMember(Global.Instance.StartHP, Global.Instance.TriangleSpeed, true);
        _coparent.GetComponent<FamilyMember>().OnFamilyMemberDeath += GameOver;
        _coparent.AddComponent<FamilyBehavior>();
        _coparent.GetComponent<FamilyBehavior>().InitBehavior(_family[0].transform, Global.Instance.TriangleAttSpan, Global.Instance.TriangleSpeed, -22, 22, -18, 18);
        _family.Add(_coparent.GetComponent<FamilyMember>());

        AudioManager.Instance.PlayMusic("Partner2-Happy");
    }

    private void CreateGoalZones()
    {
        _goals = new List<GoalArea>();

        for (int i = 0; i < 2; ++i)
        {
            int posIndex = Random.Range(0, _possiblePositions.Count);
            bool badPos = false;
            while (badPos)
            {
                badPos = false;
                foreach (GoalArea goal in _goals)
                {
                    if (_possiblePositions[posIndex].Equals(goal.transform.position))
                    {
                        badPos = true;
                    }
                }
            }
            GameObject area = Instantiate(goalAreaObject);
            float radius = Random.Range(Global.Instance.GoalAreaRadMin, Global.Instance.GoalAreaRadMax);
            area.transform.localScale = new Vector3(radius, 0.5f, radius);
            float duration = Random.Range(Global.Instance.GoalAreaHeal + 2.0f, Global.Instance.GoalAreaHeal + 3.0f);
            area.GetComponent<GoalArea>().ActivateGoalArea(_possiblePositions[posIndex], duration, Global.Instance.GoalAreaHeal);
            area.GetComponent<GoalArea>().OnHealingTriggered += GoalAreaHeal;
            area.GetComponent<GoalArea>().OnRespawnReady += ActivateGoalArea;
            _goals.Add(area.GetComponent<GoalArea>());
        }
    }

    private void CreateGoalZonePositions()
    {
        _possiblePositions = new List<Vector3>();
        _possiblePositions.Add(new Vector3(13.4f, 11.3f, 3));
        _possiblePositions.Add(new Vector3(-20, -10, 3));
        _possiblePositions.Add(new Vector3(0f, 11.3f, 3));
        _possiblePositions.Add(new Vector3(13.4f, 11.3f, 3));
        _possiblePositions.Add(new Vector3(-20, 0, 3));
        _possiblePositions.Add(new Vector3(0, -10, 3));
        _possiblePositions.Add(new Vector3(13.4f, 0, 3));
    }

    // Method to check goals
    private void CheckGoals()
    {
        int goalsOccupied = 0;

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

                    ++goalsOccupied;
                }
                else
                {
                    if (goal.IsHealingActive())
                    {
                        goal.DeactivateHealing();
                    }
                }
            }
            else
            {
                goal.UpdateRespawnTimer(Time.deltaTime);
            }
        }

        if (goalsOccupied > 0)
        {
            _inGoalArea = true;
        }
        else
        {
            _inGoalArea = false;
        }
    }

    private void GoalAreaHeal()
    {
        foreach (FamilyMember fam in _family)
        {
            fam.TakeDamage(-1);
        }
    }

    private void ActivateGoalArea(GoalArea area)
    {
        int positionIndex = Random.Range(0, _possiblePositions.Count);
        bool badPos = false;
        while (badPos)
        {
            badPos = false;
            foreach (GoalArea goal in _goals)
            {
                if (_possiblePositions[positionIndex].Equals(goal.transform.position))
                {
                    badPos = true;
                }
            }
        }

        float radius = Random.Range(Global.Instance.GoalAreaRadMin, Global.Instance.GoalAreaRadMax);
        float duration = Random.Range(Global.Instance.GoalAreaHeal + 2.0f, Global.Instance.GoalAreaHeal + 3.0f);
        area.transform.localScale = new Vector3(radius, 0.5f, radius);
        area.ActivateGoalArea(_possiblePositions[positionIndex], duration, Global.Instance.GoalAreaHeal);
    }

    public void GameOver()
    {
        // Lose the game.
        SceneManager.LoadScene("GameOver");
        StopCoroutine(ScoreCoroutine());
    }

    private IEnumerator ScoreCoroutine()
    {
        while (true)
        {
            Global.Instance.FinalScore++;
            if (_inGoalArea)
            {
                Global.Instance.FinalScore += Global.Instance.GoalScorePerSecond;
            }
            _scoreText.text = "Score: " + Global.Instance.FinalScore.ToString();

            yield return new WaitForSeconds(1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                FamilyBehavior behave = hit.collider.GetComponent<FamilyBehavior>();
                if (behave != null)
                {
                    behave.GrabAttention();
                }
            }
        }

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

        //_score = Global.Instance.FinalScore + Time.deltaTime;
        //Global.Instance.FinalScore = (int)_score;

        _healthText.text = "Health: " + newPlayer.GetComponent<FamilyMember>().GetHealth().ToString();
        _coparentHealthText.text = "Partner Health: " + _coparent.GetComponent<FamilyMember>().GetHealth().ToString();
        // check if we should be gestating a baby
        Gestate();

        CheckGoals();
    }
}
