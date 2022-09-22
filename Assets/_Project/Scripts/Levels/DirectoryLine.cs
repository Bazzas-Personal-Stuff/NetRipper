using UnityEngine;

public class DirectoryLine : MonoBehaviour {
    public Directory a;
    public Directory b;
    
    public bool wasALastLowest;
    
    public LineRenderer lineRenderer;
    public float alphaFactor = 0.35f;
    public float bufferDistance = 1.56f / 2f;

    public void Setup() {
        lineRenderer.startColor = a.invisColor;
        lineRenderer.endColor = a.invisColor;

        Vector3 startPoint = a.transform.position + (b.transform.position - a.transform.position).normalized * bufferDistance;
        startPoint += Vector3.forward;
        Vector3 endPoint = b.transform.position + (a.transform.position - b.transform.position).normalized * bufferDistance;
        endPoint += Vector3.forward;

        lineRenderer.SetPositions(new []{startPoint, endPoint});
    }


    public void Update() {

        if (a.visitState != b.visitState) {
            wasALastLowest = (int)a.visitState < (int)b.visitState ;
        }
        Directory lowestState = wasALastLowest ? a : b;
        
        Color thisColor = lowestState.currentColor;
        thisColor.a = Mathf.Max(thisColor.a * alphaFactor, 0);
        lineRenderer.startColor = thisColor;
        lineRenderer.endColor = thisColor;

    }
    
}