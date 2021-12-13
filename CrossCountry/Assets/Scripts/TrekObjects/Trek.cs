using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using static System.Math;
using UnityEngine;
using static StaticTrekData;
using UnityEngine.Events;
using System.Text;

public class Trek : MonoBehaviour
{
    [SerializeField]
    GameObject grassExample;
    [SerializeField]
    GameObject fenceExample;
    [SerializeField]
    GameObject obstacleExample;
    List<Vector3> grassPatches = new List<Vector3>();
    Dictionary<int, GameObject> obstacles = new Dictionary<int, GameObject>();
    ObstacleSuccessEvent obstacleSuccessEvent = new ObstacleSuccessEvent();
    TrekCreatedEvent trekCreatedEvent = new TrekCreatedEvent();
    FinishEvent finishEvent = new FinishEvent();
    int obstacleNum = 5;
    float size = 750f;
    float tileSize = 250;
    float hedgeHalfHeight = 25;
    TrekIO trekIO;
    List<string> obstacleLinesToSave = new List<string>();

    public void AddTrekCreatedEventListener(UnityAction listener)
    {
        trekCreatedEvent.AddListener(listener);
    }


    void CreateGround()
    {
        GameObject ground = new GameObject("Ground");
        for (float i=-size; i<=size; i += tileSize)
        {
            for(float j=-size; j<=size; j += tileSize)
            {
                Vector3 position = new Vector3(i, 0, j);
                GameObject grass = Instantiate(grassExample, position, Quaternion.identity);
                grass.transform.parent = ground.transform;
                if (j != -size && j != size && i != -size && i != size)
                {
                    grassPatches.Add(position);
                }                
            }            
        }
        grassPatches = grassPatches.OrderBy(x => Random.value).ToList();
    }

    void CreateFences()
    {
        GameObject fences = new GameObject("Fences");
        for(float i = -size; i <= size; i += tileSize)
        {
            GameObject hedge1 = Instantiate(fenceExample, new Vector3(i, hedgeHalfHeight, size+tileSize/2), Quaternion.identity);
            hedge1.transform.parent = fences.transform;
            GameObject hedge2 = Instantiate(fenceExample, new Vector3(i, hedgeHalfHeight, -size-tileSize / 2), Quaternion.identity);
            hedge2.transform.parent = fences.transform;
            GameObject hedge3 = Instantiate(fenceExample, new Vector3(size+tileSize / 2, hedgeHalfHeight, i), Quaternion.identity);
            hedge3.transform.Rotate(new Vector3(0, 90, 0));
            hedge3.transform.parent = fences.transform;
            GameObject hedge4 = Instantiate(fenceExample, new Vector3(-size-tileSize / 2, hedgeHalfHeight, i), Quaternion.identity);
            hedge4.transform.Rotate(new Vector3(0, 90, 0));
            hedge4.transform.parent = fences.transform;
        }
    }

    
    
