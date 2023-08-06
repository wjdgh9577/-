using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uMMORPG;

#region String
[Serializable]
public class StringData : TableData.IData<string>
{
    public string stringId;
    public string ko;
    public string en;
    public string jp;
    public string fr;
    public string gr;

    public string Key()
    {
        return stringId;
    }
}
#endregion

#region NPC Script
[Serializable]
public class NpcScriptData
{
    public int ScriptSetID;
    public int ScriptSequence;
    public int Speaker;
    public int Type;
    public string stringId;
}
#endregion

#region Skill/Buff
[System.Serializable]
public class BT_Skill
{
    public int skillID;
    public int skillLevel;
    public string skillType;
    public string effectType;
    public int cooldown;
    public int mP;
    public float castTime;
    public int castRange;
    public string targetType;
    public float targetRange;
    public string affectTarget;
    public float dmgMulS;
    public float dmgAddS;
    public int buffID;
    public string reqWeapon;
    public int reqLv;
    public int reqSkillID1;
    public int reqSkillLv1;
    public int reqSkillID2;
    public int reqSkillID3;
    public string attribute;
    public string animation;
    public float atkMulS;
    public int atkAddS;
}

[System.Serializable]
public class BT_Buff
{
    public enum BuffType
    {
        SETMINING,
        SETLOGGING,
        SETMOWING,
        SETFISHING,
        MPREGEN,
        HPREGEN,
        DMGMULS,
        DMGADDS,
        HPMULS,
        HPADDS,
        MSPDMULS,
        HPRECOVERYMULS,
        HPRECOVERYADDS,
        MPMULS,
        MPADDS,
        MATKMULS,
        MATKADDS,
        PATKMULS,
        PATKADDS,
        STRADDS,
        FCNADDS,
        PDEFMULS,
        CONADDS,
        PDEFADDS,
        MDEFMULS,
        WILADDS,
        MDEFADDS,
        HITMULS,
        HITADDS,
        DDGMULS,
        DDGADDS,
        BLKCHANCEMULS,
        CRIMULS,
        CRIADDS,
        MPENMULS,
        MGCADDS,
        MPENADDS,
        MRESMULS,
        MRESADDS,
        ASPDMULS,
        ASPDADDS,
        ACCLMULS,
        DMGREDMULS,
        DMGREDS,
    }

    public int buffID;
    public string buffType;
    public string effectType;
    public float duration;
    public float tickInterval;
    public string paramType1;
    public float paramAmount1;
    public string paramType2;
    public float paramAmount2;
    public string paramType3;
    public float paramAmount3;
    public string paramType4;
    public float paramAmount4;
    public string paramType5;
    public float paramAmount5;
}
#endregion

#region Recipe
[Serializable]
public class RecipeData
{
    public int typeID;
    public int unlockLevel;
    public int outputID;
    public int outputCount;
    public int makeTime;
    public int golds;

    public int skill1;
    public int skillLevel1;
    public int skill2;
    public int skillLevel2;
    public int skill3;
    public int skillLevel3;

    public int matID1;
    public int matCount1;
    public int matID2;
    public int matCount2;
    public int matID3;
    public int matCount3;
    public int matID4;
    public int matCount4;
    public int matID5;
    public int matCount5;
    public int matID6;
    public int matCount6;
    public int matID7;
    public int matCount7;
    public int matID8;
    public int matCount8;
    public int matID9;
    public int matCount9;
    public int matID10;
    public int matCount10;

    [System.Serializable]
    public struct MaterialInfo
    {
        public int matID;
        public int matCount;
    }

    [Serializable]
    public struct SkillInfo
    {
        public int skillID;
        public int level;
    }

    public List<SkillInfo> skillInfos { get; set; }
    public List<MaterialInfo> matInfos { get; set; }

#if UNITY_EDITOR
    public List<MaterialInfo> Setup()
    {
        matInfos = new List<RecipeData.MaterialInfo>();

        for (int i = 1; i <= 10; i++)
        {
            FieldInfo fieldInfo0 = typeof(RecipeData).GetField("matID" + i);
            FieldInfo fieldInfo1 = typeof(RecipeData).GetField("matCount" + i);
            int id = (int)fieldInfo0.GetValue(this);
            int count = (int)fieldInfo1.GetValue(this);
            if (id != 0 && count != 0)
                matInfos.Add(new MaterialInfo()
                {
                    matID = id,
                    matCount = count
                });
        }

        return matInfos;
    }

