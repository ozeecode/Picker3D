using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
namespace Picker3D
{
    public class Gate : MonoBehaviour
    {
        public int GateIndex { get; set; }
        public bool IsGateOpened => collectedAmount >= RequireAmount;
        public int RequireAmount
        {
            get => requireAmount;
            set
            {
                requireAmount = value;
                AmountTextUpdate();
            }
        }

        [SerializeField] private GateEventChannelSO gateEventChannel;
        [SerializeField] private VoidEventChannelSO gameFailedEventChannel;
        [SerializeField] private TMP_Text amountText;
        [SerializeField] private Transform[] barricades;
        [SerializeField] private Transform ground;


        private int requireAmount;
        private int collectedAmount;
        private WaitForSeconds waitForSeconds = new WaitForSeconds(2f);
        private Coroutine checkerCoroutine;
        private Picker picker;
        private Vector3 groundStartingPosition;

        private void Awake()
        {
            groundStartingPosition = ground.localPosition;
        }
        private void OnEnable()
        {
            Restart();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Collectable collectable))
            {
                IncreaseCollected();
                collectable.DelayedDestroy();
                ResultWatchStart();
            }
            else if (other.attachedRigidbody.TryGetComponent(out Picker picker))
            {
                //Picker hiç bir collectable objecte sahip deðilse yine de ResultWatcher çalýþmalý!
                this.picker = picker;
                picker.Stop();
                picker.PushCollectables();
                ResultWatchStart();
            }
        }
        public void Restart()
        {
            collectedAmount = 0;
            AmountTextUpdate();
            ground.localPosition = groundStartingPosition;
            foreach (Transform barricade in barricades)
            {
                Vector3 rot = barricade.localRotation.eulerAngles;
                rot.z = 0;
                barricade.localRotation = Quaternion.Euler(rot);
            }
        }
        private void IncreaseCollected()
        {
            collectedAmount++;
            AmountTextUpdate();
        }
        private void AmountTextUpdate()
        {
            amountText.SetText(string.Format("{0} / {1}", collectedAmount, RequireAmount));
        }
        private void ResultWatchStart()
        {
            if (IsGateOpened) return;
            if (checkerCoroutine is not null)
            {
                StopCoroutine(checkerCoroutine);
            }
            checkerCoroutine = StartCoroutine(ResultWatcher());
        }

        private IEnumerator ResultWatcher()
        {
            yield return waitForSeconds; //gc alloc fix
            if (IsGateOpened)
            {
                Open();
            }
            else
            {
                gameFailedEventChannel.RaiseEvent();
            }
        }

        private void Open()
        {
            gateEventChannel.RaiseEvent(this, GateIndex);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(ground.DOLocalMoveY(0, 1f));
            sequence.Append(barricades[0].DOLocalRotate(new Vector3(0, 0, -80f), 2f, RotateMode.LocalAxisAdd));
            sequence.Join(barricades[1].DOLocalRotate(new Vector3(0, 0, -80f), 2f, RotateMode.LocalAxisAdd));

            sequence.OnComplete(() =>
            {
                picker.Release();
            });
        }


    }
}
