﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedItemDataFramework;

namespace EpicLoot
{
    [Serializable]
    public class MagicItemEffectRequirements
    {
        private static StringBuilder _sb = new StringBuilder();
        private static List<string> _flags = new List<string>();

        public bool NoRoll;
        public bool ExclusiveSelf = true;
        public List<string> ExclusiveEffectTypes = new List<string>();
        public List<ItemDrop.ItemData.ItemType> AllowedItemTypes = new List<ItemDrop.ItemData.ItemType>();
        public List<ItemDrop.ItemData.ItemType> ExcludedItemTypes = new List<ItemDrop.ItemData.ItemType>();
        public List<ItemRarity> AllowedRarities = new List<ItemRarity>();
        public List<ItemRarity> ExcludedRarities = new List<ItemRarity>();
        public List<Skills.SkillType> AllowedSkillTypes = new List<Skills.SkillType>();
        public List<Skills.SkillType> ExcludedSkillTypes = new List<Skills.SkillType>();
        public List<string> AllowedItemNames = new List<string>();
        public List<string> ExcludedItemNames = new List<string>();
        public bool ItemHasPhysicalDamage;
        public bool ItemHasElementalDamage;
        public bool ItemUsesDurability;
        public bool ItemHasNegativeMovementSpeedModifier;
        public bool ItemHasBlockPower;
        public bool ItemHasParryPower;
        public bool ItemHasArmor;
        public bool ItemHasBackstabBonus;
        public bool ItemUsesStaminaOnAttack;

        public List<string> CustomFlags;

        public override string ToString()
        {
            _sb.Clear();
            _flags.Clear();

            if (NoRoll) _flags.Add(nameof(NoRoll));
            if (ExclusiveSelf) _flags.Add(nameof(ExclusiveSelf));
            if (ItemHasPhysicalDamage) _flags.Add(nameof(ItemHasPhysicalDamage));
            if (ItemHasElementalDamage) _flags.Add(nameof(ItemHasElementalDamage));
            if (ItemUsesDurability) _flags.Add(nameof(ItemUsesDurability));
            if (ItemHasNegativeMovementSpeedModifier) _flags.Add(nameof(ItemHasNegativeMovementSpeedModifier));
            if (ItemHasBlockPower) _flags.Add(nameof(ItemHasBlockPower));
            if (ItemHasParryPower) _flags.Add(nameof(ItemHasParryPower));
            if (ItemHasArmor) _flags.Add(nameof(ItemHasArmor));
            if (ItemHasBackstabBonus) _flags.Add(nameof(ItemHasBackstabBonus));
            if (ItemUsesStaminaOnAttack) _flags.Add(nameof(ItemUsesStaminaOnAttack));

            if (_flags.Count > 0)
            {
                _sb.AppendLine($"> > **Flags:** `{string.Join(", ", _flags)}`");
            }

            if (ExclusiveEffectTypes != null && ExclusiveEffectTypes.Count > 0)
            {
                _sb.AppendLine($"> > **ExclusiveEffectTypes:** `{string.Join(", ", ExclusiveEffectTypes)}`");
            }

            if (AllowedItemTypes != null && AllowedItemTypes.Count > 0)
            {
                _sb.AppendLine($"> > **AllowedItemTypes:** `{string.Join(", ", AllowedItemTypes)}`");
            }

            if (ExcludedItemTypes != null && ExcludedItemTypes.Count > 0)
            {
                _sb.AppendLine($"> > **ExcludedItemTypes:** `{string.Join(", ", ExcludedItemTypes)}`");
            }

            if (AllowedRarities != null && AllowedRarities.Count > 0)
            {
                _sb.AppendLine($"> > **AllowedRarities:** `{string.Join(", ", AllowedRarities)}`");
            }

            if (ExcludedRarities != null && ExcludedRarities.Count > 0)
            {
                _sb.AppendLine($"> > **ExcludedRarities:** `{string.Join(", ", ExcludedRarities)}`");
            }

            if (AllowedSkillTypes != null && AllowedSkillTypes.Count > 0)
            {
                _sb.AppendLine($"> > **AllowedSkillTypes:** `{string.Join(", ", AllowedSkillTypes)}`");
            }

            if (ExcludedSkillTypes != null && ExcludedSkillTypes.Count > 0)
            {
                _sb.AppendLine($"> > **ExcludedSkillTypes:** `{string.Join(", ", ExcludedSkillTypes)}`");
            }

            if (AllowedItemNames != null && AllowedItemNames.Count > 0)
            {
                _sb.AppendLine($"> > **AllowedItemNames:** `{string.Join(", ", AllowedItemNames)}`");
            }

            if (ExcludedItemNames != null && ExcludedItemNames.Count > 0)
            {
                _sb.AppendLine($"> > **ExcludedItemNames:** `{string.Join(", ", ExcludedItemNames)}`");
            }

            if (CustomFlags != null && CustomFlags.Count > 0)
            {
                _sb.AppendLine($"> > **CustomFlags:** `{string.Join(", ", CustomFlags)}`");
            }

            return _sb.ToString();
        }
    }

