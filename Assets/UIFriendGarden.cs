using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFriendGarden : MonoBehaviour {

    public void GoBackToMyGarden() {
        SceneManager.LoadScene("myGarden");
    }
}
