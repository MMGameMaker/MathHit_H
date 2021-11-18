using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearablePiece : MonoBehaviour
{
    private bool isBeingCleared = false;

    public bool IsBeingCleared
    {
        get { return isBeingCleared; }
    }

    [SerializeField]
    private float timeToClear;

    public float TimeToClear { get => timeToClear; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clear()
    {
        isBeingCleared = true;
        StartCoroutine(ClearCoroutine());
    }

    private IEnumerator ClearCoroutine()
    {
        StartCoroutine("MoveToClearPosition");
        yield return new WaitForSeconds(timeToClear);
        Destroy(this.gameObject);
    }

    private IEnumerator MoveToClearPosition()
    {
        Vector2 starPos = transform.position;
        Vector2 entPos = new Vector2(-1.5F, 3.5F);

        for (float t = 0; t < 1 * timeToClear; t += Time.deltaTime)
        {
            this.transform.position = Vector2.Lerp(starPos, entPos, t / timeToClear);
            yield return 0;
        }

        this.transform.position = entPos;
    }

}
