﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class menuScript : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("AtkinsAR");
    }
}
