using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : Singleton<PlayerMotor> {

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    //Mine mechanics
    private float destructionCounter = 0;
    private int _hittingBlockId = 0;
    [SerializeField]
    private float miningSpeed = 0.3f;
    public bool canInteract = true;

    private CharacterController controller;
    public Camera playerCamera;

    private void Awake()
    {
        
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        PlayerMoveInput();

        if (Input.GetMouseButton(0))
        {
            InteractLeft();
        }
        else if (Input.GetMouseButton(1))
        {
            InteractRight();
        }

        //Free cursor for inventory
        if (Input.GetKey("e"))
        {
            if (!GameManager.Instance.inInventory)
                GameManager.Instance.inInventory = true;
            else
                GameManager.Instance.inInventory = false;
        }

        //hotkeys for inventory bar
        if (Input.GetKey("1"))
        {
            Debug.Log("Pressed 1");
            Inventory.Instance.MoveCursorToSlot(1);
        }
        if (Input.GetKey("2"))
        {
            Debug.Log("Pressed 2");

            Inventory.Instance.MoveCursorToSlot(2);
        }
        if (Input.GetKey("3"))
        {
            Debug.Log("Pressed 3");

            Inventory.Instance.MoveCursorToSlot(3);
        }
        if (Input.GetKey("4"))
        {
            Debug.Log("Pressed 4");

            Inventory.Instance.MoveCursorToSlot(4);
        }
        if (Input.GetKey("5"))
        {
            Debug.Log("Pressed 5");

            Inventory.Instance.MoveCursorToSlot(5);
        }
        if (Input.GetKey("6"))
        {
            Debug.Log("Pressed 6");

            Inventory.Instance.MoveCursorToSlot(6);
        }
    }

    void PlayerMoveInput()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;

        transform.rotation = new Quaternion(0, Camera.main.transform.rotation.y, 0, transform.rotation.w);
        controller.Move(moveDirection * Time.deltaTime);
    }

    void InteractLeft()
    {
        if (canInteract)
        {
            StartCoroutine(InteractLeftRoutine());
            canInteract = false;
        }
    }

    IEnumerator InteractLeftRoutine()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(transform.position, playerCamera.ScreenPointToRay(Input.mousePosition).direction, Color.red, 10f);

        if (Physics.Raycast(ray, out hit, 1.75f))
        {
            Transform objectHit = hit.transform;

            //Debug.Log(objectHit.gameObject.name);

            if (objectHit.tag == "DestructibleBlock")
            {
                Block block = objectHit.GetComponent<Block>();

                if (block.ID == _hittingBlockId)
                {
                    if (destructionCounter < block.BlockResistance)
                    {
                        destructionCounter += miningSpeed;
                    }
                    else
                    {
                        //Destroy block and spawn item
                        BlocksManager.Instance.DropItem(block.blockType, objectHit.gameObject.transform.position, objectHit.gameObject.transform.rotation);
                        Destroy(objectHit.gameObject);
                    }
                }
                else
                {
                    //New block, restart counters and change ID
                    _hittingBlockId = block.ID;
                    destructionCounter = 0;

                    if (destructionCounter < block.BlockResistance)
                    {
                        destructionCounter += miningSpeed;
                    }
                    else
                    {
                        //Destroy block and spawn item
                        BlocksManager.Instance.DropItem(block.blockType, objectHit.gameObject.transform.position, objectHit.gameObject.transform.rotation);
                        Destroy(objectHit.gameObject);
                    }
                }
            }
        }
        yield return new WaitForSeconds(.30f);
        canInteract = true;
    }

    void InteractRight()
    {
        if (canInteract)
        {
            StartCoroutine(InteractRightRoutine());
            canInteract = false;
        }
    }

    IEnumerator InteractRightRoutine()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(transform.position, playerCamera.ScreenPointToRay(Input.mousePosition).direction, Color.blue, 10f);

        if (Physics.Raycast(ray, out hit, 1.75f))
        {
            Transform objectHit = hit.transform;

            //Debug.Log(objectHit.gameObject.name);
            if(Inventory.Instance.items.Count > 0)
            {
                PlaceBlock(ReturnDirection(objectHit.gameObject, this.gameObject), objectHit);
            }
        }
        yield return new WaitForSeconds(.30f);
        canInteract = true;
    }

    void PlaceBlock(HitDirection direction, Transform objectHit)
    {
        switch (direction)
        {
            case HitDirection.Top:
                Instantiate(Inventory.Instance.PickBlock(), new Vector3(objectHit.position.x, objectHit.position.y + 0.5f, objectHit.position.z), objectHit.rotation);
                break;
            case HitDirection.Bottom:
                Instantiate(Inventory.Instance.PickBlock(), new Vector3(objectHit.position.x, objectHit.position.y - 0.5f, objectHit.position.z), objectHit.rotation);
                break;
            case HitDirection.Right:
                Instantiate(Inventory.Instance.PickBlock(), new Vector3(objectHit.position.x + 0.5f, objectHit.position.y, objectHit.position.z), objectHit.rotation);
                break;
            case HitDirection.Left:
                Instantiate(Inventory.Instance.PickBlock(), new Vector3(objectHit.position.x - 0.5f, objectHit.position.y, objectHit.position.z), objectHit.rotation);
                break;
            case HitDirection.Forward:
                Instantiate(Inventory.Instance.PickBlock(), new Vector3(objectHit.position.x, objectHit.position.y, objectHit.position.z + 0.5f), objectHit.rotation);
                break;
            case HitDirection.Back:
                Instantiate(Inventory.Instance.PickBlock(), new Vector3(objectHit.position.x, objectHit.position.y, objectHit.position.z - 0.5f), objectHit.rotation);
                break;
        }
        InventoryUI.Instance.UpdateUI();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!Inventory.Instance.processingItem)
        {

            if (hit.gameObject.tag == "Item")
            {
                Debug.Log("Hit something: " + hit.gameObject.name);

                bool wasPickedUp = Inventory.Instance.Add(hit.gameObject.GetComponent<Item>());

                if (wasPickedUp)
                    hit.gameObject.SetActive(false);

                StartCoroutine(ItemWaiter(0.3f));
            }
        }
    }

    IEnumerator ItemWaiter(float time)
    {
        yield return new WaitForSeconds(time);
        Inventory.Instance.processingItem = false; 
    }

    private enum HitDirection { None, Top, Bottom, Forward, Back, Left, Right }
    private HitDirection ReturnDirection(GameObject Object, GameObject ObjectHit)
    {

        HitDirection hitDirection = HitDirection.None;
        RaycastHit MyRayHit;
        Vector3 direction = (Object.transform.position - ObjectHit.transform.position).normalized;
        Ray MyRay = new Ray(ObjectHit.transform.position, direction);

        if (Physics.Raycast(MyRay, out MyRayHit))
        {

            if (MyRayHit.collider != null)
            {

                Vector3 MyNormal = MyRayHit.normal;
                MyNormal = MyRayHit.transform.TransformDirection(MyNormal);

                if (MyNormal == MyRayHit.transform.up) { hitDirection = HitDirection.Top; }
                if (MyNormal == -MyRayHit.transform.up) { hitDirection = HitDirection.Bottom; }
                if (MyNormal == MyRayHit.transform.forward) { hitDirection = HitDirection.Forward; }
                if (MyNormal == -MyRayHit.transform.forward) { hitDirection = HitDirection.Back; }
                if (MyNormal == MyRayHit.transform.right) { hitDirection = HitDirection.Right; }
                if (MyNormal == -MyRayHit.transform.right) { hitDirection = HitDirection.Left; }
            }
        }
        return hitDirection;
    }
}

