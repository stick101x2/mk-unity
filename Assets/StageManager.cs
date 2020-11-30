using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [System.Serializable]
    public class TerrianType
    {
        public string name;
        public Color collisionColor = Color.green;
        public TerrianSetting data;
    }
    public List<TerrianType> terrianTypes;
}
