using Photon.Pun;
using UnityEngine;

public class BallView : MonoBehaviour, IBallView, IPunObservable
{
    [SerializeField] private Transform _startingPoint;
    [SerializeField] private PlayerPresenter _myPlayer;
    [SerializeField] private float _timerDuration;
    [SerializeField] private float _ballSpeed;

    private Timer _timer;

    public MeshRenderer MeshRenderer { get => gameObject.GetComponent<MeshRenderer>(); }
    public Collider Collider { get => gameObject.GetComponent<Collider>(); }
    public Timer Timer { get => _timer; }
    public Transform StartingPoint { get => _startingPoint; }
    public PlayerPresenter MyPlayer { get => _myPlayer; }
    public bool IsThrown { get; set; }
    public GameObject This { get => gameObject; }
    public float BallSpeed { get => _ballSpeed; set => _ballSpeed = value; }

    private void OnEnable()
    {
        SetTimer();
        GameEntryPoint.Instance.OnUpdateEvent += MoveBall;
    }

    private void OnDisable()
    {
        _timer.Dispose();
        GameEntryPoint.Instance.OnUpdateEvent -= MoveBall;
    }

    private void SetTimer()
    {
        _timer = new(_timerDuration, MyPlayer.ShutDownPlayer);
    }

    private void MoveBall()
    {
        if (!IsThrown) transform.position = StartingPoint.position;
        else BallModel.MoveBall(this, transform.forward);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) stream.SendNext(IsThrown);
        else IsThrown = (bool)stream.ReceiveNext();
    }
}
