﻿using System;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using UnityEngine;

namespace SCHIZO.Gadgets;

public sealed class EatableGadget : Gadget
{
    public float FoodValue { get; set; }
    public float WaterValue { get; set; }
    public bool Decomposes { get; set; }
    public float DecayRate { get; set; } = 0.015f;

    public EatableGadget(ICustomPrefab prefab) : base(prefab)
    {
    }

    public EatableGadget WithNutritionValues(float foodValue, float waterValue)
    {
        FoodValue = foodValue;
        WaterValue = waterValue;
        return this;
    }

    public EatableGadget WithDecay(float decayRate)
    {
        Decomposes = true;
        DecayRate = decayRate * 0.015f;
        return this;
    }

    public EatableGadget WithDecay(bool decomposes)
    {
        Decomposes = decomposes;
        DecayRate = 0.015f;
        return this;
    }

    protected override void Build()
    {
        if (prefab is not CustomPrefab customPrefab) throw new InvalidOperationException("EatableGadget can only be applied to CustomPrefab instances.");
        if (prefab.Info.TechType == TechType.None) throw new InvalidOperationException($"Prefab '{prefab.Info}' does not contain a TechType.");

        PrefabPostProcessorAsync originalPostProcess = customPrefab.OnPrefabPostProcess;
        customPrefab.SetPrefabPostProcessor(obj =>
        {
            originalPostProcess?.Invoke(obj);
            PrefabPostProcess(obj);
        });
    }

    private void PrefabPostProcess(GameObject obj)
    {
        Eatable eatable = obj.EnsureComponent<Eatable>();
        eatable.foodValue = FoodValue;
        eatable.waterValue = WaterValue;
        eatable.kDecayRate = DecayRate;
        eatable.decomposes = Decomposes;
    }
}

public static class EatableGadgetExtensions
{
    public static EatableGadget SetNutritionValues(this CustomPrefab prefab, float foodValue, float waterValue)
    {
        if (!prefab.TryGetGadget(out EatableGadget gadget))
            gadget = prefab.AddGadget(new EatableGadget(prefab));

        gadget.FoodValue = foodValue;
        gadget.WaterValue = waterValue;
        return gadget;
    }
}
