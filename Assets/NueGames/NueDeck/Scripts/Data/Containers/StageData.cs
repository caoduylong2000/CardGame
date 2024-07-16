using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Stage Data
[System.Serializable]
public class StageData
{
    //Stage Object
    [System.Serializable]
    public class Stage
    {
        //Stage Part
        [System.Serializable]
        public class StagePart
        {
            public GameObject StageItem;
            public bool StageFlipped;

            public StagePart(GameObject stageItem, bool stageFlipped)
            {
                StageItem = stageItem;
                StageFlipped = stageFlipped;
            }
        }

        public List<StagePart> StageParts = new List<StagePart>();

        public Stage(List<StagePart> stageParts)
        {
            StageParts = stageParts;
        }
    }

    public List<Stage> StageList = new List<Stage>();

    public StageData(List<Stage> stageList)
    {
        StageList = stageList;
    }
}
