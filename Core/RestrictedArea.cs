using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictedArea : MonoBehaviour
{
    [SerializeField] float radius=10f;

    private Transform trans;
    public Vector3 pos=Vector3.zero;
    public Vector3 normal=new Vector3(0,1,0);

    // Start is called before the first frame update
    void Awake()
    {
        trans=GetComponent<Transform>();
    }

    public void DataUpdate()
    {
        trans.position=pos;
    }
}
