using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance; 
    public Dictionary<EnumDefinition.SOUND_EFFECT, AudioSource> dicEffect = new Dictionary<EnumDefinition.SOUND_EFFECT, AudioSource>();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
     
    }
    public void InitEffect(Transform parent = null)
    {
        AudioSource source = new AudioSource();
        source = new GameObject(EnumDefinition.SOUND_EFFECT.battery_charge.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.battery_charge.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.battery_charge, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.battery_charge_finish.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.battery_charge_finish.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.battery_charge_finish, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.car_lift.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.car_lift.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.car_lift, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.close_the_driver_door.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.close_the_driver_door.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.close_the_driver_door, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.drop_the_tool.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.drop_the_tool.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.drop_the_tool, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.hook_lever_operation.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.hook_lever_operation.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.hook_lever_operation, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.ignition.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.ignition.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.ignition, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.put_on_work_table.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.put_on_work_table.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.put_on_work_table, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.ratchet_wrench.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.ratchet_wrench.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.ratchet_wrench, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.spring_compressor_01.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.spring_compressor_01.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.spring_compressor_01, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.spring_compressor_02.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.spring_compressor_02.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.spring_compressor_02, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.spring_compressor_03.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.spring_compressor_03.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.spring_compressor_03, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.spring_noise.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.spring_noise.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.spring_noise, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.torque_wrench.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.torque_wrench.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.torque_wrench, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.vehicle_starter.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.vehicle_starter.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.vehicle_starter, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.wheel_alignment_head_fixed.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.wheel_alignment_head_fixed.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.wheel_alignment_head_fixed, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.ioniq6_start.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.ioniq6_start.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.ioniq6_start, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.chain.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.chain.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.chain, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.finish.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.finish.ToString()) as AudioClip;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.finish, source);
        source = new GameObject(EnumDefinition.SOUND_EFFECT.air_pump.ToString()).AddComponent<AudioSource>();
        source.clip = Resources.Load("Sound/" + EnumDefinition.SOUND_EFFECT.air_pump.ToString()) as AudioClip;
        source.loop = true;
        dicEffect.Add(EnumDefinition.SOUND_EFFECT.air_pump, source);

        List<AudioSource> audioList = new List<AudioSource>(dicEffect.Values);
        for (int i = 0; i < audioList.Count; i++)
        {
            audioList[i].transform.SetParent(parent);
            audioList[i].playOnAwake = false; 
        }
    }

    public void Play(EnumDefinition.SOUND_EFFECT effect,bool oneShot = false)
    {
      

        if(oneShot)
        {
            dicEffect[effect].Play();

            return; 
        }

        if(dicEffect[effect].isPlaying == false)
        {
            Debug.Log("효과음on ===> " + effect.ToString());
            dicEffect[effect].Play(); 
        }

    }

    public void Stop(EnumDefinition.SOUND_EFFECT effect)
    {
        Debug.Log("효과음off ===> " + effect.ToString());
        dicEffect[effect].Stop(); 
    }


}
