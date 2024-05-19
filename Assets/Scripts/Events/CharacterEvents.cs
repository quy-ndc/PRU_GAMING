using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


public class CharacterEvents
{
    public static UnityAction<GameObject, float> characterDamaged;
    public static UnityAction<GameObject, string> characterParried;
    public static UnityAction<GameObject, float> characterBlocked;
    public static UnityAction<GameObject, Vector2, string> characterTalk;
}

