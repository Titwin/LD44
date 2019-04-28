using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedObject : MonoBehaviour
{
    [SerializeField] bool active = false;
    [SerializeField] float transitionTime = 1;
    [SerializeField] Vector3 offPosition = Vector3.zero;
    [SerializeField] Vector3 onPosition = Vector3.zero;

    private void Start()
    {
        Vector3 targetPos = active ? onPosition : offPosition;
        this.transform.localPosition = targetPos;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.parent.TransformPoint(offPosition), this.transform.lossyScale);
        Gizmos.DrawWireCube(this.transform.parent.TransformPoint(onPosition), this.transform.lossyScale);
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
