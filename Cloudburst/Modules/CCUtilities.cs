using RoR2;
using RoR2.CharacterAI;
using RoR2.Projectile;
using RoR2.Skills;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering.PostProcessing;
using R2API.Utils;
using System.Runtime.CompilerServices;
using BepInEx.Logging;

public static class CCUtilities
{
    #region Projectiles

    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }

    /// <summary>
    /// Creates a valid projectile from a GameObject 
    /// </summary>
    /// <param name="projectile"></param>
    public static void CreateValidProjectile(GameObject projectile, float lifeTime, float velocity, bool updateAfterFiring)
    {
        var networkIdentity = projectile.AddComponent<NetworkIdentity>();
        var teamFilter = projectile.AddComponent<TeamFilter>();
        var projectileController = projectile.AddComponent<ProjectileController>();
        var networkTransform = projectile.AddComponent<ProjectileNetworkTransform>();
        var projectileSimple = projectile.AddComponent<ProjectileSimple>();
        var projectileDamage = projectile.AddComponent<ProjectileDamage>();

        //setup the projectile controller
        projectileController.allowPrediction = false;
        projectileController.predictionId = 0;
        projectileController.procCoefficient = 1;
        projectileController.owner = null;

        //setup the network transform
        networkTransform.allowClientsideCollision = false;
        networkTransform.interpolationFactor = 1;
        networkTransform.positionTransmitInterval = 0.03333334f;

        //setup the projectile simple
        projectileSimple.desiredForwardSpeed = velocity;
        projectileSimple.lifetime = lifeTime;
        projectileSimple.updateAfterFiring = updateAfterFiring;
        projectileSimple.enableVelocityOverLifetime = false;
        //projectileSimple.velocityOverLifetime = UnityEngine.AnimationCurve.;
        projectileSimple.oscillate = false;
        projectileSimple.oscillateMagnitude = 20;
        projectileSimple.oscillateSpeed = 0;

        projectileDamage.damage = 0;
        projectileDamage.crit = false;
        projectileDamage.force = 0;
        projectileDamage.damageColorIndex = DamageColorIndex.Default;
        projectileDamage.damageType = DamageType.Shock5s;
    }
    #endregion
    #region AI
    /// <summary>
    /// Copies skilldriver settings from "beingCopiedFrom" to "copier"
    /// Don't forget to set requiredSkill!
    /// </summary>
    /// <param name="beingCopiedFrom"></param>
    /// <param name="copier"></param>
    public static void CopyAISkillSettings(AISkillDriver beingCopiedFrom, AISkillDriver copier)
    {
        copier.activationRequiresAimConfirmation = beingCopiedFrom.activationRequiresAimConfirmation;
        copier.activationRequiresTargetLoS = beingCopiedFrom.activationRequiresTargetLoS;
        copier.aimType = beingCopiedFrom.aimType;
        copier.buttonPressType = beingCopiedFrom.buttonPressType;
        copier.customName = beingCopiedFrom.customName;
        copier.driverUpdateTimerOverride = beingCopiedFrom.driverUpdateTimerOverride;
        copier.ignoreNodeGraph = beingCopiedFrom.ignoreNodeGraph;
        copier.maxDistance = beingCopiedFrom.maxDistance;
        copier.maxTargetHealthFraction = beingCopiedFrom.maxTargetHealthFraction;
        copier.maxUserHealthFraction = beingCopiedFrom.maxUserHealthFraction;
        copier.minDistance = beingCopiedFrom.minDistance;
        copier.minTargetHealthFraction = beingCopiedFrom.minTargetHealthFraction;
        copier.minUserHealthFraction = beingCopiedFrom.minUserHealthFraction;
        copier.moveInputScale = beingCopiedFrom.moveInputScale;
        copier.movementType = beingCopiedFrom.movementType;
        copier.moveTargetType = beingCopiedFrom.moveTargetType;
        copier.nextHighPriorityOverride = beingCopiedFrom.nextHighPriorityOverride;
        copier.noRepeat = beingCopiedFrom.noRepeat;
        //Don't do this because the skilldef is not the same.
        //_out.requiredSkill = _in.requiredSkill;
        copier.requireEquipmentReady = beingCopiedFrom.requireEquipmentReady;
        copier.requireSkillReady = beingCopiedFrom.requireSkillReady;
        copier.resetCurrentEnemyOnNextDriverSelection = beingCopiedFrom.resetCurrentEnemyOnNextDriverSelection;
        copier.selectionRequiresOnGround = beingCopiedFrom.selectionRequiresOnGround;
        copier.selectionRequiresTargetLoS = beingCopiedFrom.selectionRequiresTargetLoS;
        copier.shouldFireEquipment = beingCopiedFrom.shouldFireEquipment;
        copier.shouldSprint = beingCopiedFrom.shouldSprint;
        //shouldTapButton is deprecated, don't use it!
        //_out.shouldTapButton = _in.shouldTapButton;
        copier.skillSlot = beingCopiedFrom.skillSlot;

    }

    public static bool FriendlyFire_ShouldKnockupProceed(CharacterBody victimBody, TeamIndex attackerTeamIndex)
    {
        return victimBody.teamComponent.teamIndex != attackerTeamIndex || FriendlyFireManager.friendlyFireMode != FriendlyFireManager.FriendlyFireMode.Off || attackerTeamIndex == TeamIndex.None;
    }

    public static bool ShouldKnockup(CharacterBody victimBody, TeamIndex attackerTeamIndex)
    {
        bool canHit = true;
        //if (victimBody.isChampion)
        //{
        //    canHit = false;
        //}
        //if (victimBody.baseNameToken == "BROTHER_BODY_NAME")
        //{
        //    canHit = true;
        //}
        //knockup all fuck it
        if (!FriendlyFire_ShouldKnockupProceed(victimBody, attackerTeamIndex))
        {
            canHit = false;
        }

        return canHit;
    }

    #endregion
    #region UNITY2ROR2
    //ref here so we can pass in a rented collection from HG.CollectionPool
    public static void CharacterOverlapSphereAll(ref List<CharacterBody> hitBodies, Vector3 position, float radius, LayerMask layerMask)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, layerMask);
        foreach (Collider collider in colliders)
        {
            HurtBox hurtBox = collider.gameObject.GetComponent<HurtBox>();
            if (hurtBox)
            {
                CharacterBody characterBody = hurtBox.healthComponent.body;
                if (characterBody && !hitBodies.Contains(characterBody))
                {
                    hitBodies.Add(characterBody);
                }
            }
        }
    }


    public static void AddUpwardForceToBody(GameObject victimBody, float acceleration)
    {
        CharacterMotor hitMotor = victimBody.GetComponent<CharacterMotor>();
        if (hitMotor)
        {
            hitMotor.ApplyForce(Vector3.up * hitMotor.mass * acceleration, true, true);
        }
        RigidbodyMotor rigidMotor = victimBody.GetComponent<RigidbodyMotor>();
        if (rigidMotor)
        {
            rigidMotor.rigid.AddForce(Vector3.up * rigidMotor.mass * acceleration, ForceMode.Impulse);
        }
    }

    public static void AddExplosionForce(CharacterMotor body, float explosionForce, Vector3 explosionPosition, float explosionRadius, float upliftModifier = 0, bool useWearoff = false)
    {
        var dir = (body.transform.position - explosionPosition);

        Vector3 baseForce = Vector3.zero;

        if (useWearoff)
        {
            float wearoff = 1 - (dir.magnitude / explosionRadius);
            baseForce = dir.normalized * explosionForce * wearoff;
        }
        else
        {
            baseForce = dir.normalized * explosionForce;
        }
        //baseForce.z = 0;
        body.ApplyForce(baseForce);

        //if (upliftModifier != 0)
        //{
        float upliftWearoff = 1 - upliftModifier / explosionRadius;
        Vector3 upliftForce = Vector2.up * explosionForce * upliftWearoff;
        //upliftForce.z = 0;
        body.ApplyForce(upliftForce);
        //}

    }
    #endregion

    //                float barrierToApply = BaseBarrier.Value + ((count * StackingBarrier.Value) - StackingBarrier.Value);

    public static float GenericFlatStackingFloat(float baseValue, int itemCount, float stackingValue) {
        return baseValue + ((itemCount * stackingValue) - stackingValue);
    }

    public static HitBoxGroup FindHitBoxGroup(string groupName, Transform modelTransform)
    {
        if (!modelTransform)
        {
            return null;
        }
        HitBoxGroup result = null;
        List<HitBoxGroup> gameObjectComponents = GetComponentsCache<HitBoxGroup>.GetGameObjectComponents(modelTransform.gameObject);
        int i = 0;
        int count = gameObjectComponents.Count;
        while (i < count)
        {
            if (gameObjectComponents[i].groupName == groupName)
            {
                result = gameObjectComponents[i];
                break;
            }
            i++;
        }
        GetComponentsCache<HitBoxGroup>.ReturnBuffer(gameObjectComponents);
        return result;
    }

    public static void RefreshALLBuffStacks(CharacterBody body, BuffDef def, float duration)
    {
        int num6 = 0;
        for (int j = 0; j < body.timedBuffs.Count; j++)
        {
            if (body.timedBuffs[j].buffIndex == def.buffIndex)
            {
                num6++;
                if (body.timedBuffs[j].timer < duration)
                {
                    body.timedBuffs[j].timer = duration;
                }
            }
        }
    }

    public static Vector3 FindBestPosition(HurtBox target)
    {
        float radius = 15f;
        var originPoint = target.transform.position;
        originPoint.x += UnityEngine.Random.Range(-radius, radius);
        originPoint.z += UnityEngine.Random.Range(-radius, radius);
        originPoint.y += UnityEngine.Random.Range(radius, radius);
        return originPoint;
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public static List<T> Join<T>(this List<T> first, List<T> second)
    {
        if (first == null)
        {
            return second;
        }
        if (second == null)
        {
            return first;
        }

        return first.Concat(second).ToList();
    }

    public static Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

    /// <summary>
    /// Safely removes a buff from the target character body
    /// </summary>
    /// <param name="buffToRemove">The buff you want to safely remove</param>
    /// <param name="body">The body you safely want to remove a buff from.</param>
    public static void SafeRemoveBuff(BuffDef buffToRemove, CharacterBody body)
    {
        if (body && body.HasBuff(buffToRemove))
        {
            body.RemoveBuff(buffToRemove);
        }
    }
    /// <summary>
    /// Safely removes buffs from the target character body
    /// </summary>
    /// <param name="buffToRemove">The buff you want to safely remove</param>
    /// <param name="body">The body you safely want to remove buffs from.</param>
    public static void SafeRemoveBuffs(BuffDef buffToRemove, CharacterBody body, int stacksToRemove)
    {
        if (body)
        {
            for (int i = 0; i < stacksToRemove; i++)
            {
                SafeRemoveBuff(buffToRemove, body);
            }
        }
    }

    /// <summary>
    /// Safely removes ALL of target buff from the target character body
    /// </summary>
    /// <param name="buffToRemove">The buff you want to safely remove all of</param>
    /// <param name="body">The body you safely want to remove buffs from.</param>
    public static void SafeRemoveAllOfBuff(BuffDef buffToRemove, CharacterBody body)
    {
        if (body)
        {
            int stacks = body.GetBuffCount(buffToRemove);
            for (int i = 0; i < stacks; i++)
            {
                body.RemoveBuff(buffToRemove);
            }

        }
    }
}