    public List<SkillInfo> SetupSkill()
    {
        skillInfos = new List<SkillInfo>();

        for(int i = 1; i <= 3; i++)
        {
            FieldInfo fieldInfo0 = typeof(RecipeData).GetField("skill" + i);
            FieldInfo fieldInfo1 = typeof(RecipeData).GetField("skillLevel" + i);
            int id = (int)fieldInfo0.GetValue(this);
            int level = (int)fieldInfo1.GetValue(this);
            if (id != 0 && level != 0)
                skillInfos.Add(new SkillInfo()
                {
                    skillID = id,
                    level = level
                });
        }

        return skillInfos;
    }
#endif
}
#endregion

#region Quest
[Serializable]
public class QuestData
{
    public int questId;
    public int playerMinLevel;
    public int playerMaxLevel;
    public string questConditionType;
    public int questTargetId;
    public int progressCount;
    public int previousQuestId;
    public int exp;
    public int gem;
    public int gold;
    public int item1Id;
    public int item1Count;
    public int item2Id;
    public int item2Count;
    public int item3Id;
    public int item3Count;
    public int item4Id;
    public int item4Count;
    public int startNPC;
    public int endNPC;
}
#endregion

#region Mission
[Serializable]
public class MissionData
{
    [Header("========== 일반 ==========")]
    public int MissionID;

    [Tooltip("도시발전기금 또는 탐험가 연맹")]
    public int Group;

    [Tooltip("일반/일일/주간")]
    public int Type;

    [Header("========== 요구 사항 ==========")]

    [Tooltip("최소 계정 레벨 요구량")]
    public int PlayerMinLevel;

    [Tooltip("최대 계정 레벨 요구량")]
    public int PlayerMaxLevel;

    [Tooltip("수행 시 필요한 생산 스킬1")]
    public string ProduceSkill1;
    [Tooltip("생산 스킬1 레벨 요구량")]
    public string ProduceReqLevel1;

    [Tooltip("수행 시 필요한 생산 스킬2")]
    public string ProduceSkill2;
    [Tooltip("생산 스킬2 레벨 요구량")]
    public string ProduceReqLevel2;

    [Tooltip("수행 시 필요한 생산 스킬3")]
    public string ProduceSkill3;
    [Tooltip("생산 스킬3 레벨 요구량")]
    public string ProduceReqLevel3;

    public string MissionConditionType;
    public int MissionTargetId;
    public int ProgressCount;

    [Header("========== 보상 ==========")]

    [Tooltip("계정 경험치 보상")]
    public int Exp;

    public string Skill;
    [Tooltip("제작스킬 경험치 보상")]
    public string SkillExp;

    [Tooltip("젬 보상(블록페스타와 연계된 재화)")]
    public string Gem;

    [Tooltip("골드 보상)")]
    public string Gold;

    public int item1Id;
    public int item1Count;

    public int item2Id;
    public int item2Count;

    public int item3Id;
    public int item3Count;

    public int item4Id;
    public int item4Count;

    public int GetGem()
    {
        if (string.IsNullOrEmpty(Gem))
            return 0;

        return int.Parse(Gem);
    }

    public int GetGold()
    {
        if (string.IsNullOrEmpty(Gold))
            return 0;

        return int.Parse(Gold);
    }

    public int GetSkill()
    {
        if (string.IsNullOrEmpty(Skill))
            return 0;

        return int.Parse(Skill);
    }



    public int GetProduceSkill1()
    {
        if (string.IsNullOrEmpty(ProduceSkill1))
            return 0;

        return int.Parse(ProduceSkill1);
    }



    public int GetProduceReqLevel1()
    {
        if (string.IsNullOrEmpty(ProduceReqLevel1))
            return 0;

        return int.Parse(ProduceReqLevel1);
    }

    public int GetProduceSkill2()
    {
        if (string.IsNullOrEmpty(ProduceSkill2))
            return 0;

        return int.Parse(ProduceSkill2);
    }



