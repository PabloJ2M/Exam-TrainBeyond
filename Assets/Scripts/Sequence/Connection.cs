using UnityEngine;

public class Connection : MonoBehaviour
{
    [SerializeReference] private Point[] points;
    private int _nextExpectedIndex;

    private void Start()
    {
        ConnectPoints();
        SubscribeMethods();
    }
    private void OnDestroy() => UnSubscribeMethods();

    [ContextMenu("Connect")]
    public void ConnectPoints()
    {
        points = GetComponentsInChildren<Point>();
        
        for (int i = 0; i < points.Length; i++)
            points[i].Setup(i);
    }

    private void SubscribeMethods()
    {
        if (points == null) return;
        
        foreach (var point in points)
            point.OnActivated += HandlePointActivated;
    }
    private void UnSubscribeMethods()
    {
        if (points == null) return;
        
        foreach (var point in points)
            point.OnActivated -= HandlePointActivated;
    }
    
    private bool HandlePointActivated(int index)
    {
        if (index != _nextExpectedIndex) {
            ResetSequence();
            return false;
        }
        
        _nextExpectedIndex++;

        if (_nextExpectedIndex >= points.Length)
            OnSequenceComplete();

        return true;
    }
    
    private void OnSequenceComplete()
    {
        Debug.Log("Secuencia completa!");
    }
    private void ResetSequence()
    {
        _nextExpectedIndex = 0;
        foreach (var point in points)
            point.Deactivate();
    }
}