using Code.input;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(IInput))]
[RequireComponent(typeof(CubeMoving))]
public class Cube : MonoBehaviour
{
    private IInput input;
    private CubeMoving moving;
    internal CubeContent content;
    private BoxCollider collider;
    private Rigidbody rb;

    internal bool is_player
    {
        get { return tag == "Player"; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<IInput>();
        moving = GetComponent<CubeMoving>();
        content = GetComponent<CubeContent>();

        collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = is_player;
        collider.size = Vector3.one * content.width;   
        
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.angularDrag = 0;

        input.step_size = content.width;
        
        if(is_player)
            content.GenerateColored(percent: 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if(input.axis != Vector3.zero)
            moving.Rotate(input.axis, content.width);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(is_player && other.gameObject.GetComponent<Cube>() != null) 
            World.Impact(this, other.gameObject.GetComponent<Cube>());
    }

    IEnumerator DestroyCube()
    {
        while (transform.localScale.x > 0)
        {
            Vector3 temp = transform.localScale;
            temp.x = temp.y = temp.z = temp.x * 0.95f;
            transform.localScale = temp;
            yield return null;
        }
        Destroy(gameObject);
    }

    public void Kill()
    {
        StartCoroutine(DestroyCube());
    }
}
