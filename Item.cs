using UnityEngine;

public class Item : MonoBehaviour
{
    public string name;
    public int price;
    public ItemType itemtype;

    public SelectItemData itemData;
    private Vector3 pos;
    private Collider collider;
    private UISprite sprite;
    private UIDragObject _dragObject;
    private GameObject popup;
    public bool isSelect = false;
    private bool isDrag = false;
    public bool isHover = false;
    public bool equipItem = false;
    private GameObject itemInformation;

    private void Start()
    {
        itemtype = itemData.type;
        collider = this.gameObject.GetComponent<BoxCollider>();
        sprite = transform.GetComponent<UISprite>();
        _dragObject = gameObject.GetComponent<UIDragObject>();
        _dragObject.target = this.gameObject.transform;
        itemInformation = GameObject.Find("Anchor").gameObject;
    }

    private void Update()
    {
        if (isHover == true && Input.GetMouseButtonDown(1) && equipItem == false)
        {
            if (true == isSelect)
            {
                isSelect = false;
                this.transform.GetChild(0).gameObject.SetActive(false);
                Manager.instance.inventorySelectObject = null;
            }
            foreach (var slot in Manager.instance.stateSlot)
            {
                if (slot.GetComponent<StateSlot>().slotType == itemData.type)
                {
                    if (slot.childCount != 0)
                    {
                        Transform temp = slot.GetChild(0).transform;
                        temp.parent = this.transform.parent;
                        temp.transform.localPosition = Vector3.zero;
                        temp.transform.localScale = Vector3.one;
                        temp.GetComponent<Item>().equipItem = false;
                        Manager.instance.playerState = StateSub(Manager.instance.playerState, temp.GetComponent<Item>().itemData.state);

                        this.transform.parent = slot;
                        this.transform.localPosition = Vector3.zero;
                        this.transform.localScale = Vector3.one;
                        this.transform.gameObject.SetActive(false);
                        this.transform.gameObject.SetActive(true);
                        Manager.instance.playerState = StateAdd(Manager.instance.playerState, itemData.state);
                        Destroy(popup.gameObject);
                        isHover = false;
                        equipItem = true;

                        SoundAction.instance.audio.volume = 0.4f;
                        SoundAction.instance.audio.PlayOneShot(SoundAction.instance.clips[1]);
                    }
                    else
                    {
                        Manager.instance.playerSlotCount--;
                        this.transform.parent = slot;
                        this.transform.localPosition = Vector3.zero;
                        this.transform.localScale = Vector3.one;
                        this.transform.gameObject.SetActive(false);
                        this.transform.gameObject.SetActive(true);
                        Manager.instance.playerState = StateAdd(Manager.instance.playerState, itemData.state);
                        Manager.instance.equipItemCount++;
                        Destroy(popup.gameObject);
                        isHover = false;
                        equipItem = true;

                        SoundAction.instance.audio.volume = 0.5f;
                        SoundAction.instance.audio.PlayOneShot(SoundAction.instance.clips[1]);
                        break;
                    }
                }
            }
        }
        else if (isHover == true && equipItem == true && Input.GetMouseButtonDown(1))
        {
            if (Manager.instance.playerSlotCount >= 20)
            {
                Manager.instance.block.SetActive(true);
                Manager.instance.msg.SetActive(true);
                TweenScale ts = Manager.instance.msg.GetComponent<TweenScale>();
                UILabel label = Manager.instance.msg.transform.GetChild(0).GetComponent<UILabel>();
                label.text = "가방의 여유공간이\n없습니다!";
                ts.PlayForward();
                SoundAction.instance.audio.volume = 0.5f;
                SoundAction.instance.audio.PlayOneShot(SoundAction.instance.clips[2]);
            }
            else
            {
                foreach (var slot in Manager.instance.inventorySlot)
                {
                    if (slot.childCount == 0)
                    {
                        Manager.instance.playerSlotCount++;
                        this.transform.parent = slot;
                        this.transform.localPosition = Vector3.zero;
                        this.transform.localScale = Vector3.one;
                        Manager.instance.playerState = StateSub(Manager.instance.playerState, itemData.state);
                        Manager.instance.equipItemCount--;
                        equipItem = false;

                        SoundAction.instance.audio.volume = 0.5f;
                        SoundAction.instance.audio.PlayOneShot(SoundAction.instance.clips[1]);
                        break;
                    }
                }
            }
        }
    }

    public static PlayerState StateAdd(PlayerState playerState, PlayerState itemState)
    {
        playerState.hp += itemState.hp;
        playerState.mp += itemState.mp;
        playerState.str += itemState.str;
        playerState.def += itemState.def;
        playerState.dex += itemState.dex;
        playerState._int += itemState._int;

        return playerState;
    }

    public static PlayerState StateSub(PlayerState playerState, PlayerState itemState)
    {
        playerState.hp -= itemState.hp;
        playerState.mp -= itemState.mp;
        playerState.str -= itemState.str;
        playerState.def -= itemState.def;
        playerState.dex -= itemState.dex;
        playerState._int -= itemState._int;

        return playerState;
    }