    [Serializable]
    public class MagicItemEffectDefinition
    {
        [Serializable]
        public class ValueDef
        {
            public float MinValue;
            public float MaxValue;
            public float Increment;
        }

        [Serializable]
        public class ValuesPerRarityDef
        {
            public ValueDef Magic;
            public ValueDef Rare;
            public ValueDef Epic;
            public ValueDef Legendary;
        }

        public string Type { get; set; }

        public string DisplayText = "";
        public MagicItemEffectRequirements Requirements = new MagicItemEffectRequirements();
        public ValuesPerRarityDef ValuesPerRarity = new ValuesPerRarityDef();
        public float SelectionWeight = 1;
        public bool CanBeAugmented = true;
        public string Comment;

        public List<ItemDrop.ItemData.ItemType> GetAllowedItemTypes()
        {
            return Requirements?.AllowedItemTypes ?? new List<ItemDrop.ItemData.ItemType>();
        }

        public bool CheckRequirements(ExtendedItemData itemData, MagicItem magicItem)
        {
            if (Requirements == null)
            {
                return true;
            }

            if (Requirements.NoRoll)
            {
                return false;
            }

            if (Requirements.ExclusiveSelf && magicItem.HasEffect(Type))
            {
                return false;
            }

            if (Requirements.ExclusiveEffectTypes?.Count > 0 && magicItem.HasAnyEffect(Requirements.ExclusiveEffectTypes))
            {
                return false;
            }

            if (Requirements.AllowedItemTypes?.Count > 0 && !Requirements.AllowedItemTypes.Contains(itemData.m_shared.m_itemType))
            {
                return false;
            }

            if (Requirements.ExcludedItemTypes?.Count > 0 && Requirements.ExcludedItemTypes.Contains(itemData.m_shared.m_itemType))
            {
                return false;
            }

            if (Requirements.AllowedRarities?.Count > 0 && !Requirements.AllowedRarities.Contains(magicItem.Rarity))
            {
                return false;
            }

            if (Requirements.ExcludedRarities?.Count > 0 && Requirements.ExcludedRarities.Contains(magicItem.Rarity))
            {
                return false;
            }

            if (Requirements.AllowedSkillTypes?.Count > 0 && !Requirements.AllowedSkillTypes.Contains(itemData.m_shared.m_skillType))
            {
                return false;
            }

            if (Requirements.ExcludedSkillTypes?.Count > 0 && Requirements.ExcludedSkillTypes.Contains(itemData.m_shared.m_skillType))
            {
                return false;
            }

            if (Requirements.AllowedItemNames?.Count > 0 && !Requirements.AllowedItemNames.Contains(itemData.m_shared.m_name))
            {
                return false;
            }

            if (Requirements.ExcludedItemNames?.Count > 0 && Requirements.ExcludedItemNames.Contains(itemData.m_shared.m_name))
            {
                return false;
            }

            if (Requirements.ItemHasPhysicalDamage && itemData.m_shared.m_damages.GetTotalPhysicalDamage() <= 0)
            {
                return false;
            }

            if (Requirements.ItemHasElementalDamage && itemData.m_shared.m_damages.GetTotalElementalDamage() <= 0)
            {
                return false;
            }

            if (Requirements.ItemUsesDurability && !itemData.m_shared.m_useDurability)
            {
                return false;
            }

            if (Requirements.ItemHasNegativeMovementSpeedModifier && itemData.m_shared.m_movementModifier >= 0)
            {
                return false;
            }

            if (Requirements.ItemHasBlockPower && itemData.m_shared.m_blockPower <= 0)
            {
                return false;
            }

            if (Requirements.ItemHasParryPower && itemData.m_shared.m_deflectionForce <= 0)
            {
                return false;
            }

            if (Requirements.ItemHasArmor && itemData.m_shared.m_armor <= 0)
            {
                return false;
            }

            if (Requirements.ItemHasBackstabBonus && itemData.m_shared.m_backstabBonus <= 0)
            {
                return false;
            }

            if (Requirements.ItemUsesStaminaOnAttack && itemData.m_shared.m_attack.m_attackStamina <= 0 && itemData.m_shared.m_secondaryAttack.m_attackStamina <= 0)
            {
                return false;
            }

            return true;
        }

