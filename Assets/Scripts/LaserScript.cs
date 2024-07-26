using System.Collections;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float onTime = 3.0f; // Time the laser stays on
    public float offTime = 3.0f; // Time the laser stays off
    private Renderer laserRenderer; 

    void Start()
    {
        laserRenderer = GetComponent<Renderer>();
        StartCoroutine(LaserRoutine());
    }

    IEnumerator LaserRoutine()
    {
        while (true)
        {
            laserRenderer.enabled = true;
            yield return new WaitForSeconds(onTime);

            laserRenderer.enabled = false;
            yield return new WaitForSeconds(offTime);
        }
    }
}
