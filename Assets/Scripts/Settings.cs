using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Languages
{
    English = 0,
    Hrvatski = 1
}

public static class Settings
{
    [SerializeField]
    public static Languages GameLanguage = Languages.Hrvatski;
}
