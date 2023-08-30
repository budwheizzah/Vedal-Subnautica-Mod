﻿using System.Collections;
using ECCLibrary;
using ECCLibrary.Data;
using ECCLibrary.Mono;
using Nautilus.Assets;
using Nautilus.Utility;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SCHIZO.Ermfish;

public class Ermfish : CreatureAsset
{
	public Ermfish(PrefabInfo prefabInfo) : base(prefabInfo)
	{
	}

	protected override CreatureTemplate CreateTemplate()
	{
		const float swimVelocity = 8f;

		CreatureTemplate template = new(GetModel(), BehaviourType.SmallFish, EcoTargetType.Peeper, float.MaxValue);
		CreatureTemplateUtils.SetCreatureDataEssentials(template, LargeWorldEntity.CellLevel.Medium, 10, bioReactorCharge: 420);
		CreatureTemplateUtils.SetCreatureMotionEssentials(template, new SwimRandomData(0.2f, swimVelocity, new Vector3(20, 5, 20)), new StayAtLeashData(0.6f, swimVelocity * 1.25f, 14f));
		CreatureTemplateUtils.SetPreyEssentials(template, swimVelocity, new PickupableFishData(TechType.Floater, "WM", "VM"), new EdibleData(13, -7, false, 1f));
		template.ScannerRoomScannable = true;
		template.CanBeInfected = false;
		template.AvoidObstaclesData = new AvoidObstaclesData(1f, swimVelocity, false, 5f, 5f);
		template.SizeDistribution = new AnimationCurve(new Keyframe(0, 0.5f), new Keyframe(1, 1f));
		template.AnimateByVelocityData = new AnimateByVelocityData(swimVelocity);
		template.SwimInSchoolData = new SwimInSchoolData(0.5f, swimVelocity, 2f, 0.5f, 1f, 0.1f, 25f);
		template.SetWaterParkCreatureData(new WaterParkCreatureDataStruct(0.1f, 0.5f, 1f, 1.5f, true, true, ClassID));

		return template;
	}

	private static GameObject GetModel()
	{
		GameObject model = new("Ermfish model");
		model.SetActive(false);

		GameObject worldModel = new("WM")
		{
			transform =
			{
				parent = model.transform
			}
		};

		GameObject erm = AssetLoader.GetAssetBundle("erm").LoadAsset<GameObject>("erm_fishes");
		GameObject ermInstance = GameObject.Instantiate(erm, worldModel.transform, true);
		ermInstance.transform.GetChild(0).localPosition = Vector3.zero;
		ermInstance.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
		ermInstance.transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
		ermInstance.transform.localScale *= 0.2f;

		GameObject viewModel = Object.Instantiate(worldModel, model.transform, true);
		viewModel.name = "VM";
		viewModel.SetActive(false);
		viewModel.transform.localScale *= 0.35f;
		viewModel.transform.Rotate(180, 180, 0);

		worldModel.AddComponent<Animator>();
		viewModel.AddComponent<Animator>();

		foreach (Collider col in model.GetComponentsInChildren<Collider>(true))
		{
			Object.DestroyImmediate(col);
		}
		model.gameObject.AddComponent<SphereCollider>();

		Object.DontDestroyOnLoad(model);

		return model;
	}

	protected override IEnumerator ModifyPrefab(GameObject prefab, CreatureComponents components)
	{
		prefab.GetComponent<HeldFish>().ikAimLeftArm = true;
		prefab.EnsureComponent<ErmfishNoises>();

		CreaturePrefabUtils.AddDamageModifier(prefab, DamageType.Heat, 0f);
		CreaturePrefabUtils.AddDamageModifier(prefab, DamageType.Acid, 0f);
		CreaturePrefabUtils.AddDamageModifier(prefab, DamageType.Cold, 0f);
		CreaturePrefabUtils.AddDamageModifier(prefab, DamageType.Fire, 0f);
		CreaturePrefabUtils.AddDamageModifier(prefab, DamageType.Poison, 0f);
		CreaturePrefabUtils.AddDamageModifier(prefab, DamageType.Radiation, 0f);
		CreaturePrefabUtils.AddDamageModifier(prefab, DamageType.Starve, 0f);

		yield break;
	}

	protected override void ApplyMaterials(GameObject prefab) => MaterialUtils.ApplySNShaders(prefab, 1f);
}
