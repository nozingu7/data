using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public int number;
    public GameObject droppedItemPrefab;

    void OnDrop(GameObject dropped)
    {
        if (dropped.CompareTag("Item"))
        {
            dropped.transform.parent = this.transform;
            dropped.transform.localPosition = Vector3.zero;
            //Debug.Log(UICamera.lastHit.collider.transform.name);
            //if (dropped.transform.parent.childCount > 1)
            //{
            //    temp = dropped.transform.parent.GetChild(1).transform;
            //}


            //if (temp != null)
            //{
            //    temp.transform.parent = this.transform;
            //    temp.transform.localPosition = Vector3.zero;
            //}
            //this.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
        }


        // 드롭된 게임오브젝트에 Z_Item 컴포넌트가 있는지 확인하다.
        //temtem droppedItem = dropped.GetComponent<temtem>();
        //Debug.Log(droppedItem.name);
        //// 컴포넌트가 없다면, 즉 아이템이 아니라면 더 이상 진행할 필요가 없다.
        //if (droppedItem == null)
        //    return;
        //Debug.Log(this.transform.GetChild(0).name);
        //dropped.transform.parent = this.transform.GetChild(0).transform;

        // 드롭된 아이템 프리팹의 인스턴스를 생성한다.
        //GameObject newPower = NGUITools.AddChild(itemParent,
        //                                         droppedItemPrefab);

        //newPower.GetComponent<UIDragObject>().target = droppedItemPrefab.transform;
        //newPower.GetComponent<UIDragObject>().dragEffect = UIDragObject.DragEffect.None;

        // 드롭된 게임오브젝트는 삭제한다.
        //Destroy(dropped);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("ㅎ");
    //    if (collision.transform.tag == "Item")
    //        Debug.Log("왓따~");
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("ㅋ");
    //    if (other.tag == "Item")
    //    {
    //        other.transform.parent = this.transform;
    //        other.transform.localPosition = Vector3.zero;
    //    }
    //}
}