    void OnHover(bool on)
    {
        if (on)
            isHover = true;
        else
            isHover = false;

        if (on && !isDrag)
        {
            if (popup != null) { Destroy(popup.gameObject); }

            // 프리팹을 NGUI 오브젝트의 자식으로 만들때 사용 NGUITools.AddChild( 부모객체, 프리팹 )
            popup = NGUITools.AddChild(itemInformation, Manager.instance.popup);

            // ★ 뷰표트 좌표는 좌상단이 0 , 0 이다. 즉 좌상단이 기준
            // ★ 스크린 좌표는 좌하단이 기준이다.
            // ★ NGUI 오브젝트들은 화면 정중앙이 기준이다.

            // pos변수에 마우스포지션 정보를 넣고 pos의 x, y 를 Mathf.Clamp01 함수를 이용해서 0.0f ~ 1.0f 사이의 값으로 변환, z값은 반드시 0
            // ★ 뷰포트 좌표는 좌우 상하로 0 ~ 1 사이의 값을 갖는다.
            // 그리고 UI를 비추는 카메라로 뷰표트좌표로 값을 정해둔 pos를 스크린 좌표로 변환한다.
            pos = Input.mousePosition;
            pos.x = Mathf.Clamp01(pos.x / Screen.width);
            pos.y = Mathf.Clamp01(pos.y / Screen.height);
            pos.z = 0.0f;

            // Mathf.Round 함수는 인자의 값을 가장 가까운 정수로 변환시켜준다.
            // ex) 만약 인자로 10.2가 들어왔다면 10으로 변환시킴.
            popup.transform.localPosition = Manager.instance.cam.ViewportToScreenPoint(pos);
            Vector3 lp = popup.transform.localPosition;
            lp.x = Mathf.Round(lp.x);
            lp.y = Mathf.Round(lp.y);
            popup.transform.localPosition = lp;

            UILabel text = popup.transform.GetChild(0).GetComponent<UILabel>();
            text.text = string.Format("이름 : {0}\n{1}\n판매가격 : {2}", itemData.name, itemData.information, itemData.price);
            text.transform.localPosition = Vector3.zero;
        }
        else
        {
            if (popup != null)
                Destroy(popup.gameObject);
        }
    }

    void OnDragStart()
    {
        Destroy(popup.gameObject);
        Manager.instance.oldTR = this.transform.parent.transform;
        isDrag = true;
        collider.enabled = false;
        if (isSelect)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }

        sprite.depth = 10;
    }

    void OnDragEnd()
    {
        isDrag = false;
        collider.enabled = true;
        if (isSelect)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

        Collider col = UICamera.lastHit.collider;
        if (!col.CompareTag("Item") || !col.CompareTag("Slot"))
        {
            this.transform.localPosition = Vector3.zero;
        }

        sprite.depth = 5;
    }

    void OnClick()
    {
        if (false == equipItem && UICamera.currentKey == KeyCode.Mouse0)
        {
            // 이미 선택된 아이템이 있고, 현재 클릭한 아이템과 다르다면 이미 선택된 아이템의 셀렉트를 해제하고, 현재 클릭한 아이템으로 셀렉트를 변경한다.
            if (Manager.instance.inventorySelectObject != null && Manager.instance.inventorySelectObject != this.transform)
            {
                // 보유한 아이템리스트 중에서 첫번째자식 ( Select )의 active가 true라면 fasle로 바꿔주고 해당 오브젝트의 bool값도 변경
                foreach (GameObject list in Manager.instance.possessionItem)
                {
                    if (list.transform.GetChild(0).gameObject.active == true)
                    {
                        list.transform.GetComponent<Item>().isSelect = false;
                        list.transform.GetChild(0).gameObject.SetActive(false);
                        break;
                    }
                }
                isSelect = true;
                this.transform.GetChild(0).gameObject.SetActive(true);
                Manager.instance.inventorySelectObject = this.transform.transform;
            }
            // 이미 선택된 아이템이 있고, 그 아이템이 현재 클릭한 아이템과 같다면 선택된 아이템의 셀렉트를 해제한다.
            else if (Manager.instance.inventorySelectObject != null && Manager.instance.inventorySelectObject == this.transform)
            {
                isSelect = false;
                this.transform.GetChild(0).gameObject.SetActive(false);
                Manager.instance.inventorySelectObject = null;
            }
            // 이전에 선택한 아이템이 아무것도 없을경우 현재 클릭한 아이템의 셀렉트로 한다.
            else if (Manager.instance.inventorySelectObject == null)
            {
                isSelect = true;
                this.transform.GetChild(0).gameObject.SetActive(true);
                Manager.instance.inventorySelectObject = this.transform.transform;

                // 상점에 선택하고 있는 아이템이 있다면, 상점의 아이템 셀렉트를 해제한다.
                Destroy(GameObject.FindGameObjectWithTag("Select"));
                Manager.instance.selectObject = null;
                Manager.instance.sellerLabel.text = "";
            }
        }
    }

    // 아이템위에 드랍된 객체의 tag가 Item 이라면 둘의 위치를 서로 스왑해준다.
    void OnDrop(GameObject drop)
    {
        if (drop.CompareTag("Item"))
        {
            Manager.instance.newTR = this.transform.parent.transform;
            this.transform.parent = Manager.instance.oldTR;
            this.transform.localPosition = Vector3.zero;

            drop.transform.parent = Manager.instance.newTR;
            drop.transform.localPosition = Vector3.zero;
        }
    }
}