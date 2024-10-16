using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform targetObj;
    void Start()
    {
        
    }

void Update()
{
    Vector3 direction = (targetObj.position - transform.position).normalized;
    
    float distanceToMove = 3 * Time.deltaTime;

    RaycastHit hit;
    if (Physics.Raycast(transform.position, direction, out hit, distanceToMove))
    {
        if (hit.collider.CompareTag("Platform"))
        {
            return;
        }
    }

    transform.position = Vector3.MoveTowards(this.transform.position, targetObj.position, distanceToMove);
}


}
