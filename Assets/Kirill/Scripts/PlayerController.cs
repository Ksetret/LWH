using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using static MapGenerator;

public class PlayerController : MonoBehaviour
{
    public Animator Animator;
    [SerializeField]private float _laneOffset = 2f;
    //private Vector3 _targetPos;
    private float _laneChangeSpeed = 10;

    private Rigidbody _rb;
    private Vector3 _targerVelocity;

    [SerializeField] private GameObject _deathWindow;

    private bool _isJumping = false;
    private float _jumpPower = 15f;
    private float _jumpGravity = -40f;
    private float _realGravity = -9.8f;

    [SerializeField] private GameObject _mousePlayer;
    [SerializeField] private GameObject _monkeyPlayer;

    private float _pointStart;
    private float _pointFinish;

    private bool _isMoving = false;
    Coroutine _movingCorotine;

    private void Awake()
    {
        instanse = this;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
      
    }
    static public PlayerController instanse;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && _pointFinish  > -_laneOffset)
        {
            MoveHorizontal(-_laneChangeSpeed);
            
        }

        if (Input.GetKeyDown(KeyCode.D) && _pointFinish < _laneOffset)
        {

            MoveHorizontal(_laneChangeSpeed);
            
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isJumping == false)
        {
            Jump();
        }


        //  transform.position = Vector3.MoveTowards(transform.position, _targetPos, _laneChangeSpeed * Time.deltaTime);
    }

   /* private void FixedUpdate()
    {
        _rb.velocity = _targerVelocity;
        if((transform.position.x  > _pointFinish && _targerVelocity.x > 0) || (transform.position.x < _pointFinish && _targerVelocity.x < 0))
        {
            _targerVelocity = Vector3.zero;
            _rb.velocity = _targerVelocity;
            _rb.position = new Vector3(_pointFinish, _rb.position.y, _rb.position.z);
        }
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Dead");
        if (collision.gameObject.tag == "Lose")
        {
            Score.instance.StopScore();
            _deathWindow.SetActive(true);
            RoadGenerator.instanse.ResetLevel();
        }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Beast");
        if (other.gameObject.GetComponent<MonkeyControl>())
        {
            MapGenerator.instanse.SetBest(BeastPool.Monkey);
            _mousePlayer.SetActive(false);
            _monkeyPlayer.SetActive(true);
            Destroy(other.gameObject);
            Debug.Log("Beast1");
        }

        if (other.gameObject.GetComponent<MouseController>()) 
        {
            MapGenerator.instanse.SetBest(BeastPool.Mouse);
            _mousePlayer.SetActive(true);
            _monkeyPlayer.SetActive(false);
            Destroy(other.gameObject);
            Debug.Log("Beast2");
        }
    }
    private void Jump()
    {
        _isJumping = true;

        _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        Physics.gravity = new Vector3(0, _jumpGravity, 0);

        StartCoroutine(StopjumpCorotine());
    }
    IEnumerator StopjumpCorotine()
    {
        do
        {
            yield return new WaitForFixedUpdate();
        }
        while (_rb.velocity.y != 0);
        _isJumping = false;
        Physics.gravity = new Vector3(0, _realGravity, 0);

    }
    void MoveHorizontal(float speed)
    {
        _pointStart = _pointFinish;
        _pointFinish += Mathf.Sign(speed) * _laneOffset;


        if(_isMoving) 
        { 
            StopCoroutine(_movingCorotine); 
            _isMoving = false; 
        }
      _movingCorotine = StartCoroutine(MoveCoroutine(speed));
      
    }
    IEnumerator MoveCoroutine(float vectorX)
    {
        _isMoving = true;

        while (Mathf.Abs(_pointStart - transform.position.x) < _laneOffset)
        {
            yield return new WaitForFixedUpdate();

            _rb.velocity = new Vector3(vectorX, _rb.velocity.y, 0);
            float x = Mathf.Clamp(transform.position.x, Mathf.Min(_pointStart, _pointFinish), Mathf.Max(_pointStart, _pointFinish));
            transform.position = new Vector3(x, transform.position.y, transform.position.z);

        }

        _rb.velocity = Vector3.zero;
        transform.position = new Vector3(_pointFinish, transform.position.y, transform.position.z);
        _isMoving = false;
    }

    public void RestartBeast()
    {
       
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }

        this.transform.GetChild(0).gameObject.SetActive(true);
    }
}
