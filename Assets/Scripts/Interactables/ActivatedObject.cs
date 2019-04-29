using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedObject : MonoBehaviour
{
    [SerializeField] bool active = false;
    [SerializeField] float transitionTime = 1;
    [SerializeField] Vector3 offPosition = Vector3.zero;
    [SerializeField] Vector3 onPosition = Vector3.zero;

    [SerializeField] Vector3 size = new Vector3(1, 3, 1);
    private void Start()
    {
        Vector3 targetPos = active ? onPosition : offPosition;
        this.transform.localPosition = targetPos;
    }
    private void OnDrawGizmos()
    {
        Vector3 wOff = this.transform.parent.TransformPoint(offPosition);
        Vector3 wOn = this.transform.parent.TransformPoint(onPosition);
        Vector3 wSize = transform.TransformVector(size);

        Gizmos.color = Color.white;
        //Path
        Gizmos.DrawLine(wOff, wOn);
        //Bounding box
        Gizmos.DrawWireCube((wOff+wOn)/2, wSize + new Vector3(Mathf.Abs(wOff.x - wOn.x), Mathf.Abs(wOff.y - wOn.y), Mathf.Abs(wOff.z - wOn.z))+Vector3.one*0.1f);

        // Off location
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wOff, wSize);
        Gizmos.DrawSphere(wOff, 0.25f);
        Gizmos.color = Color.green;
        // On location
        Gizmos.DrawWireCube(wOn, wSize);
        Gizmos.DrawSphere(wOn, 0.25f);

    }

    private void Update()
    {
        Vector3 targetPos = active ? onPosition : offPosition;
        float range = Vector3.Distance(onPosition, offPosition);
        if (this.transform.localPosition != targetPos)
        {
            if (transitionTime == 0)
            {
                this.transform.localPosition = targetPos;
            }
            else
            {
                this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, targetPos, range * Time.deltaTime / transitionTime);
            }
        }
    }

    public void SetActive(bool active)
    {
        this.active = active;
    }
}
