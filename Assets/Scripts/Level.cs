using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    private const float Camera_size = 50f;
    private const float Pipe_width = 13f;
    public const float Move_speed = 30f;
    private const float x_destroy = -100f;
    private const float x_spawn = 100f;
    private const float cloud_spawn = 192f;
    private const float cloud_destroy = -192f;
    private const float Bird_position = 0f;

    private static Level instance;
    public static Level GetInstance()
    {
        return instance;
    }

    private List<Pipe> pipeList;
    private List<Transform> cloudsList;
    private float cloudSpawnTimer;
    public int score = 0;
    private int pipesSpawned;
    private float pipeSpawnTimer = 2f;
    private float TimeBetweenSpawns;
    private float gapSize;

    private State state;
    private enum State {
        WaitingToStart,
        Playing,
        BirdDead,
    }
    

public enum Difficulty {
        Easy,
        Medium,
        Hard,
        Impossible,
    }        

    private void Awake() {
        instance = this;
        pipeList = new List<Pipe>();
        SpawnInitialClouds();
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
    }

    private void Start() {
        Bird.GetInstance().OnDied += Bird_OnDied;
        Bird.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;
        pipeList = new List<Pipe>();
    }

    private void Bird_OnStartedPlaying(object sender, System.EventArgs e) {
        state = State.Playing;
    }

    private void Bird_OnDied(object sender, System.EventArgs e) {
        state = State.BirdDead;
    }

    private void Update() {
        if (state == State.Playing) {
            PipeMovement();
            PipeSpawning();
            HandleClouds();
        }
    }

    private void SpawnInitialClouds()
    {
        cloudsList = new List<Transform>();
        Transform cloudsTransform;
        cloudsTransform = Instantiate(GameAssets.GetInstance().pfClouds, new Vector3(0, 0, 0), Quaternion.identity);
        cloudsList.Add(cloudsTransform);
        cloudsTransform = Instantiate(GameAssets.GetInstance().pfClouds, new Vector3(cloud_spawn, 0, 0), Quaternion.identity);
        cloudsList.Add(cloudsTransform);
    }

    private void SpawnNewCloud()
    {
        Transform cloudsTransform;
        cloudsTransform = Instantiate(GameAssets.GetInstance().pfClouds, new Vector3(cloud_spawn, 0, 0), Quaternion.identity);
        cloudsList.Add(cloudsTransform);
    }

    private void HandleClouds()
    {
        /*
        // Handle Cloud Spawning
        cloudSpawnTimer -= Time.deltaTime;
        if (cloudSpawnTimer < 0)
        {
            // Time to spawn another cloud
            float cloudSpawnTimerMax = 6f;
            cloudSpawnTimer = cloudSpawnTimerMax;
            Transform cloudTransform = Instantiate(GameAssets.GetInstance().pfClouds, new Vector3(cloud_spawn, 0, 0), Quaternion.identity);
            cloudsList.Add(cloudTransform);
        }
        */

        // Handle Cloud Moving
        for (int i = 0; i < cloudsList.Count; i++)
        {
            Transform cloudTransform = cloudsList[i];
            // Mover nuvens mais devagar, para efeito Parallax
            cloudTransform.position += new Vector3(-1, 0, 0) * Move_speed * Time.deltaTime * 0.7f;

            if (cloudTransform.position.x < cloud_destroy)
            {
                // Cloud past destroy point, destroy self
                Destroy(cloudTransform.gameObject);
                cloudsList.RemoveAt(i);
                i--;

                SpawnNewCloud();

            }
        }

        
    }
        
    private void PipeSpawning() {
        
        if (Time.time >= pipeSpawnTimer) {
        // Time to spawn another Pipe
            pipeSpawnTimer = TimeBetweenSpawns + Time.time;
            

            // Definindo a altura dos pipes
            float heightEdgeLimit = 20f;
            float minHeight = gapSize / 2f + heightEdgeLimit;
            float totalHeight = Camera_size * 2f;
            float maxHeight = totalHeight - gapSize / 2f - heightEdgeLimit;

            float height = Random.Range(minHeight, maxHeight);
            SpawnPipes(height, gapSize, x_spawn);
        }
    }

    private void PipeMovement() {
        for (int i=0; i<pipeList.Count; i++) {
            Pipe pipe = pipeList[i];

            // Checo se o pipe esta à direita do bird, e se. após o movimento ele passou do bird, marcando um ponto
            bool isToTheRightOfBird = pipe.GetXPosition() > Bird_position;
            pipe.Move();
            if (isToTheRightOfBird && pipe.GetXPosition() <= Bird_position && pipe.IsBottom()) {
                // Pipe passed Bird
                score++;
                FindObjectOfType<AudioManager>().Play(AudioManager.Sounds.Score);
            }

            if (pipe.GetXPosition() < x_destroy) {
                // Destroy Pipe
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }

    public int GetScore()
    {
        return score;
    }

    private void SetDifficulty(Difficulty difficulty) {
        switch (difficulty) {
        case Difficulty.Easy:
            gapSize = 40f;
            TimeBetweenSpawns = 1.6f;
            break;
        case Difficulty.Medium:
            gapSize = 35f;
            TimeBetweenSpawns = 1.4f;
            break;
        case Difficulty.Hard:
            gapSize = 30f;
            TimeBetweenSpawns = 1.3f;
            break;
        case Difficulty.Impossible:
            gapSize = 25f;
            TimeBetweenSpawns = 1.3f;
            break;
        }
    }

    private Difficulty GetDifficulty() {
        if (pipesSpawned >= 30) return Difficulty.Impossible;
        if (pipesSpawned >= 15) return Difficulty.Hard;
        if (pipesSpawned >= 5) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    private void SpawnPipes(float gapY, float gapSize, float xPosition) {
        CreatePipe(gapY - gapSize / 2f, xPosition, true);
        CreatePipe(Camera_size * 2f - gapY - gapSize / 2f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
    }

    private void CreatePipe(float height, float xPosition, bool createBottom) {
        // Set up Pipe Body
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pipeBody);
        
        if (createBottom) {
            pipeBody.position = new Vector3(xPosition, -Camera_size);

        } else {
            pipeBody.position = new Vector3(xPosition, +Camera_size);
            Quaternion inverter = Quaternion.Euler(0, 180, 180);
            pipeBody.rotation = inverter;

        }

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(Pipe_width, height);

        BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider.size = new Vector2(Pipe_width, height);
        pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f);

        Pipe pipe = new Pipe(pipeBody, createBottom);
        pipeList.Add(pipe);
    }

    /*
     * Represents a single Pipe
     * */
    private class Pipe {

        private Transform pipeBodyTransform;
        private bool isBottom;

        public Pipe(Transform pipeBodyTransform, bool isBottom) {
            this.pipeBodyTransform = pipeBodyTransform;
            this.isBottom = isBottom;
        }

        public void Move() {
            pipeBodyTransform.position += new Vector3(-1, 0, 0) * Move_speed * Time.deltaTime;
        }

        public float GetXPosition() {
            return pipeBodyTransform.position.x;
        }

        public bool IsBottom() {
            return isBottom;
        }

        public void DestroySelf() {
            Destroy(pipeBodyTransform.gameObject);
        }

    }

}

