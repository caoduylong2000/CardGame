using System.Collections.Generic;

[System.Serializable]
public class VoacationalTraining
{
    [System.Serializable]
    public class TrainingLevelInfo
    {
        [System.Serializable]
        public class ListIndex
        {
            public float AttackBonus;
            public float RecoveryBonus;
            public float ClosingRate;
            public float LegendItemRate;
            public float MoneyBonus;
            public float DesireBonus;
            public float PlayerHP;
            public float CrystalRate;
        }

        public int TrainingLv;
        public int RequireCrystal;
        public ListIndex ListIndexMul;
    }

    public List<TrainingLevelInfo> ListTrainingLevelInfo;
}