    public int GetProduceReqLevel2()
    {
        if (string.IsNullOrEmpty(ProduceReqLevel2))
            return 0;

        return int.Parse(ProduceReqLevel2);
    }

    public int GetProduceSkill3()
    {
        if (string.IsNullOrEmpty(ProduceSkill3))
            return 0;

        return int.Parse(ProduceSkill3);
    }



    public int GetProduceReqLevel3()
    {
        if (string.IsNullOrEmpty(ProduceReqLevel3))
            return 0;

        return int.Parse(ProduceReqLevel3);
    }

    public int GetSkillExp()
    {
        if (string.IsNullOrEmpty(SkillExp))
            return 0;

        return int.Parse(SkillExp);
    }
}
#endregion

#region Item
public class BT_ItemData
{
    public int itemId;
    public int maxStack;
    public int priceGold;
    public int priceGem;
    public int sellPrice;
    public bool sellable;
    public bool tradable;
    public bool destroyable;
}

public class BT_Item_Equipment : BT_ItemData
{
    public int equipPart;
    public int weaponType;

    public int reqLv;

    public int pAtkE;
    public int mAtkE;
    public int pDefAddE;
    public int mDefAddE;
    public int blkChanceE;
    public float atkCclE;
    public int skillID;
}

public class BT_Item_Potion : BT_ItemData
{
    public string category;
    public float cooldown;
    public int usageMana;
    public int usageHealth;
}

public class BT_Item_Material : BT_ItemData
{

}

public class BT_Item_Skill : BT_ItemData
{
    public int skillId;
}

public class BT_Item_Buff : BT_ItemData
{
    public string category;
    public int skillId;
    public int skillLevel;
}

public class BT_Item_Drop : BT_ItemData
{
    public int dropId;
    public int dropCount;
}

public class BT_Item_RBox : BT_ItemData
{
    public string category;
    public string useType;
    public string type;
    public int RBoxGroupId;
}
#endregion

#region Cash Shop
public class BT_Cash_Item
{
    public enum ShopType
    {
        Gold,
        Gem
    }

    public enum CostType
    {
        Gem,
        Cash
    }

    public int typeID;
    public ShopType shopType;
    public int itemValue;
    public CostType costType;
    public float costValue;
    public string imagePath;
}
#endregion

#region GatherItem
[Serializable]
public class GatherItemData
{
    public int typeID;
    public string prefabName;
    public string gatherAction;
    public int requiredAbility;
    public int maxHP;
    public int exp;
    public int dropID1;
    public int dropCount1;
    public int dropID2;
    public int dropCount2;
    public int dropID3;
    public int dropCount3;
}
#endregion

#region DropData
[Serializable]
public struct DropDataStruct
{
    public int itemId;
    public int count;
    public int rate;
}

[Serializable]
public class DropData
{
    public int dropID;

    public int itemID1;
    public int count1;
    public int rate1;
    public int itemID2;
    public int count2;
    public int rate2;
    public int itemID3;
    public int count3;
    public int rate3;
    public int itemID4;
    public int count4;
    public int rate4;
    public int itemID5;
    public int count5;
    public int rate5;
    public int itemID6;
    public int count6;
    public int rate6;
    public int itemID7;
    public int count7;
    public int rate7;
    public int itemID8;
    public int count8;
    public int rate8;
    public int itemID9;
    public int count9;
    public int rate9;
    public int itemID10;
    public int count10;
    public int rate10;
    public int itemID11;
    public int count11;
    public int rate11;
    public int itemID12;
    public int count12;
    public int rate12;
    public int itemID13;
    public int count13;
    public int rate13;
    public int itemID14;
    public int count14;
    public int rate14;
    public int itemID15;
    public int count15;
    public int rate15;
    public int itemID16;
    public int count16;
    public int rate16;
    public int itemID17;
    public int count17;
    public int rate17;
    public int itemID18;
    public int count18;
    public int rate18;
    public int itemID19;
    public int count19;
    public int rate19;
    public int itemID20;
    public int count20;
    public int rate20;

