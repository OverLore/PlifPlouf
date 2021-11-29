using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinePath : MonoBehaviour
{
    public Transform[] controlPoints;

    public Color startColor = Color.red;
    public Color endColor = Color.green;

    public float strength = 3;

    public float gizmoSize = .25f;

    Vector2 guizmosPosition;

    public Vector2 GetSplinePoint(float i)
    {
       return Mathf.Pow(1 - i, 3) * controlPoints[0].position +
                strength * Mathf.Pow(1 - i, 2) * i * controlPoints[1].position +
                strength * (1 - i) * Mathf.Pow(i, 2) * controlPoints[2].position +
                Mathf.Pow(i, 3) * controlPoints[3].position;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            return;
        }

        for (float i = 0; i <= 1; i += 0.05f)
        {
            guizmosPosition = GetSplinePoint(i);

            Gizmos.color = Color.Lerp(startColor, endColor, i);

            Gizmos.DrawSphere(guizmosPosition, gizmoSize);

            Gizmos.DrawLine(new Vector2(controlPoints[0].position.x, controlPoints[0].position.y),
                new Vector2(controlPoints[1].position.x, controlPoints[1].position.y));

            Gizmos.DrawLine(new Vector2(controlPoints[2].position.x, controlPoints[2].position.y),
                new Vector2(controlPoints[3].position.x, controlPoints[3].position.y));
        }

        Gizmos.color = Color.white;
    }
}
