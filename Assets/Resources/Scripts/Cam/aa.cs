using UnityEngine;

public class aa : MonoBehaviour
{
    public Camera cam;
    public PlayerController A;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        cam.transform.position = new Vector3(A.transform.position.x, cam.transform.position.y, cam.transform.position.z);
    }




}
