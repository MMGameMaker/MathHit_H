using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthTextPop : MonoBehaviour
{
    public float moveSpeed;

    public AnimationClip popUpAnimation;

    [SerializeField]
    private TextMesh healthText;

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

    public void SetHealthText(int damage)
    {
        healthText.text = damage.ToString();
    }

    public IEnumerator TextPopUp()
    {
        Animator animator = GetComponent<Animator>();

        yield return new WaitForSeconds(0.1f);

        if (animator)
        {
            animator.Play(popUpAnimation.name);
        }
        yield return new WaitForSeconds(0.1f);

        Destroy(this.gameObject);
    }
}