    void CreateObstacles()
    {
        obstacleNum = Min(obstacleNum, grassPatches.Count);        
        Dictionary<int, System.Tuple<float, string>> heights = trekIO.WriteObstaclesDoc(obstacleNum);
        List<int> obstacleList = new List<int>();
        for (int i = 1; i < obstacleNum+1; i++)
        {
            obstacleList.Add(i);
        }
        obstacleList = obstacleList.OrderBy(x => Random.value).ToList(); ;
        for (int i = 0; i< obstacleNum; i++)
        {
            Vector3 position = new Vector3(grassPatches[i].x+Random.Range(-tileSize/2,tileSize/2), 
                grassPatches[i].y+heights[obstacleList[i]].Item1 /2, 
                grassPatches[i].z+Random.Range(-tileSize / 2, tileSize / 2));
            GameObject obstacle = Instantiate(obstacleExample, position, Quaternion.identity);
            float rotationAngle = Random.Range(0, 90);
            obstacle.transform.Rotate(new Vector3(0,rotationAngle,0));
            Obstacle script = obstacle.GetComponent<Obstacle>();
            script.Id = obstacleList[i];
            script.PostStart();
            obstacles.Add(script.Id, obstacle);
            obstacleLinesToSave.Add(script.Id.ToString() + "," +
                position.x.ToString("0.00", CultureInfo.InvariantCulture) + "," +
                position.y.ToString("0.00", CultureInfo.InvariantCulture) + "," +
                position.z.ToString("0.00", CultureInfo.InvariantCulture) + "," +
                rotationAngle.ToString("0.00", CultureInfo.InvariantCulture) + "," +
                heights[obstacleList[i]].Item2);
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material.color = color;
        lr.SetWidth(5f, 5f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        Renderer renderer = myLine.transform.GetComponent<Renderer>();
        renderer.receiveShadows = false;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    float CreateRoute()
    {
        float COEFF = 250;
        float distance = 0;
        Vector3 beginning = StaticTrekData.Start;
        for (int i = 1; i<=obstacleNum; i++)
        {
            GameObject obstacle = obstacles[i];
            Vector3 center = obstacle.transform.position;
            Vector3 forward = obstacle.transform.right;
            Vector3 start = new Vector3(center.x+ COEFF * forward.x, beginning.y, center.z + COEFF * forward.z);
            Vector3 end = new Vector3(center.x - COEFF * forward.x, beginning.y, center.z - COEFF * forward.z);
            if (Vector3.Distance(beginning, start) > Vector3.Distance(beginning, end))
            {
                Vector3 swap = start;
                start = end;
                end = swap;
            }
            Color color = new Color(1, center.y - Mathf.Floor(center.y), center.z - Mathf.Floor(center.z), 1);
            DrawLine(start, end, color);
            DrawLine(beginning, start, color);
            obstacles[i].GetComponent<Obstacle>().Colour = color;
            distance += Vector3.Distance(start,end) + Vector3.Distance(beginning, start);
            beginning = end;
            if (i == 1)
            {
                StaticTrekData.DirectHorseTo = new Vector3(start.x, StaticTrekData.HorseHeight, start.z);
            }
        }
        return distance;
    }

    void RestoreOldTrek()
    {
        try
        {
            List<string> data = trekIO.ReadOldTrekData();
            obstacleNum = data.Count;
            trekIO.ReWriteObstaclesDoc(data);
            for (int i = 0; i < data.Count; i++)
            {
                string[] lineData = data[i].Split(',');
                Vector3 position = new Vector3((float)System.Convert.ToDouble(lineData[1], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), 
                    (float)System.Convert.ToDouble(lineData[2], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), 
                    (float)System.Convert.ToDouble(lineData[3], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")));
                GameObject obstacle = Instantiate(obstacleExample, position, Quaternion.identity);
                obstacle.transform.Rotate(new Vector3(0, (float)System.Convert.ToDouble(lineData[4], System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), 0));
                Obstacle script = obstacle.GetComponent<Obstacle>();
                script.Id = (int)System.Convert.ToDouble(lineData[0]);
                script.PostStart();
                obstacles.Add(script.Id, obstacle);
            }
        }
        catch (System.Exception e) {
            Debug.Log(e.Message);
        }
    }
        
    void BuildTrek()
    {
        CreateGround();
        CreateFences();
        if (!StaticTrekData.IsLoaded)
        {
            CreateObstacles();
        }
        else
        {
            RestoreOldTrek();
        }
        float distance = CreateRoute();
        float limitTime = distance / 35;
        TrekTiming trekTiming = gameObject.GetComponent<TrekTiming>();
        trekTiming.LimitTime = limitTime;
        trekTiming.Run();
        StaticTrekData.ExpectedTime = Utils.GetFormattedTime((double)limitTime);
        StaticTrekData.Finished = false;
        StaticTrekData.Stopped = false;
        StaticTrekData.TimeScore = "00:00:00:00";
        StaticTrekData.PenaltyScore = 0;
        StaticTrekData.ObstacleScore = 0;
        StaticTrekData.NextObstacleId = 1;
        trekCreatedEvent.Invoke();
    }

    

    public void AddObstacleSuccessEventListener(UnityAction<int> listener)
    {
        obstacleSuccessEvent.AddListener(listener);
    }
    public void AddFinishEventListener(UnityAction listener)
    {
        finishEvent.AddListener(listener);
    }

    public void CheckOverjumpedObstacle(int id)
    {
        if (id == StaticTrekData.NextObstacleId)
        {
            obstacleSuccessEvent.Invoke(id);
            if (id == obstacleNum && !StaticTrekData.Stopped)
            {
                StaticTrekData.Finished = true;
                StopGame();
                finishEvent.Invoke();
            }
            else
            {
                StaticTrekData.NextObstacleId = StaticTrekData.NextObstacleId + 1;
            }           
        }
    }
    public void StopGame()
    {
        StaticTrekData.Stopped = true;
        trekIO.SaveTrekResults();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        trekIO = new TrekIO(Application.dataPath);
        EventManager.AddTrekCreatedInvoker(this);
        EventManager.AddOverjumpedObstacleListener(CheckOverjumpedObstacle);
        EventManager.AddObstacleSuccessInvoker(this);
        EventManager.AddFinishInvoker(this);

        EventManager.AddTimeRanOutListener(StopGame);
        EventManager.AddHorseExaustedListener(StopGame);
        EventManager.AddRefusalEliminationListener(StopGame);
        BuildTrek();
        trekIO.SaveTrek(obstacleLinesToSave);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
