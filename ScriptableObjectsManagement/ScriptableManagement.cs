using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace uMMORPG
{    
    public class ScriptableManagement : Editor
    {
        //Table path
        public const string _npcScriptTablePath = "Assets/DataTable/NPCScriptTable.json";
        public const string _playerSkillTablePath = "Assets/DataTable/PlayerSkillTable.json";
        public const string _playerExpTablePath = "Assets/DataTable/PlayerExpTable.json";
        public const string _playerBuffTablePath = "Assets/DataTable/PlayerBuffTable.json";
        public const string _recipeTablePath = "Assets/DataTable/RecipeTable.json";
        public const string _questTablePath = "Assets/DataTable/QuestTable.json";
        public const string _missionTablePath = "Assets/DataTable/MissionTable.json";
        public const string _itemTable_PotionPath = "Assets/DataTable/ItemTable_Potion.json";
        public const string _itemTable_MaterialPath = "Assets/DataTable/ItemTable_Material.json";
        public const string _equipItemTablePath = "Assets/DataTable/EquipItemTable.json";
        public const string _itemTable_SkillPath = "Assets/DataTable/ItemTable_Skill.json";
        public const string _itemTable_BuffPath = "Assets/DataTable/ItemTable_Buff.json";
        public const string _itemTable_DropPath = "Assets/DataTable/ItemTable_Drop.json";
        public const string _itemTable_RBoxPath = "Assets/DataTable/ItemTable_RBox.json";
        public const string _equipmentEnhanceTablePath = "Assets/DataTable/EquipEnhanceTable.json";
        public const string _equipmentEnhanceFeeTablePath = "Assets/DataTable/EquipEnhanceFeeTable.json";
        public const string _equipmentEnhanceChanceTablePath = "Assets/DataTable/EquipEnhanceChanceTable.json";
        public const string _equipUpgradeTablePath = "Assets/DataTable/EquipUpgradeTable.json";
        public const string _equipOptionTablePath = "Assets/DataTable/EquipOptionTable.json";

        public const string _gatherItemTablePath = "Assets/DataTable/GatherItemTable.json";
        public const string _dropTablePath = "Assets/DataTable/DropTable.json";
        public const string _mobTablePath = "Assets/DataTable/MobTable.json";
        public const string _npcTablePath = "Assets/DataTable/NPCTable.json";
        public const string _shopTablePath = "Assets/DataTable/ShopTable.json";
        public const string _craftTablePath = "Assets/DataTable/CraftTable.json";
        public const string _buildingTablePath = "Assets/DataTable/BuildingTable.json";
        public const string _customizeTablePath = "Assets/DataTable/CharacterCustomise.json";
        public const string _banNickNameTablePath = "Assets/DataTable/NickNameBan.json";
        public const string _cashShopTablePath = "Assets/DataTable/CashShopTable.json";

        // asset path
        public const string _stringAssetPath = "Assets/Resources/Blocktopia/Strings";
        public const string _scriptAssetPath = "Assets/Resources/Blocktopia/NPCScripts";
        public const string _skillAssetPath = "Assets/Resources/Blocktopia/Skills";
        public const string _playerExpAssetPath = "Assets/Resources/Blocktopia/PlayerExp";
        public const string _passiveSkillAssetPath = "Assets/Resources/Blocktopia/Skills/Passive";
        public const string _pDmgSkillAssetPath = "Assets/Resources/Blocktopia/Skills/Active/Damage/PDmg";
        public const string _mDmgSkillAssetPath = "Assets/Resources/Blocktopia/Skills/Active/Damage/MDmg";
        public const string _targetBuffSkillAssetPath = "Assets/Resources/Blocktopia/Skills/Active/Buff";
        public const string _buffAssetPath = "Assets/Resources/Blocktopia/Buffs";
        public const string _recipeAssetPath = "Assets/Resources/Blocktopia/Recipes";
        public const string _questAssetPath = "Assets/Resources/Blocktopia/Quests";
        public const string _locationQuestAssetPath = "";
        public const string _missionAssetPath = "Assets/Resources/Blocktopia/Missions";

        // Item
        public const string _potionItemAssetPath = "Assets/Resources/Blocktopia/Items/Potions";
        public const string _materialItemAssetPath = "Assets/Resources/Blocktopia/Items/Materials";
        public const string _equipmentItemAssetPath = "Assets/Resources/Blocktopia/Items/Equipments";
        public const string _skillItemAssetPath = "Assets/Resources/Blocktopia/Items/Skills";
        public const string _buffItemAssetPath = "Assets/Resources/Blocktopia/Items/Buffs";
        public const string _dropItemAssetPath = "Assets/Resources/Blocktopia/Items/Drops";
        public const string _rBoxItemAssetPath = "Assets/Resources/Blocktopia/Items/RBoxes";
        public const string _equipEnhanceAssetPath = "Assets/Resources/Blocktopia/Items/EquipEnhance";

        public const string _gatherItemAssetPath = "Assets/Resources/Blocktopia/GatherItems";
        public const string _dropDataAssetPath = "Assets/Resources/Blocktopia/DropDatas";
        public const string _monsterCharacterAssetPath = "Assets/Resources/Blocktopia/Characters/Monsters";
        public const string _npcAssetPath = "Assets/Resources/Blocktopia/NPCs";
        public const string _buildingAssetPath = "Assets/Resources/Blocktopia/Structures/Buildings";
        public const string _decorationAssetPath = "Assets/Resources/Blocktopia/Structures/Decorations";
        public const string _customizeDataAssetPath = "Assets/Resources/Blocktopia/Customize";
        public const string _banNickNameAssetPath = "Assets/Resources/Blocktopia/NicknameBanList";
        public const string _cashShopAssetPath = "Assets/Resources/Blocktopia/CashShopItems";

        [MenuItem("Tools/Blocktopia/Update All")]
        public static void UpdataAll()
        {
            UpdateString();
            UpdateNPCScript();
            UpdateSkillAndBuff();
            UpdateRecipe();
            UpdateItem();
            UpdateCashShop();
            UpdateGatherItems();
            UpdateDropDatas();
            UpdateMonster();
            UpdateNPC();
            UpdateBuilding();
            //UpdateDecoration();
            UpdateQuest();
            //UpdateLocationQuest();
            UpdateMission();
            UpdateCustomizeData();
            UpdateNickNameBan();
            UpdatePlayerExp();
        }

        static void ClearDirectory(string assetPath)
        {
            foreach (var asset in AssetDatabase.FindAssets("", new string[]{ assetPath }))
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
            }
        }

        #region String
        [MenuItem("Tools/Blocktopia/Update Individually/String/Update All")]
        public static void UpdateString()
        {
            UpdateString_Gathering();
            UpdateString_GUI();
            UpdateString_Item();
            UpdateString_Mate();
            UpdateString_Monster();
            UpdateString_NPC();
            UpdateString_Object();
            UpdateString_Quest();
            UpdateString_Script();
            UpdateString_Skill();
            UpdateString_Tip();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/String/StringTable_Gathering")]
        public static void UpdateString_Gathering()
        {
            ClearDirectory(_stringAssetPath + "/Gathering");
            LoadStringTable("StringTable_Gathering");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/String/StringTable_GUI")]
        public static void UpdateString_GUI()
        {
            ClearDirectory(_stringAssetPath + "/GUI");
            LoadStringTable("StringTable_GUI");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/String/StringTable_Item")]
        public static void UpdateString_Item()
        {
            ClearDirectory(_stringAssetPath + "/Item");
            LoadStringTable("StringTable_Item");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/String/StringTable_Mate")]
        public static void UpdateString_Mate()
        {
            ClearDirectory(_stringAssetPath + "/Mate");
            LoadStringTable("StringTable_Mate");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/String/StringTable_Monster")]
        public static void UpdateString_Monster()
        {
            ClearDirectory(_stringAssetPath + "/Monster");
            LoadStringTable("StringTable_Monster");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/String/StringTable_NPC")]
        public static void UpdateString_NPC()
        {
            ClearDirectory(_stringAssetPath + "/NPC");
            LoadStringTable("StringTable_NPC");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/String/StringTable_Object")]
        public static void UpdateString_Object()
        {
            ClearDirectory(_stringAssetPath + "/Object");
            LoadStringTable("StringTable_Object");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/String/StringTable_Quest")]
        public static void UpdateString_Quest()
        {
            ClearDirectory(_stringAssetPath + "/Quest");
            LoadStringTable("StringTable_Quest");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/String/StringTable_Script")]
        public static void UpdateString_Script()
        {
            ClearDirectory(_stringAssetPath + "/Script");
            LoadStringTable("StringTable_Script");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/String/StringTable_Skill")]
        public static void UpdateString_Skill()
        {
            ClearDirectory(_stringAssetPath + "/Skill");
            LoadStringTable("StringTable_Skill");
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/String/StringTable_Tip")]
        public static void UpdateString_Tip()
        {
            ClearDirectory(_stringAssetPath + "/Tip");
            LoadStringTable("StringTable_Tip");
            AssetDatabase.Refresh();
        }

        static void LoadStringTable(string fileName)
        {
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath($"Assets/DataTable/{fileName}.json", typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<StringData>>(textAsset.text);
            var category = fileName.Split('_')[1];

            for (int i = 0; i < data.array.Length; i++)
            {
                StringData stringData = data.array[i];
                ScriptableString asset = CreateInstance<ScriptableString>();

                asset.stringId = stringData.stringId;
                asset.ko = stringData.ko;
                asset.en = stringData.en;
                asset.jp = stringData.jp;
                asset.fr = stringData.fr;
                asset.gr = stringData.gr;
                
                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_stringAssetPath}/{category}/String_{stringData.stringId}.asset");
                AssetDatabase.SaveAssets();
            }
        }
        #endregion

        #region NPC Script
        [MenuItem("Tools/Blocktopia/Update Individually/NPCScriptTable")]
        public static void UpdateNPCScript()
        {
            ClearDirectory(_scriptAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_npcScriptTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<NpcScriptData>>(textAsset.text);

            Dictionary<int, ScriptableNPCScript> pairs = new Dictionary<int, ScriptableNPCScript>();
            for (int i = 0; i < data.array.Length; i++)
            {
                NpcScriptData sData = data.array[i];

                if (pairs.TryGetValue(sData.ScriptSetID, out ScriptableNPCScript scriptableNPCScript))
                {
                    ScriptableNPCScript asset = CreateInstance<ScriptableNPCScript>();

                    asset.scriptSetID = sData.ScriptSetID;
                    asset.scriptDatas = new List<NpcScriptData>();
                    asset.scriptDatas.AddRange(scriptableNPCScript.scriptDatas);
                    asset.scriptDatas.Add(sData);

                    pairs[sData.ScriptSetID] = asset;
                    //Debug.Log($"Add Asset {asset.scriptSetID} {asset.scriptDatas.Count}");
                }
                else
                {
                    ScriptableNPCScript asset = CreateInstance<ScriptableNPCScript>();

                    asset.scriptSetID = sData.ScriptSetID;
                    asset.scriptDatas = new List<NpcScriptData>() { sData };

                    pairs.Add(sData.ScriptSetID, asset);
                    //Debug.Log($"Create Asset {asset.scriptSetID} {asset.scriptDatas.Count}");
                }
            }

            foreach (var pair in pairs)
            {
                var asset = pair.Value;

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_scriptAssetPath}/ScriptSet_{asset.scriptSetID}.asset");
                AssetDatabase.SaveAssets();
                //Debug.Log($"Save Asset {asset.scriptSetID} {asset.scriptDatas.Count}");
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #region Skill/Buff
        [MenuItem("Tools/Blocktopia/Update Individually/Skill&Buff/Update All")]
        public static void UpdateSkillAndBuff()
        {
            UpdateBuff();
            UpdateSkill();
        }

        #region Skill

        [MenuItem("Tools/Blocktopia/Update Individually/Skill&Buff/PlayerSkillTable")]
        public static void UpdateSkill()
        {
            //ClearDirectory(_passiveSkillAssetPath);
            //ClearDirectory(_pDmgSkillAssetPath);
            //ClearDirectory(_mDmgSkillAssetPath);
            ClearDirectory(_skillAssetPath);

            

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_playerSkillTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<BT_Skill>>(textAsset.text);

            int cur = 0;
            try
            {
                for (int i = 0; i < data.array.Length; i++)
                {
                    var item = data.array[i];

                    var effectType = item.effectType;

                    var skillName = $"Skill_{ScriptableSkill.GetSkillid(item.skillID, item.skillLevel)}";// $"{item.skillID}{lastNum}";               

                    var asset = CreateInstance(effectType) as ScriptableSkill;

                    asset.originId = item.skillID;
                    asset.skillId = ScriptableSkill.GetSkillid(item.skillID, item.skillLevel);// int.Parse($"{item.skillID}{item.skillLevel}");
                    asset.skillLevel = item.skillLevel;
                    asset.requiredLevel = item.reqLv;
                    asset.requiredWeaponCategory = String.IsNullOrEmpty(item.reqWeapon) ? "" : $"{((WeaponType)Enum.Parse(typeof(WeaponType), item.reqWeapon))}";
                    asset.reqWeapon = int.Parse(String.IsNullOrEmpty(item.reqWeapon) ? "0" : item.reqWeapon);
                    asset.manaCosts = item.mP;
                    asset.castTime = item.castTime;

                    asset.effectType = item.effectType;
                    asset.skillType = (SkillType)Enum.Parse(typeof(SkillType), item.skillType);
                    asset.cooldown = item.cooldown;
                    asset.castRange = item.castRange;

                    asset.reqSkill1 = item.reqSkillID1;
                    asset.reqSkillLv1 = item.reqSkillLv1;
                    asset.reqSkill2 = item.reqSkillID2;
                    asset.reqSkill3 = item.reqSkillID3;

                    asset.attribute = item.attribute;

                    asset.animationName = item.animation;

                    asset.atkMuls = item.atkMulS;
                    asset.atkAddS = item.atkAddS;

                    asset.image = BT_ResManager.ResLoad<Sprite>($"Sprite/PlayerSkillIcon/PlayerSkillIcon_{item.skillID}");

                    if (asset is TargetProjectileSkill)
                        ((TargetProjectileSkill)asset).projectile = BT_ResManager.ResLoad<ProjectileSkillEffect>("Blocktopia/Prefabs/Arrow");

                    asset.buffID = item.buffID;

                    AssetDatabase.CreateAsset(asset, $"{_skillAssetPath}/{skillName}.asset");

                    AssetDatabase.SaveAssets();

                    cur++;
                }
            }
            catch
            {
                Debug.LogError($"{cur}번째 실패");
            }

            AssetDatabase.Refresh();
        }

        string SetAsset(string effectType)
        {
            return effectType;
        }
        
        #endregion

        #region Buff
        [MenuItem("Tools/Blocktopia/Update Individually/Skill&Buff/PlayerBuffTable")]
        public static void UpdateBuff()
        {
            ClearDirectory(_buffAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_playerBuffTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<BT_Buff>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                var asset = CreateInstance<ScriptableBuff>();

                asset.buffID = item.buffID;
                asset.buffType = item.buffType;
                asset.effectType = item.effectType;
                asset.duration = item.duration;

                asset.paramType1 = item.paramType1;
                asset.paramType2 = item.paramType2;
                asset.paramType3 = item.paramType3;
                asset.paramType4 = item.paramType4;
                asset.paramType5 = item.paramType5;

                asset.paramAmount1 = item.paramAmount1;
                asset.paramAmount2 = item.paramAmount2;
                asset.paramAmount3 = item.paramAmount3;
                asset.paramAmount4 = item.paramAmount4;
                asset.paramAmount5 = item.paramAmount5;

                AssetDatabase.CreateAsset(asset, $"{_buffAssetPath}/Buff_{item.buffID}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #endregion

        #region Recipe
        [MenuItem("Tools/Blocktopia/Update Individually/RecipeTable")]
        public static void UpdateRecipe()
        {
            ClearDirectory(_recipeAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_recipeTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<RecipeData>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                RecipeData recipeData = data.array[i];
                ScriptableRecipe asset = CreateInstance<ScriptableRecipe>();

                asset.typeID = recipeData.typeID;
                asset.unlockLevel = recipeData.unlockLevel;
                asset.outputID = recipeData.outputID;
                asset.outputCount = recipeData.outputCount;
                asset.makeTime = recipeData.makeTime;
                asset.golds = recipeData.golds;
                asset.skillInfos = recipeData.SetupSkill();
                asset.matInfos = recipeData.Setup();

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_recipeAssetPath}/Recipe_{recipeData.typeID}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #region Quest
        [MenuItem("Tools/Blocktopia/Update Individually/QuestTable")]
        public static void UpdateQuest()
        {
            ClearDirectory(_questAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_questTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<QuestData>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                try
                {
                    var quest = data.array[i];

                    ScriptableQuest asset = null;

                    switch (quest.questConditionType)
                    {
                        //대상과 대화
                        case "Conver":
                            asset = CreateInstance<ConverQuest>();
                            break;

                        //아이템 보유
                        case "Gather":
                            asset = CreateInstance<GatherQuest>();
                            break;
                        //건설
                        case "Build":
                            asset = CreateInstance<BuildQuest>();
                            break;
                        //사냥
                        case "Hunt":
                            asset = CreateInstance<HuntQuest>();
                            //// ★임시코드(몬스터 작업 필요)
                            //((HuntQuest)asset).huntTarget = Resources.Load<Monster>("Blocktopia/Monsters/Bandit Boss");
                            //((HuntQuest)asset).huntAmount = quest.progressCount;
                            break;
                        //친구 섬 방문
                        case "VisitFriend":
                            asset = CreateInstance<VisitQuest>();
                            break;
                        //친구신청
                        case "AddFriend":
                            asset = CreateInstance<AddFriendQuest>();
                            break;
                        //야생섬 방문
                        case "Visit":
                            asset = CreateInstance<VisitQuest>();
                            break;
                        //오픈필드의 해당 지역 방문
                        case "Teleport":
                            asset = CreateInstance<VisitQuest>();
                            break;
                        //해당 아이템을 판매
                        case "Sell":
                            asset = CreateInstance<SellQuest>();
                            break;
                        //캐릭터 강화
                        case "Enforcement":
                            asset = CreateInstance<EnforceQuest>();
                            break;
                        //채팅 글자 입력
                        case "Chat":
                            asset = CreateInstance<ChatQuest>();
                            break;
                        //채팅창에 이모티콘을 입력
                        case "Emoji":
                            asset = CreateInstance<EmojiQuest>();
                            break;
                        //친구 도와주기
                        case "Help":
                            asset = CreateInstance<HelpQuest>();
                            break;
                        //친구섬에 건설재료 주기
                        case "Give":
                            asset = CreateInstance<GiveQuest>();
                            break;
                        //친구 섬에 내 캐릭터를 배치
                        case "Set":
                            asset = CreateInstance<SetQuest>();
                            break;
                    }

                    if (asset == null)
                    {
                        Debug.LogError($"잘못된 퀘스트 타입 : {quest.questConditionType}");
                        continue;
                    }

                    asset.questId = quest.questId;

                    asset.playerMinLevel = quest.playerMinLevel;
                    asset.playerMaxLevel = quest.playerMaxLevel;

                    asset.questTargetId = quest.questTargetId;
                    asset.progressCount = quest.progressCount;

                    asset.previousQuestId = quest.previousQuestId;

                    asset.exp = quest.exp;
                    asset.gem = quest.gem;
                    asset.gold = quest.gold;

                    asset.item1 = quest.item1Id;
                    asset.item1Count = quest.item1Count;
                    asset.item2 = quest.item2Id;
                    asset.item2Count = quest.item2Count;
                    asset.item3 = quest.item3Id;
                    asset.item3Count = quest.item3Count;
                    asset.item4 = quest.item4Id;
                    asset.item4Count = quest.item4Count;


                    asset.startNPC = quest.startNPC;
                    asset.endNPC = quest.endNPC;


                    // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                    AssetDatabase.CreateAsset(asset, $"{_questAssetPath}/Quest_{quest.questId}.asset");
                    AssetDatabase.SaveAssets();
                }
                catch
                {
                    Debug.Log("I번째 에서 오류");
                }
                
            }

            AssetDatabase.Refresh();
        }
        //[MenuItem("Tools/Blocktopia/Update Individually/Quest/Location Quest Update")]
        //public static void UpdateLocationQuest()
        //{

        //}
        #endregion

        #region Mission
        [MenuItem("Tools/Blocktopia/Update Individually/MissionTable")]
        public static void UpdateMission()
        {
            ClearDirectory(_missionAssetPath);



            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_missionTablePath, typeof(TextAsset)) as TextAsset;

            TableData.SerializeObj<MissionData> data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<MissionData>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                var mission = data.array[i];

                ScriptableMission asset = null;

                switch (mission.MissionConditionType)
                {
                    case "Hunt":
                        asset = CreateInstance<HuntMission>();
                        // ★임시코드(몬스터 작업 필요)
                        //((HuntMission)asset).huntTarget = Resources.Load<Monster>("Blocktopia/Monsters/Bandit Boss");
                        break;

                    case "Gather":
                        asset = CreateInstance<GatherMission>();
                        break;

                    case "Build":
                        asset = CreateInstance<BuildMission>();
                        break;
                }

                if (asset == null)
                {
                    Debug.LogError($"잘못된 미션 타입 : {mission.MissionConditionType}");
                    continue;
                }

                asset.MissionID = mission.MissionID;
                asset.Group = (MissionGroup)mission.Group;
                asset.Type = (MissionType)mission.Type;

                asset.PlayerMinLevel = mission.PlayerMinLevel;
                asset.PlayerMaxLevel = mission.PlayerMaxLevel;

                asset.ProduceSkill1 = mission.GetProduceSkill1();
                asset.ProduceReqLevel1 = mission.GetProduceReqLevel1();

                asset.ProduceSkill2 = mission.GetProduceSkill2();
                asset.ProduceReqLevel2 = mission.GetProduceReqLevel2();

                asset.ProduceSkill3 = mission.GetProduceSkill3();
                asset.ProduceReqLevel3 = mission.GetProduceReqLevel3();

                asset.MissionConditionType = mission.MissionConditionType;
                asset.MissionTargetId = mission.MissionTargetId;
                asset.ProgressCount = mission.ProgressCount;


                asset.Exp = mission.Exp;

                asset.Skill = mission.GetSkill();
                asset.SkillExp = mission.GetSkillExp();

                asset.Gem = mission.GetGem();

                asset.Gold = mission.GetGold();


                asset.item1 = mission.item1Id;
                asset.item1Count = mission.item1Count;

                asset.item2 = mission.item2Id;
                asset.item2Count = mission.item2Count;

                asset.item3 = mission.item3Id;
                asset.item3Count = mission.item3Count;

                asset.item4 = mission.item4Id;
                asset.item4Count = mission.item4Count;


                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_missionAssetPath}/Mission_{mission.MissionID}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #region Item
        [MenuItem("Tools/Blocktopia/Update Individually/Item/Update All")]
        public static void UpdateItem()
        {
            UpdatePosionItem();
            UpdateMaterialItem();
            UpdateEquipmentItem();
            UpdateSkillItem();
            UpdateBuffItem();
            UpdateDropItem();
            UpdateRBoxItem();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/Item/ItemTable_Potion")]
        public static void UpdatePosionItem()
        {
            ClearDirectory(_potionItemAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_itemTable_PotionPath, typeof(TextAsset)) as TextAsset; ;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<BT_Item_Potion>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                var asset = CreateInstance<PotionItem>();

                asset.itemId = item.itemId;
                asset.maxStack = item.maxStack;
                asset.priceGold = item.priceGold;
                asset.priceGem = item.priceGem;
                asset.sellPrice = item.sellPrice;
                asset.sellable = item.sellable;
                asset.tradable = item.tradable;
                asset.destroyable = item.destroyable;

                asset.category = item.category;
                asset.cooldown = item.cooldown;
                asset.usageMana = item.usageMana;
                asset.usageHealth = item.usageHealth;

                string path = "Assets/Res/Blocktopia/UI/ItemIcon/item_" + item.itemId + ".png";
                Sprite spr = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
                if (spr != null)
                    asset.image = spr;
                else
                    Debug.LogError($"아이템 아이콘이 존재하지 않습니다. 경로 : {path}");

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_potionItemAssetPath}/Item_{item.itemId}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/Item/ItemTable_Material")]
        public static void UpdateMaterialItem()
        {
            ClearDirectory(_materialItemAssetPath);

            TextAsset itemTable = AssetDatabase.LoadAssetAtPath(_itemTable_MaterialPath, typeof(TextAsset)) as TextAsset;
            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<BT_Item_Material>>(itemTable.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                var asset = CreateInstance<MaterialItem>();

                asset.itemId = item.itemId;
                asset.maxStack = item.maxStack;
                asset.priceGold = item.priceGold;
                asset.priceGem = item.priceGem;
                asset.sellPrice = item.sellPrice;
                asset.sellable = item.sellable;
                asset.tradable = item.tradable;
                asset.destroyable = item.destroyable;

                string path = "Assets/Res/Blocktopia/UI/ItemIcon/item_" + item.itemId + ".png";
                Sprite spr = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
                if (spr != null)
                    asset.image = spr;
                else
                    Debug.LogError($"아이템 아이콘이 존재하지 않습니다. 경로 : {path}");

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_materialItemAssetPath}/Item_{item.itemId}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/Item/ItemTable_Equipment")]
        public static void UpdateEquipmentItem()
        {
            ClearDirectory(_equipmentItemAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_equipItemTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<BT_Item_Equipment>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                EquipmentItem asset = CreateInstance<EquipmentItem>();

                switch ((EquipmentCategory)item.equipPart)
                {
                    case EquipmentCategory.Weapon:
                        asset = CreateInstance<WeaponItem>();
                        ((WeaponItem)asset).weaponType = (WeaponType)item.weaponType;
                        SetWeaponPrefab((WeaponType)item.weaponType, ref asset);
                        break;

                    case EquipmentCategory.Helmet:
                    case EquipmentCategory.Armour:
                    case EquipmentCategory.Gloves:
                    case EquipmentCategory.Belt:
                    case EquipmentCategory.Shoes:
                        asset = CreateInstance<ArmorItem>();
                        ((ArmorItem)asset).armorType = (ArmorType)item.weaponType;
                        break;
                    case EquipmentCategory.Accessories:
                        asset = CreateInstance<EquipmentItem>();
                        break;

                    default:
                        Debug.LogError($"존재하지 않는 장비 파츠 : {item.equipPart}");
                        return;
                }

                asset.itemId = item.itemId;
                asset.maxStack = 1;
                asset.priceGold = item.priceGold;
                asset.priceGem = item.priceGem;
                asset.sellPrice = item.sellPrice;
                asset.sellable = item.sellable;
                asset.tradable = item.tradable;
                asset.destroyable = item.destroyable;

                asset.category = (EquipmentCategory)item.equipPart;

                asset.reqLv = item.reqLv;

                asset.pAtkE = item.pAtkE;
                asset.mAtkE = item.mAtkE;
                asset.pDefE = item.pDefAddE;
                asset.mDefE = item.mDefAddE;
                asset.blkChanceE = item.blkChanceE;
                asset.atkCclE = item.atkCclE;
                asset.skillID = item.skillID;

                string path = "Assets/Res/Blocktopia/UI/ItemIcon/item_" + item.itemId + ".png";
                Sprite spr = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
                if (spr != null)
                    asset.image = spr;
                else
                    Debug.LogError($"아이템 아이콘이 존재하지 않습니다. 경로 : {path}");

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_equipmentItemAssetPath}/Item_{item.itemId}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();

            void SetWeaponPrefab(WeaponType type, ref EquipmentItem eItem)
            {
                string path = "Assets/ExternalLib/uMMORPG/Prefabs/ItemModels/";
                switch (type)
                {
                    case WeaponType.OneHandAndShield:
                    case WeaponType.DualSword:
                    case WeaponType.TwoHandedSword:
                        path += "Blacksmiths Sword.prefab";
                        break;

                    case WeaponType.Bow:
                    case WeaponType.Staff:
                        path += "White Bow.prefab";
                        break;
                }

                eItem.modelPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            }
        }


        [MenuItem("Tools/Blocktopia/Update Individually/Item/ItemTable_Skill")]
        public static void UpdateSkillItem()
        {
            ClearDirectory(_skillItemAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_itemTable_SkillPath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<BT_Item_Skill>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                var asset = CreateInstance<SkillItem>();

                asset.itemId = item.itemId;
                asset.maxStack = item.maxStack;
                asset.priceGold = item.priceGold;
                asset.priceGem = item.priceGem;
                asset.sellPrice = item.sellPrice;
                asset.sellable = item.sellable;
                asset.tradable = item.tradable;
                asset.destroyable = item.destroyable;

                asset.skillId = item.skillId;

                string path = "Assets/Res/Blocktopia/UI/ItemIcon/item_" + item.itemId + ".png";
                Sprite spr = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
                if (spr != null)
                    asset.image = spr;
                else
                    Debug.LogError($"아이템 아이콘이 존재하지 않습니다. 경로 : {path}");

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_skillItemAssetPath}/Item_{item.itemId}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/Item/ItemTable_Buff")]
        public static void UpdateBuffItem()
        {
            ClearDirectory(_buffItemAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_itemTable_BuffPath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<BT_Item_Buff>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                var asset = CreateInstance<BuffItem>();

                asset.itemId = item.itemId;
                asset.maxStack = item.maxStack;
                asset.priceGold = item.priceGold;
                asset.priceGem = item.priceGem;
                asset.sellPrice = item.sellPrice;
                asset.sellable = item.sellable;
                asset.tradable = item.tradable;
                asset.destroyable = item.destroyable;

                asset.category = item.category;
                asset.skillId = item.skillId;
                asset.skillLevel = item.skillLevel;

                string path = "Assets/Res/Blocktopia/UI/ItemIcon/item_" + item.itemId + ".png";
                Sprite spr = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
                if (spr != null)
                    asset.image = spr;
                else
                    Debug.LogError($"아이템 아이콘이 존재하지 않습니다. 경로 : {path}");

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_buffItemAssetPath}/Item_{item.itemId}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/Item/ItemTable_Drop")]
        public static void UpdateDropItem()
        {
            ClearDirectory(_dropItemAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_itemTable_DropPath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<BT_Item_Drop>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                var asset = CreateInstance<DropItem>();

                asset.itemId = item.itemId;
                asset.maxStack = item.maxStack;
                asset.priceGold = item.priceGold;
                asset.priceGem = item.priceGem;
                asset.sellPrice = item.sellPrice;
                asset.sellable = false;
                asset.tradable = false;
                asset.destroyable = false;

                asset.dropId = item.dropId;
                asset.dropCount = item.dropCount;

                string path = "Assets/Res/Blocktopia/UI/ItemIcon/item_" + item.itemId + ".png";
                Sprite spr = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
                if (spr != null)
                    asset.image = spr;
                else
                    Debug.LogError($"아이템 아이콘이 존재하지 않습니다. 경로 : {path}");

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_dropItemAssetPath}/Item_{item.itemId}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/Item/ItemTable_RBox")]
        public static void UpdateRBoxItem()
        {
            ClearDirectory(_rBoxItemAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_itemTable_RBoxPath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<BT_Item_RBox>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                var asset = CreateInstance<RBoxItem>();

                asset.itemId = item.itemId;
                asset.maxStack = item.maxStack;
                asset.priceGold = item.priceGold;
                asset.priceGem = item.priceGem;
                asset.sellPrice = item.sellPrice;
                asset.sellable = false;
                asset.tradable = false;
                asset.destroyable = false;

                asset.category = item.category;
                asset.useType = item.useType;
                asset.type = item.type;
                asset.RBoxGroupId = item.RBoxGroupId;

                string path = "Assets/Res/Blocktopia/UI/ItemIcon/item_" + item.itemId + ".png";
                Sprite spr = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
                if (spr != null)
                    asset.image = spr;
                else
                    Debug.LogError($"아이템 아이콘이 존재하지 않습니다. 경로 : {path}");

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_rBoxItemAssetPath}/Item_{item.itemId}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/Item/EquipEnhanceTable")]
        public static void UpdateEquipmentEnhance()
        {
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_equipmentEnhanceTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<EquipEnhance>>(textAsset.text);

            var asset = CreateInstance<ScriptableEquipEnhance>();
            asset.enhanceTable = new List<EquipEnhance>();
            for(int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                asset.enhanceTable.Add(new EquipEnhance
                {
                    equipPart = item.equipPart,
                    weaponType = item.weaponType,
                    enhance1 = item.enhance1,
                    enhance2 = item.enhance2,
                    enhance3 = item.enhance3,
                    enhance4 = item.enhance4,
                    enhance5 = item.enhance5,
                    enhance6 = item.enhance6,
                    enhance7 = item.enhance7,
                    enhance8 = item.enhance8,
                    enhance9 = item.enhance9,
                    enhance10 = item.enhance10,
                    enhance11 = item.enhance11,
                    enhance12 = item.enhance12,
                    enhance13 = item.enhance13,
                    enhance14 = item.enhance14,
                    enhance15 = item.enhance15,
                    enhance16 = item.enhance16,
                    enhance17 = item.enhance17,
                    enhance18 = item.enhance18,
                    enhance19 = item.enhance19,
                    enhance20 = item.enhance20
                });

            }
            // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
            AssetDatabase.CreateAsset(asset, $"{_equipEnhanceAssetPath}/EquipEnhanceTable.asset");
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/Item/EquipEnhanceFeeTable")]
        public static void UpdateEquipEnhanceFee()
        {
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_equipmentEnhanceFeeTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<EquipEnhanceFee>>(textAsset.text);

            var asset = CreateInstance<ScriptableEquipEnhanceFee>();
            asset.enhanceFeeTable = new List<EquipEnhanceFee>();
            for (int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                asset.enhanceFeeTable.Add(new EquipEnhanceFee
                {
                    reqLvMin = item.reqLvMin,
                    reqLvMax = item.reqLvMax,
                    rarity = item.rarity,
                    gold = item.gold
                });
            }
            // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
            AssetDatabase.CreateAsset(asset, $"{_equipEnhanceAssetPath}/EquipEnhanceFeeTable.asset");
            AssetDatabase.SaveAssets();
        }
        [MenuItem("Tools/Blocktopia/Update Individually/Item/EquipEnhanceChanceTable")]
        public static void UpdateEquipEnhanceChance()
        {
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_equipmentEnhanceChanceTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<EquipEnhanceChance>>(textAsset.text);

            var asset = CreateInstance<ScriptableEquipEnhanceChance>();
            asset.enhanceChanceTable = new List<EquipEnhanceChance>();
            for (int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                asset.enhanceChanceTable.Add(new EquipEnhanceChance
                {
                    index = item.index,
                    success = item.success,
                    retain = item.retain,
                    down = item.down,
                    crash = item.crash,
                });
            }
            // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
            AssetDatabase.CreateAsset(asset, $"{_equipEnhanceAssetPath}/EquipEnhanceChanceTable.asset");
            AssetDatabase.SaveAssets();
        }
        [MenuItem("Tools/Blocktopia/Update Individually/Item/EquipUpgradeTable")]
        public static void UpdateEquipUpgrade()
        {
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_equipUpgradeTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<EquipUpgrade>>(textAsset.text);

            var asset = CreateInstance<ScriptableEquipUpgrade>();
            asset.upgradeTable = new List<EquipUpgrade>();
            for(int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                asset.upgradeTable.Add(new EquipUpgrade
                {
                    reqLvMin = item.reqLvMin,
                    reqLvMax = item.reqLvMax,
                    equipPart = item.equipPart,
                    weaponType = item.weaponType,
                    rarity = item.rarity,
                    material1 = item.material1,
                    materialCount1 = item.materialCount1,
                    material2 = item.material2,
                    materialCount2 = item.materialCount2,
                    material3 = item.material3,
                    materialCount3 = item.materialCount3,
                    material4 = item.material4,
                    materialCount4 = item.materialCount4,
                    material5 = item.material5,
                    materialCount5 = item.materialCount5
                });
            }
            AssetDatabase.CreateAsset(asset, $"{_equipEnhanceAssetPath}/EquipUpgradeTable.asset");
            AssetDatabase.SaveAssets();
        }

        [MenuItem("Tools/Blocktopia/Update Individually/Item/EquipOptionTable")]
        public static void UpdateEquipOption()
        {
            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_equipOptionTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<EquipOption>>(textAsset.text);

            var asset = CreateInstance<ScriptableEquipOption>();
            asset.options = new List<EquipOption>();
            for (int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                asset.options.Add(new EquipOption
                {
                    optionID = item.optionID,
                    reqLvMin = item.reqLvMin,
                    reqLvMax = item.reqLvMax,
                    equipPart = item.equipPart,
                    weaponType = item.weaponType,
                    optionType = item.optionType,
                    optionAmount = item.optionAmount
                });
            }
            AssetDatabase.CreateAsset(asset, $"{_equipEnhanceAssetPath}/EquipOptionTable.asset");
            AssetDatabase.SaveAssets();
        }
        #endregion

        #region Cash Shop
        [MenuItem("Tools/Blocktopia/Update Individually/CashShop")]
        public static void UpdateCashShop()
        {
            ClearDirectory(_cashShopAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_cashShopTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<BT_Cash_Item>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                var item = data.array[i];

                ScriptableCashShopItem asset = null;

                switch (item.shopType)
                {
                    case BT_Cash_Item.ShopType.Gem:
                        asset = CreateInstance<ScriptableGemItem>();
                        break;

                    case BT_Cash_Item.ShopType.Gold:
                        asset = CreateInstance<ScriptableGoldItem>();
                        break;

                    default:
                        Debug.LogError($"올바르지 않은 타입의 아이템입니다. : {item.shopType}");
                        continue;
                }

                asset.typeId = item.typeID;
                asset.shopType = item.shopType;
                asset.itemValue = item.itemValue;
                asset.costType = item.costType;
                asset.costValue = item.costValue;

                string path = item.imagePath + ".png";
                Sprite spr = (Sprite)AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
                if (spr != null)
                    asset.image = spr;
                else
                    Debug.LogError($"아이템 아이콘이 존재하지 않습니다. 경로 : {path}");

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_cashShopAssetPath}/CashItem_{item.typeID}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
        #endregion

        
        #region GatherItem
        [MenuItem("Tools/Blocktopia/Update Individually/GatherItemTable")]
        public static void UpdateGatherItems()
        {
            ClearDirectory(_gatherItemAssetPath);

            TextAsset textAsset_gatherItem = AssetDatabase.LoadAssetAtPath(_gatherItemTablePath, typeof(TextAsset)) as TextAsset;

            var gatherItemData = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<GatherItemData>>(textAsset_gatherItem.text);
            for (int i = 0; i < gatherItemData.array.Length; i++)
            {
                var item = gatherItemData.array[i];

                var asset = CreateInstance<ScriptableGatherItem>();

                asset.typeId = item.typeID;
                asset.gatherAction = (GatherAction)Enum.Parse(typeof(GatherAction), item.gatherAction.ToUpper());
                asset.requiredAbility = item.requiredAbility;
                asset.maxHP = item.maxHP;


                string skillPath = "Assets/ExternalLib/uMMORPG/Resources/Skills, Buffs, Status Effects/Player/GatherSkills/" + item.gatherAction + "Skill.asset";
                GatherSkill gatherSkill = (GatherSkill)AssetDatabase.LoadAssetAtPath(skillPath, typeof(GatherSkill));
                if (gatherSkill != null)
                    asset.reqGatherSkill = gatherSkill;
                else
                    Debug.LogError("해당 채집 스킬이 존재하지 않습니다 : " + item.gatherAction + "Skill");

                asset.exp = item.exp;
                asset.dropID1 = item.dropID1;
                asset.dropCount1 = item.dropCount1;
                asset.dropID2 = item.dropID2;
                asset.dropCount2 = item.dropCount2;
                asset.dropID3 = item.dropID3;
                asset.dropCount3 = item.dropCount3;

                string path = "Assets/Res/Blocktopia/Models/GatherItems/Models/" + item.prefabName + ".fbx";
                GameObject modeling = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                if (modeling != null)
                    asset.modeling = modeling;
                else
                    Debug.LogError("존재하지 않는 모델링 : " + path);

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_gatherItemAssetPath}/GatherItem_{item.typeID}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #region DropData
        [MenuItem("Tools/Blocktopia/Update Individually/DropTable")]
        public static void UpdateDropDatas()
        {
            ClearDirectory(_dropDataAssetPath);

            TextAsset textAsset_dropTable = AssetDatabase.LoadAssetAtPath(_dropTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<DropData>>(textAsset_dropTable.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                DropData dropData = data.array[i];
                ScriptableDrop asset = CreateInstance<ScriptableDrop>();

                asset.dropId = dropData.dropID;
                asset.dropData = dropData.GetDropDataStructs();

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_dropDataAssetPath}/Drop_{dropData.dropID}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #region Monster
        [MenuItem("Tools/Blocktopia/Update Individually/MobTable")]
        public static void UpdateMonster()
        {
            ClearDirectory(_monsterCharacterAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_mobTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<MobData>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                MobData mobData = data.array[i];
                ScriptableMonster asset = CreateInstance<ScriptableMonster>();

                asset.typeID = mobData.typeID;
                asset.level = mobData.level;

                asset.pAtk = mobData.pAtk;
                asset.mAtk = mobData.mAtk;
                asset.pDef = mobData.pDef;
                asset.mDef = mobData.mDef;
                asset.hit = mobData.hit;
                asset.ddg = mobData.ddg;
                asset.cri = mobData.cri;
                asset.mPen = mobData.mPen;
                asset.mRes = mobData.mRes;
                asset.aSpd = mobData.aSpd;
                asset.hp = mobData.hp;

                asset.exp = mobData.exp;
                asset.dropID1 = mobData.dropID1;
                asset.dropCount1 = mobData.dropCount1;
                asset.dropID2 = mobData.dropID2;
                asset.dropCount2 = mobData.dropCount2;
                asset.dropID3 = mobData.dropID3;
                asset.dropCount3 = mobData.dropCount3;

                asset.modelPrefab = Resources.Load<GameObject>($"UpdateResource/Modeling/Prefab/Monster/{mobData.modelName}");

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_monsterCharacterAssetPath}/Mob_{mobData.typeID}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #region NPC
        [MenuItem("Tools/Blocktopia/Update Individually/NPCTable")]
        public static void UpdateNPC()
        {
            ClearDirectory(_npcAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_npcTablePath, typeof(TextAsset)) as TextAsset;            
            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<BT_NPCData>>(textAsset.text);

            TextAsset shopTextAsset = AssetDatabase.LoadAssetAtPath(_shopTablePath, typeof(TextAsset)) as TextAsset;
            var shopTable = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<ShopTable>>(shopTextAsset.text);
            Debug.Log(shopTextAsset);
            TextAsset craftTextAsset = AssetDatabase.LoadAssetAtPath(_craftTablePath, typeof(TextAsset)) as TextAsset;
            var craftTable = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<CraftTable>>(craftTextAsset.text);
            for (int i = 0; i < data.array.Length; i++)
            {
                BT_NPCData npcData = data.array[i];
                ScriptableNPC asset = CreateInstance<ScriptableNPC>();
                
                asset.NpcID = npcData.NpcID;
                asset.Portal = npcData.Portal;
                asset.Shop = npcData.Shop;
                asset.TradeShop = npcData.TradeShop;
                asset.CraftID = npcData.CraftID;

                asset.saleItems = SetsaleItems(npcData.Shop);
                asset.craftIds = SetCraftIds(npcData.CraftID);

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_npcAssetPath}/NPC_{npcData.NpcID}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();

            List<NpcSaleItem> SetsaleItems(int shopId)
            {
                var npcSaleItems = new List<NpcSaleItem>();

                for (int i = 0; i < shopTable.array.Length; i++)
                {
                    var shopData = shopTable.array[i];
                    if (shopData.ShopId == shopId)
                    {
                        npcSaleItems.Add(new NpcSaleItem { 
                            itemId = shopData.itemId,
                            playerMinLevel = shopData.PlayerMinLevel, 
                            playerMaxLevel = shopData.PlayerMaxLevel, 
                            maxCount = shopData.MaxCount, 
                            refillCycle = shopData.refillCycle,
                            });
                    }
                }

                return npcSaleItems;
            }

            List<int> SetCraftIds(int craftId)
            {
                var craftIds = new List<int>();

                for (int i = 0; i < craftTable.array.Length; i++)
                {
                    var craftData = craftTable.array[i];
                    if (craftData.CraftId == craftId)
                    {
                        craftIds.Add(craftData.typeID);
                    }
                }

                return craftIds;
            }
        }
        #endregion

        #region Structure
        [MenuItem("Tools/Blocktopia/Update Individually/Structure/BuildingTable")]
        public static void UpdateBuilding()
        {
            ClearDirectory(_buildingAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_buildingTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<BuildingData>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                BuildingData buildingData = data.array[i];
                ScriptableBuilding asset = CreateInstance<ScriptableBuilding>();

                asset.typeID = buildingData.typeID;
                asset.uniqueID = buildingData.uniqueID;
                asset.groundType = buildingData.groundType;
                asset.constructRecipeID = buildingData.constructRecipeID;
                asset.level = buildingData.level;
                asset.requiredTownLevel = buildingData.requiredTownLevel;
                asset.upGradeTypeID = buildingData.upGradeTypeID;
                asset.abilityValue1 = buildingData.abilityValue1;
                asset.abilityValue2 = buildingData.abilityValue2;
                asset.maxBuildingCount = buildingData.maxBuildingCount;
                asset.abilityType = buildingData.abilityType;
                asset.recipeIDList = buildingData.Setup();
                asset.modelPrefab = Resources.Load<GameObject>($"Blocktopia/Structures/Prefabs/Building/Building_{buildingData.typeID}");

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_buildingAssetPath}/Building_{buildingData.typeID}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }

        //[MenuItem("Tools/Blocktopia/Update Individually/Structure/Decoration Update")]
        //public static void UpdateDecoration()
        //{

        //}
        #endregion

        #region Customize
        [MenuItem("Tools/Blocktopia/Update Individually/CharacterCustomise")]
        public static void UpdateCustomizeData()
        {
            ClearDirectory(_customizeDataAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_customizeTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<CustomizeData>>(textAsset.text);

            for (int i = 0; i < data.array.Length; i++)
            {
                CustomizeData customizeData = data.array[i];
                ScriptableCustomizeData asset = CreateInstance<ScriptableCustomizeData>();

                asset.typeId = customizeData.Id;
                asset.BoneType = customizeData.BoneType;
                asset.PartsType = customizeData.PartsType;
                asset.DefaultView = customizeData.DefaultView;
                asset.Gender = customizeData.Gender;
                asset.CharFileName = customizeData.CharFileName;
                asset.ThumbnailFileName = customizeData.ThumbnailFileName;
                asset.ViewDate = customizeData.ViewDate;

                // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
                AssetDatabase.CreateAsset(asset, $"{_customizeDataAssetPath}/Customize_{customizeData.Id}.asset");
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #region NickNameBan
        [MenuItem("Tools/Blocktopia/Update Individually/BanNickname")]
        public static void UpdateNickNameBan()
        {
            ClearDirectory(_banNickNameAssetPath);

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_banNickNameTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<NickNameBan>>(textAsset.text);

            ScriptableBanNickName asset = CreateInstance<ScriptableBanNickName>();
            asset.banStringList = new List<string>();

            for (int i = 0; i < data.array.Length; i++)
            {
                NickNameBan nickname = data.array[i];

                asset.banStringList.Add(nickname.BanString);
            }

            // ★Resources 폴더로 임시 저장 추후 에셋번들 폴더 위치로 변경 필요
            AssetDatabase.CreateAsset(asset, $"{_banNickNameAssetPath}/BanNickName.asset");
            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();
        }
        #endregion

        #region PlayerExp
        [MenuItem("Tools/Blocktopia/Update Individually/PlayerExp")]
        public static void UpdatePlayerExp()
        {
            ClearDirectory(_playerExpAssetPath);


            TextAsset textAsset = AssetDatabase.LoadAssetAtPath(_playerExpTablePath, typeof(TextAsset)) as TextAsset;

            var data = JsonFx.Json.JsonReader.Deserialize<TableData.SerializeObj<PlayerExp>>(textAsset.text);

            int cur = 0;
            try
            {
                for (int i = 0; i < data.array.Length; i++)
                {
                    var item = data.array[i];




                    ScriptablePlayerExp asset = CreateInstance<ScriptablePlayerExp>();
                    asset.level = item.level;
                    asset.reqExp = item.reqExp;
                    asset.skillCount = item.skillCount;


                    AssetDatabase.CreateAsset(asset, $"{_playerExpAssetPath}/{item.level}.asset");

                    AssetDatabase.SaveAssets();

                    cur++;
                }
            }
            catch
            {
                Debug.LogError($"{cur}번째 실패");
            }

            AssetDatabase.Refresh();
        }
        #endregion
    }
}