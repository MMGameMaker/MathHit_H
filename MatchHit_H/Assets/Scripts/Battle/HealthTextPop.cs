using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTextPop : MonoBehaviour
{
    public float moveSpeed;

    public AnimationClip popUpAnimation;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TextPopUp());
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveUp();
    }

    private void MoveUp()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
    }

    public IEnumerator TextPopUp()
    {
        Animator animator = GetComponent<Animator>();

        yield return new WaitForSeconds(0.1f);

        if (animator)
        {
            animator.Play(popUpAnimation.name);

            yield return new WaitForSeconds(popUpAnimation.length);

            Destroy(this.gameObject);
        }

    }



}
