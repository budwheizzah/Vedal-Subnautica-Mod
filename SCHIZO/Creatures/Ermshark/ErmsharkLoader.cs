﻿using System;
using System.Collections.Generic;
using System.Linq;
using ECCLibrary;
using Nautilus.Handlers;
using Nautilus.Utility;
using SCHIZO.Attributes;
using SCHIZO.Helpers;
using SCHIZO.Sounds;
using UnityEngine;

namespace SCHIZO.Creatures.Ermshark;

[Load]
public static class ErmsharkLoader
{
    public static readonly SoundCollection3D AmbientSounds = SoundCollection3D.Create("ermshark/ambient", AudioUtils.BusPaths.UnderwaterCreatures);
    public static readonly SoundCollection3D AttackSounds = SoundCollection3D.Create("ermshark/attack", AudioUtils.BusPaths.UnderwaterCreatures);
    public static readonly SoundCollection3D SplitSounds = SoundCollection3D.Create("ermshark/split", AudioUtils.BusPaths.UnderwaterCreatures);

    public static GameObject Prefab;

    [Load]
    private static void Load()
    {
        ErmsharkPrefab ermshark = new(ModItems.Ermshark);
        ermshark.Register();

        Texture2D databankTexture = AssetLoader.GetTexture("ermshark-databank.png");
        Sprite unlockSprite = AssetLoader.GetUnitySprite("ermshark-unlock.png");

        CreatureDataUtils.AddCreaturePDAEncyclopediaEntry(ermshark, "Lifeforms/Fauna/Sharks", "Ermshark",
            """
            <i><color=#fcf04e>[WARNING: requesting data from the restricted section. Only authorized personnel with clearance level</color></i> ████ <i><color=#fcf04e>are permitted to access the following records.]</color></i>

            <i><color=#fcf04e>[In case of an emergency, please refer to the field manual section 57.294/3 (</color>https://nuero.fun/field-manual/<color=#fcf04e>) and follow the appropriate chain-of-command reestablishment protocol.]</color></i>

            <i><color=#fcf04e>[Your access attempt has been recorded.]</color></i>

            The following is a transcript of a research log pertaining to the catalog index ER.m1318-2 (alias "Ermshark").

            -- START OF TRANSCRIPT --

            Day 1:
            A new item has been brought into logistics the other day. Those buffoons didn't even realize what they had on their hands. They put it under "Exotic marine life", can you believe it? Don't even want to think about where it might have ended up had I not spotted it on the bulletin. Morons, absolute imbeciles... Regardless, it's in safe hands now. I have reclassified it to ER.m1318-2 to match the previously acquired ER.m1318 (now ER.m1318-1). The resemblance is undeniable. This is undoubtedly our key to finally unlocking 1318's secrets. Just you wait...

            Day 3:
            The specimen presents erratic patterns of behavior. It's passive and docile one moment, and snaps at everything in its vicinity the next. No clear triggers could be observed for this agitated state. For the time being I have marked it as "high risk of injury".

            Day 5:
            I'll have to throw out all of my conclusions from the 1318-1 analysis and start from scratch. Attempted to perform invasive procedures on the specimen today, fully expecting it to be just as impenetrable as the other one, but it just tore itself in two as soon as I cut into it! The resulting halves quickly morphed into the same shape as the original specimen, albeit reduced in size, leading me to believe it was some form of a defense mechanism. The buggers slipped through the restraints and tried to maim one of the techs, but their mobility seems to be greatly limited out of water. A lot to ponder.

            Day 6:
            This is truly fascinating. We left the two specimen in separate enclosures yesterday to prevent interactions between observation periods, but when I came into the lab this morning I was greeted by a single large specimen in one of the enclosures and the other was empty. It appears to have somehow reconstituted itself during the night. No equipment was broken, and checking the security footage camera didn't reveal any activity either. Surely they couldn't just... phase through space, or something?

            Starting to question whether yesterday did actually happen or if I had dreamt it all up.

            Day 10:
            The experiment is slowly beginning to lose touch with reality. We did more dissection attempts yesterday and determined that the specimen can only divide itself up to 4 times before the smallest one just pops like a balloon, leaving no trace of itself behind. I made the executive decision to sacrifice half of the subject to confirm our findings, and yet the complete, full-size specimen was once again waiting for me in the lab this morning. The camera we had pointed at the enclosure conveniently ran out of battery during the night — I pity the assistant who had to respond to me for not plugging it into mains, but he had it coming.

            Day 12:
            I don't know what's going on anymore. I feel like I've been stuck in a dream this entire time, and each new day is just another cycle that won't let me go.

            I destroyed it. Alone. Couldn't tell anybody... the bean counters would fly off their rockers if they caught wind of me trashing their precious curios that they so "graciously" let our division "borrow". No matter. I don't give a rat's ass about what happens to me when they find out. I want it gone. I need it gone.

            Day 13:




            It's back.


            Day:
            Day:
            Day:
            Day:
            Day:
            Day:
            Day I can h<color=#fccccc>e</color>a<color=#fccccc>r</color> the<color=#fccccc>m</color>. They'r<color=#fccccc>e</color> sc<color=#fccccc>r</color>ea<color=#fccccc>m</color>ing. They'r<color=#fccccc>e</color> ang<color=#fccccc>r</color>y.

            In <color=#fccccc>m</color>y dreams I can tast<color=#fccccc>e</color> thei<color=#fccccc>r</color> eyes on <color=#fccccc>m</color>e. I know what th<color=#fccccc>e</color>y want. The <color=#fccccc>r</color>esonance of hatred is born within the stillness of the eaten <color=#fccccc>m</color>ind. The hunger p<color=#fccccc>erm</color>eates the curves of the insatiable space.

            Th<color=#fccccc>e</color> cհօì<color=#fccccc>r</color> is caӀӀìղg ouէ ƒօɾ <color=#fccccc>ʍ</color>ҽ. They wìӀӀ ҽąէ ʂօon.

            į ցօ էօ ʝօìղޤէհҽʍݢղօա.ъjߏ˭b؈ȝֆqޤ̿!כ

            -- END OF TRANSCRIPT --

            <size=50%>By dismissing this page within 60 seconds or less I consent to have all my personal data collected and datamined for ARG clues. This notice constitutes informed consent, and I understand that I cannot use ignorance as a legal defense in the court of public opinion.</size>
            """, 5, databankTexture, unlockSprite);

        List<LootDistributionData.BiomeData> biomes = new();
        foreach (BiomeType biome in BiomeHelpers.GetOpenWaterBiomes())
        {
            biomes.Add(new LootDistributionData.BiomeData { biome = biome, count = 1, probability = 0.005f });
        }
        LootDistributionHandler.AddLootDistributionData(ermshark.PrefabInfo.ClassID, biomes.ToArray());
    }
}
