using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;
using static MapGenerator;

public class PlayerController : MonoBehaviour
{
    public Animator _animatorMouse;
    public Animator _animatorFox;
    public Animator _animatorFish;

    [SerializeField] private AudioSource _foxJumpSound;
    [SerializeField] private AudioSource _fishJumpSound;
    [SerializeField] private AudioSource _foxIdleSound;
    [SerializeField] private AudioSource _fishIdleSoynd;
    [SerializeField] private AudioSource _gameOver;

    [SerializeField] private AudioSource _jumpMouseSound;
    [SerializeField] private AudioSource _stepMouseSound;

    [SerializeField]private float _laneOffset = 2f;
    [SerializeField] private GameObject _endGameWindow;
    //private Vector3 _targetPos;
    private float _laneChangeSpeed = 10;

    private Rigidbody _rb;
    private Vector3 _targerVelocity;

    [SerializeField] private GameObject _deathWindow;

    private bool _isJumping = false;
    private float _jumpPower = 10f;
    private float _jumpGravity = -40f;
    private float _realGravity = -9.8f;

    [SerializeField] private GameObject _mousePlayer;
    [SerializeField] private GameObject _monkeyPlayer;
    [SerializeField] private GameObject _fishPlayer;

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

      
    }
    static public PlayerController instanse;

    private void Update()
    {
        if (RoadGenerator.instanse.Speed != 0)
        {

            if (Input.GetKeyDown(KeyCode.A) && _pointFinish > -_laneOffset)
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
            _animatorMouse.SetTrigger("Die");
            _gameOver.Play();
            Score.instance.StopScore();
            _deathWindow.SetActive(true);
            RoadGenerator.instanse.ResetLevel();
        }

        
    }
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Beast");
        if (other.gameObject.GetComponent<FoxControl>())
        {
            _jumpPower = 12f;
            _animatorMouse = _animatorFox;
            // MapGenerator.instanse.SetBeast(BeastPool.Fox);
            _mousePlayer.SetActive(false);
            _monkeyPlayer.SetActive(true);
            _fishPlayer.SetActive(false);
            Destroy(other.gameObject);
            Debug.Log("Beast1");
            AnimationController(BeastPool.Fox);
        }

        if (other.gameObject.GetComponent<MouseController>()) 
        {
            _jumpPower = 15f;
          //  MapGenerator.instanse.SetBeast(BeastPool.Mouse);
            _mousePlayer.SetActive(true);
            _monkeyPlayer.SetActive(false);
            _fishPlayer.SetActive(false);
            Destroy(other.gameObject);
            Debug.Log("Beast2");
        }

        if (other.gameObject.GetComponent<FishController>())
        {
            _animatorMouse = _animatorFish;
            _animatorFish.SetTrigger("StartGame");
            AnimationController(BeastPool.Fish);
            _jumpPower = 12f;
            //MapGenerator.instanse.SetBeast(BeastPool.Fish);
            RoadGenerator.instanse.SetNextRoud(BeastPool.Fish);
            _mousePlayer.SetActive(false);
            _monkeyPlayer.SetActive(false);
            _fishPlayer.SetActive(true);
            Destroy(other.gameObject);
            Debug.Log("Beast2");
            
        }

        if (other.gameObject.tag == "EndGame")
        {
            _rb.AddForce(Vector3.up * 1000f, ForceMode.Impulse);
            _rb.useGravity = false;
            _endGameWindow.SetActive(true);
            Score.instance.StopScore();
        }
    }
    private void Jump()
    {
        _isJumping = true;
        _jumpMouseSound.Play();
        _animatorMouse.SetTrigger("Jump");
        _animatorFox.SetTrigger("Jump");
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
        _animatorMouse.SetTrigger("Jump");
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
            transform.position = new Vector3(x, transform.position.y, 0f);

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

    public void StepSoundPlay() { _stepMouseSound.Play(); Debug.Log("StepSound"); }

    public void AnimationController(BeastPool nowBeast)
    {
        if (nowBeast == BeastPool.Mouse)
            _animatorMouse.SetTrigger("StartGame");

        else if (nowBeast == BeastPool.Fox)
            _animatorFox.SetTrigger("StartGame");

        else if (nowBeast == BeastPool.Fish)
            _animatorFish.SetTrigger("StartGame");
    }
}