        public bool HasRarityValues()
        {
            return ValuesPerRarity.Magic != null && ValuesPerRarity.Epic != null && ValuesPerRarity.Rare != null && ValuesPerRarity.Legendary != null;
        }

        public ValueDef GetValuesForRarity(ItemRarity itemRarity)
        {
            switch (itemRarity)
            {
                case ItemRarity.Magic: return ValuesPerRarity.Magic;
                case ItemRarity.Rare: return ValuesPerRarity.Rare;
                case ItemRarity.Epic: return ValuesPerRarity.Epic;
                case ItemRarity.Legendary: return ValuesPerRarity.Legendary;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemRarity), itemRarity, null);
            }
        }
    }

    public class MagicItemEffectsList
    {
        public List<MagicItemEffectDefinition> MagicItemEffects = new List<MagicItemEffectDefinition>();
    }

    public static class MagicItemEffectDefinitions
    {
        public static readonly Dictionary<string, MagicItemEffectDefinition> AllDefinitions = new Dictionary<string, MagicItemEffectDefinition>();
        public static event Action OnSetupMagicItemEffectDefinitions;

        public static void Initialize(MagicItemEffectsList config)
        {
            foreach (var magicItemEffectDefinition in config.MagicItemEffects)
            {
                Add(magicItemEffectDefinition);
            }
            OnSetupMagicItemEffectDefinitions?.Invoke();
        }

        public static void Add(MagicItemEffectDefinition effectDef)
        {
            if (AllDefinitions.ContainsKey(effectDef.Type))
            {
                EpicLoot.LogWarning($"Removed previously existing magic effect type: {effectDef.Type}");
                AllDefinitions.Remove(effectDef.Type);
            }

            EpicLoot.Log($"Added MagicItemEffect: {effectDef.Type}");
            AllDefinitions.Add(effectDef.Type, effectDef);
        }

        public static MagicItemEffectDefinition Get(string type)
        {
            AllDefinitions.TryGetValue(type, out MagicItemEffectDefinition effectDef);
            return effectDef;
        }

        public static List<MagicItemEffectDefinition> GetAvailableEffects(ExtendedItemData itemData, MagicItem magicItem, int ignoreEffectIndex = -1)
        {
            MagicItemEffect effect = null;
            if (ignoreEffectIndex >= 0 && ignoreEffectIndex < magicItem.Effects.Count)
            {
                effect = magicItem.Effects[ignoreEffectIndex];
                magicItem.Effects.RemoveAt(ignoreEffectIndex);
            }

            var results = AllDefinitions.Values.Where(x => x.CheckRequirements(itemData, magicItem)).ToList();

            if (effect != null)
            {
                magicItem.Effects.Insert(ignoreEffectIndex, effect);
            }

            return results;
        }

        public static bool IsValuelessEffect(string effectType, ItemRarity rarity)
        {
            var effectDef = Get(effectType);
            if (effectDef == null)
            {
                EpicLoot.LogWarning($"Checking if unknown effect is valuless ({effectType}/{rarity})");
                return false;
            }

            return effectDef.GetValuesForRarity(rarity) == null;
        }
    }
}