    public List<DropDataStruct> GetDropDataStructs()
    {
        var dropDataStructs = new List<DropDataStruct>();

        for (int i = 1; i <= 20; i++)
        {
            FieldInfo fieldInfo_Id = typeof(DropData).GetField("itemID" + i);
            FieldInfo fieldInfo_Count = typeof(DropData).GetField("count" + i);
            FieldInfo fieldInfo_Rate = typeof(DropData).GetField("rate" + i);

            int id = (int)fieldInfo_Id.GetValue(this);
            int count = (int)fieldInfo_Count.GetValue(this);
            int rate = (int)fieldInfo_Rate.GetValue(this);

            if (id != 0 && count != 0 && rate != 0)
            {
                dropDataStructs.Add(new DropDataStruct() { itemId = id, count = count, rate = rate });
            }
            else
            {
                Debug.LogError($"존재하지 않는 itemId : {id}");
            }
        }

        return dropDataStructs;
    }
}
#endregion

#region Monster
[Serializable]
public class MobData
{
    public int typeID;
    public string modelName;
    public int level;

    public int pAtk;
    public int mAtk;
    public int pDef;
    public int mDef;
    public int hit;
    public int ddg;
    public int cri;
    public int mPen;
    public int mRes;
    public int aSpd;
    public int hp;

    public int exp;
    public int dropID1;
    public int dropCount1;
    public int dropID2;
    public int dropCount2;
    public int dropID3;
    public int dropCount3;
}
#endregion

#region NPC
[Serializable]
public class BT_NPCData : TableData.IData<int>
{
    public int NpcID;
    public int Portal;
    public int Shop;
    public int TradeShop;
    public int CraftID;

    public int Key()
    {
        return NpcID;
    }
}
#endregion

