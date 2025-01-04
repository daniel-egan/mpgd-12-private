//This script was made using this tutorial: https://www.youtube.com/watch?v=NqrJHj9xlqY

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnMobile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(Application.isMobilePlatform); //only enables if on mobile
    }
}
