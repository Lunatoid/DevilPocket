using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstToSelcetBTN : MonoBehaviour
{
    public Button firtstSelect;

    private void Awake() {
        firtstSelect.Select();
    }
}
