using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
    public GameObject targetHook;
    public Vector2 normalVector;
    private Vector2 targetPosition;


    [SerializeField] float rocketSpeed = 1;

    private Satellite hookProperties;
    private float hookAngSpeed;
    private float hookRad;
    public float trueDistance;

    [SerializeField] GameObject debugPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LateStart());

        

        Destroy(gameObject, 10);
    }

    IEnumerator LateStart()
    {
        yield return null;

        hookProperties = targetHook.GetComponent<Satellite>();


        hookRad = hookProperties.orbitRadius;
        hookAngSpeed = hookProperties.orbitVelocity / hookRad;

        targetPosition = (normalVector * hookRad);
        trueDistance = (hookRad - ((transform.position).magnitude));
       
        StartCoroutine(LaunchSequence());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float IdealAngle()
    {
        return ((trueDistance * hookAngSpeed) / (rocketSpeed));

        
    }

    Vector2 IdealPosition()
    {
        //in radians
        float targetAngle = Mathf.Atan2(targetPosition.y, targetPosition.x);
       // Debug.Log(targetAngle * Mathf.Rad2Deg);
        //turn backwards
        float totalAngle = (targetAngle - IdealAngle());


        Vector2 idealPos = new Vector2(hookRad * Mathf.Cos(totalAngle), hookRad * Mathf.Sin(totalAngle));
        
        
        
        GameObject debugObj = GameObject.Instantiate(debugPrefab, idealPos, Quaternion.identity);
        Destroy(debugObj, 4f);



        return idealPos;
    }

    float TimeToIdealPosition()
    {

        //distance between current hook pos and ideal pos
        Vector2 idealPosition = (IdealPosition());
        Vector2 distance = (idealPosition - (Vector2)targetHook.transform.position);
        

        //use law of cosines to figure out time to ideal position
        float lawCosValue = (Square(distance.magnitude) - (2 * Square(hookRad))) / (-2 * Square(hookRad));
        float angleLawCos = Mathf.Acos(lawCosValue);

       // GameObject debugObj = GameObject.Instantiate(debugPrefab, targetHook.transform.position, Quaternion.identity);
       // Destroy(debugObj, 4f);

        //if the angle of the mirrored point is greater than the angle of the original point, the point is behind the object

        Vector2 mDistance = (MirrorPoint(idealPosition) - (Vector2)targetHook.transform.position);
        float mirroredAngleCheck = Mathf.Acos((Square(mDistance.magnitude) - (2 * Square(hookRad))) / (-2 * Square(hookRad)));
        if (mirroredAngleCheck > angleLawCos)
        {
            angleLawCos = ((Mathf.PI * 2) - angleLawCos);
            Debug.Log("MEASURED ANGLE WAS BEHIND HOOK, REMEASURED");
        }




      //  Debug.Log("centerAngle opposing Chord CI: " + angleLawCos * Mathf.Rad2Deg);
        float timeToIdeal = (angleLawCos / hookAngSpeed);
        return timeToIdeal;

    }

    float Square(float f)
    {
        return Mathf.Pow(f, 2);
        
    }

    IEnumerator LaunchSequence()
    {

        yield return new WaitForSeconds(TimeToIdealPosition());

        
        float duration = (hookRad / rocketSpeed);
        Vector2 initPos = transform.position;

        //calc targetPos

        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            float normalizedTime = i / duration;
            yield return null;
            Vector2 lerpPos = Vector2.Lerp((Vector2.zero), targetPosition, normalizedTime);
            transform.position = lerpPos;

            

        }

        transform.position = targetPosition;

    }

    Vector2 MirrorPoint(Vector2 point, float radius = 1)
    {
        Vector2 norm = point.normalized;
        float angle = Mathf.Atan2(norm.y, norm.x);
        float mirroredAngle = angle + Mathf.PI;

        Vector2 mirroredPoint = new Vector2(Mathf.Cos(mirroredAngle) * radius, Mathf.Sin(mirroredAngle) * radius);
        return mirroredPoint;



    }

}
