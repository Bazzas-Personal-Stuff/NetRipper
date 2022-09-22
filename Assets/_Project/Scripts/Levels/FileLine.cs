using UnityEngine;

public class FileLine : MonoBehaviour {
    public Directory dir;
    public File file;
    
    public LineRenderer lineRenderer;
    public float alphaFactor = 0.45f;
    
    public float bufferDirDistance = 1.56f / 2f;
    public float bufferFileDistance = 1f;

    public void Setup() {
        lineRenderer.startColor = file.invisColor;
        lineRenderer.endColor = file.invisColor;

        Vector3 startPoint = dir.transform.position + (file.transform.position - dir.transform.position).normalized * bufferDirDistance;
        startPoint += Vector3.forward;
        Vector3 endPoint = file.transform.position + (dir.transform.position - file.transform.position).normalized * bufferFileDistance;
        endPoint += Vector3.forward;

        lineRenderer.SetPositions(new []{startPoint, endPoint});
    }


    public void Update() {
        
        Color thisColor = file.currentColor;
        thisColor.a = Mathf.Max(thisColor.a * alphaFactor, 0);
        lineRenderer.startColor = thisColor;
        lineRenderer.endColor = thisColor;

    }
    
}