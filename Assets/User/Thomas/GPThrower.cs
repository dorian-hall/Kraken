using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameProto {
    public class GPThrower : MonoBehaviour
    {
        private bool isRotateLeftDown = false;
        private bool isRotateRightDown = false;
        //private bool isForceIncreaseDown = false;
        private bool isLoaded = false;
 
        private GameObject ItemInstance;
        //[SerializeField]
        private GameObject ItemPrefab;

        private int selectedItem;

        [Header("Items")]
        public float HoldingDistanceForward = 1;
        public float HoldingDistanceUp = 1;

        public List<GameObject> items = new();
        public int SelectedItem = 0;
        public bool IsAutoLoading = true; // todo: cooldown
        public KeyCode ItemLoadKey = KeyCode.X;
        public KeyCode ItemSelectKey = KeyCode.Tab;


        // todo: throw direction
        // todo: max throwing range
        [Header("Targetting")]
        [SerializeField] private Vector3 Rotation = Vector3.zero;
        public KeyCode RotateLeftKey = KeyCode.Q;
        public KeyCode RotateRightKey = KeyCode.E;

        [Header("Throwing Forces")]
        [SerializeField] private float currentForce;
        [SerializeField] private float InitialForce = 100;
        [SerializeField] private float ForceIncrease = 10;
        [SerializeField] private KeyCode ForceIncreaseKey = KeyCode.LeftShift;

        [Header("Throw Conditions")]
        [SerializeField] private bool UseReleaseKey = true;
        [SerializeField] private KeyCode ReleaseKey = KeyCode.Mouse0;
        [SerializeField] private bool UseNeededReleaseForce = true;
        [SerializeField] private float NeededReleaseForce = 1000;

        private void Start()
        {
            SelectItem(0);
            LoadItem();
        }

        void Update()
        {
            // basic targetting

            // decrease rotation (angle in degrees) around y axis, gear/turn left
            if (Input.GetKeyDown(RotateLeftKey)) isRotateLeftDown = true;
            if (Input.GetKeyUp(RotateLeftKey)) isRotateLeftDown = false;
            if (isRotateLeftDown)
            {
                Rotation.y -= 1;
            }

            // increase rotation (angle in degrees) around y axis, gear/turn right
            if (Input.GetKeyDown(RotateRightKey)) isRotateRightDown = true;
            if (Input.GetKeyUp(RotateRightKey)) isRotateRightDown = false;
            if (isRotateRightDown) {
                Rotation.y += 1;
            }

            // increase rotation angle around x-axis, pitch/rotate up/down
            Rotation.x += Input.mouseScrollDelta.y;
            transform.localRotation = Quaternion.Euler(Rotation); // finally set rotation

            // item loading
            if (Input.GetKeyDown(ItemLoadKey))
                LoadItem();
            // item selection
            if (Input.GetKeyDown(ItemSelectKey))
                SwitchItem();

            if (isLoaded) // item throwing
            {
                // hold item at some distance in front/up of thrower
                if (ItemInstance)
                {
                    ItemInstance.transform.position =
                        transform.position + 
                        transform.forward * HoldingDistanceForward + 
                        transform.up      * HoldingDistanceUp;
                }

                //if (isForceIncreaseDown == true)
                if (Input.GetKey(ForceIncreaseKey))
                {
                    StartCoroutine(Wait(1));
                    currentForce += ForceIncrease;
                }
                // release item
                if (UseReleaseKey && Input.GetKey(ReleaseKey))
                    ThrowItem();
                if (UseNeededReleaseForce && currentForce >= NeededReleaseForce)
                    ThrowItem();
            }

        }

        //todo: OnValidate, ValidateSelected
        public void SelectItem(int i = 0)
        {
            if (items.Count == 0)
                return;

            if (i >= 0 && i < items.Count)
            {
                selectedItem = i;
                SelectedItem = i;
                ItemPrefab = items[SelectedItem];
            }
        }

        public void SwitchItem()
        {
            if (items.Count == 0)
                return;

            SelectedItem++;
            selectedItem = SelectedItem % items.Count;
            SelectedItem = selectedItem;

            ItemPrefab = items[SelectedItem];

            UpdateLoadedItem();
        }

        void UpdateLoadedItem()
        {
            if (!isLoaded)
            {
                LoadItem();
            }
            if (isLoaded && ItemInstance)
            {
                Destroy(ItemInstance);
                isLoaded = false;
                LoadItem();
            }
        }

        public void LoadItem()
        {
            if (ItemPrefab == null)
                return;

            if (!isLoaded)
            {
                ItemInstance = Instantiate(ItemPrefab, transform.parent);
                ItemInstance.transform.position = transform.position + transform.forward * HoldingDistanceForward;

                if (ItemInstance.TryGetComponent<Rigidbody>(out Rigidbody body))
                {
                    body.useGravity = false;
                    body.constraints =
                        RigidbodyConstraints.FreezeRotationX |
                        RigidbodyConstraints.FreezeRotationY |
                        RigidbodyConstraints.FreezeRotationZ;

                }
                if (ItemInstance.TryGetComponent<Collider>(out Collider collider))
                    collider.enabled = false;

                currentForce = InitialForce;
                isLoaded = true;
            }
        }

        public void ThrowItem()
        {
            if (isLoaded)
            {
                if (ItemInstance.TryGetComponent<Rigidbody>(out Rigidbody body))
                {
                    body.isKinematic = false;
                    body.useGravity = true;
                    body.AddForce(transform.forward * currentForce);
                }
                if (ItemInstance.TryGetComponent<Collider>(out Collider collider))
                    collider.enabled = true;

                isLoaded = false;
                if (IsAutoLoading)
                    LoadItem();
            }
        }

        IEnumerator Wait(float d)
        {
            yield return new WaitForSeconds(d);
        }

    } // end class
} // end namespace
