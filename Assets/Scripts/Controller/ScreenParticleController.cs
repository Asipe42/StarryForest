using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScreenParticleData
{
    public ParticleSystem[] screenParticles;
}

public class ScreenParticleController : MonoBehaviour
{
    public static ScreenParticleController instance;

    [SerializeField]
    ScreenParticleData screenParticleData;

    Dictionary<EChapterType, ParticleSystem> screenParticleDic;

    ParticleSystem currentParitlce;

    void Awake()
    {
        InitProperty();
        ConvertParitlce(GameManager.instance.gameData.chapterType);
    }

    void InitProperty()
    {
        instance = this;
        
        screenParticleDic = new Dictionary<EChapterType, ParticleSystem>();

        for (int i = 0; i < screenParticleData.screenParticles.Length; i++)
        {
            var particle = screenParticleData.screenParticles[i];
            particle.Stop();

            screenParticleDic.Add((EChapterType)i, particle);
        }

    }

    public void SetParticleModifier(float value)
    {
        var system = currentParitlce.velocityOverLifetime;

        system.xMultiplier = -value;
    }

    public void ConvertParitlce(EChapterType type)
    {
        if (currentParitlce != null)
            currentParitlce.Stop();

        var particle = screenParticleDic[type];

        particle.Play();
        currentParitlce = particle;
    }
}