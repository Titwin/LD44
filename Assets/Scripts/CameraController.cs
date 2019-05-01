using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player target;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 softEdge;
    [SerializeField] Vector2 hardEdge;
    [SerializeField] Vector2 clipEdge;
    [SerializeField] Vector2 predictionFactor;
    // Start is called before the first frame update

    void Awake()
    {
        SetTarget(target);
    }
    public void SetTarget(Player target)
    {
        this.target = target;
        if (target)
        {
            this.transform.position = target.transform.position + offset;
        }
    }
    Vector3 targetPos;
    // Update is called once per frame
    void LateUpdate()
    {
        if (target)
        {
            Vector2 prediction = target.Controller.rb2D.velocity;
            prediction.Scale(predictionFactor);
            targetPos = (Vector2)(target.transform.position) + prediction;
            targetPos += target.transform.right;
            targetPos += offset;
            if (Mathf.Abs(this.transform.position.x - targetPos.x) > clipEdge.x
             || Mathf.Abs(this.transform.position.y - targetPos.y) > clipEdge.y)
            {
                this.transform.position = target.transform.position+ offset;
            }
            else if (Mathf.Abs(this.transform.position.x- targetPos.x) > hardEdge.x
                 || Mathf.Abs(this.transform.position.y - targetPos.y) > hardEdge.y)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos , 1f);
            }
            else if (Mathf.Abs(this.transform.position.x - targetPos.x) > softEdge.x
                  || Mathf.Abs(this.transform.position.y - targetPos.y) > softEdge.y)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, 0.25f);
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        if (target)
        {
            Vector3 currentPos = this.transform.position;
            currentPos.z = 0;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(targetPos, softEdge);
            Gizmos.DrawWireCube(currentPos, softEdge);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(targetPos, hardEdge);
            Gizmos.DrawWireCube(currentPos, hardEdge);
        }
    }
}
