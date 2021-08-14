﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllableSummon 
{
    void HandleCommand(PlayerCommand command);

    string GetStatString();

    bool CanBeSelected();

    Transform GetTransform();
}