#region Structure
[Serializable]
public class BuildingData
{
    public int typeID;
    public int uniqueID;
    public string groundType;
    public int constructRecipeID;
    public int level;
    public int requiredTownLevel;
    public int upGradeTypeID;
    public int abilityValue1;
    public int abilityValue2;
    public int maxBuildingCount;
    public ScriptableBuilding.AbilityType abilityType;
    public int recipeID1;
    public int recipeID2;
    public int recipeID3;
    public int recipeID4;
    public int recipeID5;
    public int recipeID6;
    public int recipeID7;
    public int recipeID8;
    public int recipeID9;
    public int recipeID10;
    public int recipeID11;
    public int recipeID12;
    public int recipeID13;
    public int recipeID14;
    public int recipeID15;
    public int recipeID16;
    public int recipeID17;
    public int recipeID18;
    public int recipeID19;
    public int recipeID20;
    public int recipeID21;
    public int recipeID22;
    public int recipeID23;
    public int recipeID24;
    public int recipeID25;
    public int recipeID26;
    public int recipeID27;
    public int recipeID28;
    public int recipeID29;
    public int recipeID30;
    public int recipeID31;
    public int recipeID32;
    public int recipeID33;
    public int recipeID34;
    public int recipeID35;
    public int recipeID36;
    public int recipeID37;
    public int recipeID38;
    public int recipeID39;
    public int recipeID40;
    public int recipeID41;
    public int recipeID42;
    public int recipeID43;
    public int recipeID44;
    public int recipeID45;
    public int recipeID46;
    public int recipeID47;
    public int recipeID48;
    public int recipeID49;
    public int recipeID50;
    public int recipeID51;
    public int recipeID52;
    public int recipeID53;
    public int recipeID54;
    public int recipeID55;
    public int recipeID56;
    public int recipeID57;
    public int recipeID58;
    public int recipeID59;
    public int recipeID60;
    public int recipeID61;
    public int recipeID62;
    public int recipeID63;
    public int recipeID64;
    public int recipeID65;
    public int recipeID66;
    public int recipeID67;
    public int recipeID68;
    public int recipeID69;
    public int recipeID70;
    public int recipeID71;
    public int recipeID72;
    public int recipeID73;
    public int recipeID74;
    public int recipeID75;
    public int recipeID76;
    public int recipeID77;
    public int recipeID78;
    public int recipeID79;
    public int recipeID80;
    public int recipeID81;
    public int recipeID82;
    public int recipeID83;
    public int recipeID84;
    public int recipeID85;
    public int recipeID86;
    public int recipeID87;
    public int recipeID88;
    public int recipeID89;
    public int recipeID90;
    public int recipeID91;
    public int recipeID92;
    public int recipeID93;
    public int recipeID94;
    public int recipeID95;
    public int recipeID96;
    public int recipeID97;
    public int recipeID98;
    public int recipeID99;
    public int recipeID100;
    public int recipeID101;
    public int recipeID102;
    public int recipeID103;
    public int recipeID104;
    public int recipeID105;
    public int recipeID106;
    public int recipeID107;
    public int recipeID108;
    public int recipeID109;
    public int recipeID110;
    public int recipeID111;
    public int recipeID112;
    public int recipeID113;
    public int recipeID114;
    public int recipeID115;
    public int recipeID116;
    public int recipeID117;
    public int recipeID118;
    public int recipeID119;
    public int recipeID120;
    public int recipeID121;
    public int recipeID122;
    public int recipeID123;
    public int recipeID124;
    public int recipeID125;
    public int recipeID126;
    public int recipeID127;
    public int recipeID128;
    public int recipeID129;
    public int recipeID130;
    public int recipeID131;
    public int recipeID132;
    public int recipeID133;
    public int recipeID134;
    public int recipeID135;
    public int recipeID136;
    public int recipeID137;
    public int recipeID138;
    public int recipeID139;
    public int recipeID140;
    public int recipeID141;
    public int recipeID142;
    public int recipeID143;
    public int recipeID144;
    public int recipeID145;
    public int recipeID146;
    public int recipeID147;
    public int recipeID148;
    public int recipeID149;
    public int recipeID150;
    public int recipeID151;
    public int recipeID152;
    public int recipeID153;
    public int recipeID154;
    public int recipeID155;
    public int recipeID156;
    public int recipeID157;
    public int recipeID158;
    public int recipeID159;
    public int recipeID160;
    public int recipeID161;
    public int recipeID162;
    public int recipeID163;
    public int recipeID164;
    public int recipeID165;
    public int recipeID166;
    public int recipeID167;
    public int recipeID168;
    public int recipeID169;
    public int recipeID170;
    public int recipeID171;
    public int recipeID172;
    public int recipeID173;
    public int recipeID174;
    public int recipeID175;
    public int recipeID176;
    public int recipeID177;
    public int recipeID178;
    public int recipeID179;
    public int recipeID180;
    public int recipeID181;
    public int recipeID182;
    public int recipeID183;
    public int recipeID184;
    public int recipeID185;
    public int recipeID186;
    public int recipeID187;
    public int recipeID188;
    public int recipeID189;
    public int recipeID190;
    public int recipeID191;
    public int recipeID192;
    public int recipeID193;
    public int recipeID194;
    public int recipeID195;
    public int recipeID196;
    public int recipeID197;
    public int recipeID198;
    public int recipeID199;
    public int recipeID200;

    public List<int> recipeIDList;

    public List<int> Setup()
    {
        recipeIDList = new List<int>();

        for (int i = 1; i <= 200; i++)
        {
            FieldInfo fieldInfo = typeof(BuildingData).GetField("recipeID" + i);
            int id = (int)fieldInfo.GetValue(this);
            if (id != 0)
                recipeIDList.Add(id);
        }

        return recipeIDList.Count == 0 ? null : recipeIDList;
    }
}
#endregion

#region Customize
[Serializable]
public class CustomizeData : TableData.IData<int>
{
    public enum EType
    {
        None,
        Buy
    }

    public enum EPartsType
    {
        Set,
        Head,
        UpperBody,
        LowerBody
    }

    public enum EGender
    {
        Male,
        Female
    }

    public enum EBoneType
    {
        A,
        B
    }

    public int Id;
    public EBoneType BoneType;
    public EPartsType PartsType;
    public bool DefaultView;
    public EGender Gender;
    public string CharFileName;
    public string ThumbnailFileName;
    public DateTime ViewDate;

    public int Key()
    {
        return Id;
    }
}
#endregion

#region NickNameBan
[Serializable]
public class NickNameBan
{
    public string BanString;
}
#endregion

#region PlayerExp
[Serializable]
public class PlayerExp
{
    public int level;
    public int reqExp;
    public int skillCount;
}
#endregion