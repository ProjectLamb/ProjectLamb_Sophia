using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.MUIP;
public class NotiForceClose : MonoBehaviour
{
    [SerializeField] NotificationManager notificationManager;

    public void CloseForce() {
        notificationManager.isOn = false;
        notificationManager.notificationAnimator.Play("Out");
        notificationManager.onClose.Invoke();
        notificationManager.StopAllCoroutines();
        if (notificationManager.closeBehaviour == NotificationManager.CloseBehaviour.Disable) { 
            notificationManager.gameObject.SetActive(false); 
            notificationManager.isOn = false; 
        }
        else if (notificationManager.closeBehaviour == NotificationManager.CloseBehaviour.Destroy) { 
            Destroy(notificationManager.gameObject); 
        }
        
    }
}
