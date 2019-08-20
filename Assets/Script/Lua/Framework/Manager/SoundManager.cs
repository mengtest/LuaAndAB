using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System;

namespace LuaFramework
{
    public class SoundManager : LuaFramework.Manager
    {
        public static SoundManager instance;

        public delegate AudioClip LoadSoundFunc(string filename);
        public static LoadSoundFunc onLoadSound = null;

        protected List<string> m_SearchPaths = new List<string>();

        protected bool m_IsMute = false;

        protected float m_MusicVolume = 0.4f;
        protected float m_SoundVolume = 1.0f;

        //背景音乐
        protected AudioSource m_Background = null;

        //语音
        protected bool m_IsVoiceExclusive;
        protected AudioSource m_Voice = null;

        protected LuaFunction m_LuaCallback = null;
        protected System.Action m_VoiceCallback = null;

        //音效
        protected List<AudioSource> m_Sounds = new List<AudioSource>();

        //
        protected Hashtable m_AudioClips = new Hashtable();



        protected override void Init()
        {
            instance = this;
            if (this.gameObject.GetComponent<AudioSource>() != null)
                m_Background = this.gameObject.GetComponent<AudioSource>();
            else
                m_Background = this.gameObject.AddComponent<AudioSource>();
            m_Background.loop = true;
            m_Background.priority = 128;

            m_Voice = this.gameObject.AddComponent<AudioSource>();
            m_Voice.loop = false;
            m_Voice.priority = 128;

            m_IsMute = PlayerPrefs.GetInt("IsMute", 0) == 0 ? false : true;
            m_MusicVolume = PlayerPrefs.GetFloat("MusicVolume", .4f);
            m_SoundVolume = PlayerPrefs.GetFloat("SoundVolume", .4f);
        }

        void Update()
        {
            if (m_Voice != null && !m_Voice.isPlaying)
            {
                m_IsVoiceExclusive = false;

                if (m_VoiceCallback != null)
                {
                    System.Action callback = m_VoiceCallback;
                    m_VoiceCallback = null;
                    callback();
                }

                if (m_LuaCallback != null)
                {
                    LuaFunction callback = m_LuaCallback;
                    m_LuaCallback = null;
                    callback.Call();
                }
            }
        }

        void OnDestroy()
        {
            m_Background = null;
            m_Voice = null;
            m_Sounds.Clear();
        }

        protected override void Term()
        {
            base.Term();
            OnDestroy();
            instance = null;
        }

        /// <summary>
        /// 加载音频对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileString"> 音效文件名 </param>
        /// <returns></returns>
        public AudioClip LoadSound(int key, string filename)
        {
            return null;
            //Debug.LogError(key);

            //AudioClip audioClip = this.Get(key);
            //if (audioClip == null)
            //{
            //    if (onLoadSound != null)
            //        audioClip = onLoadSound(filename);
            //    else
            //        audioClip = Resources.Load(Globe.SOUND_PATH + filename) as AudioClip;

            //    if (audioClip)
            //        m_AudioClips.Add(key, audioClip);
            //}

           // return audioClip;
        }


        private string[] split = (new string[] { "/", @"\" });
        /// <summary>
        /// ab包热更新
        /// </summary>
        /// <param name="ABID"></param>
        /// <param name="streamPath"> 表中的resource路径</param>
        /// <param name="_callBack"></param>
        /// <returns></returns>
        public AudioClip GetSoundByABID(int _key, int ABID, string streamPath, LuaFunction _callBack)
        {
            
            return null;
        }

        /// <summary>
        /// 删除音频对象
        /// </summary>
        /// <param name="key"></param>
        public void RemoveSound(int key)
        {
            if (m_AudioClips.ContainsKey(key))
                m_AudioClips.Remove(key);
        }

        /// <summary>
        /// 获取音频对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AudioClip Get(int key)
        {
            if (m_AudioClips.ContainsKey(key))
                return m_AudioClips[key] as AudioClip;
            return null;
        }

        /// <summary>
        /// 是否静音状态
        /// </summary>
        /// <returns></returns>
        public bool IsMute()
        {
            return m_IsMute;
        }

        /// <summary>
        /// 设置静音状态
        /// </summary>
        /// <param name="isMute"></param>
        public void SetMute(bool isMute)
        {
            if (m_IsMute != isMute)
            {
                m_IsMute = isMute;
                PlayerPrefs.SetInt("IsMute", m_IsMute ? 1 : 0);

                if (m_Background != null)
                    m_Background.mute = m_IsMute;

                if (m_Voice != null)
                    m_Voice.mute = m_IsMute;

                for (int i = 0; i < m_Sounds.Count; i++)
                    m_Sounds[i].mute = m_IsMute;
            }
        }

        /// <summary>
        /// 设置背景音量
        /// </summary>
        /// <param name="volume"></param>
        public void SetMusicVolume(float volume)
        {
            if (m_MusicVolume != volume)
            {
                m_MusicVolume = volume;
                PlayerPrefs.SetFloat("MusicoVolume", m_MusicVolume);

                if (m_Background != null)
                    m_Background.volume = m_MusicVolume;
            }
        }

        /// <summary>
        /// 返回背景音量
        /// </summary>
        /// <returns></returns>
        public float GetMusicVolume()
        {
            return m_MusicVolume;
        }

        /// <summary>
        /// 设置音频音量
        /// </summary>
        /// <param name="volume"></param>
        public void SetSoundVolume(float volume)
        {
            if (m_SoundVolume != volume)
            {
                m_SoundVolume = volume;
                PlayerPrefs.SetFloat("SoundVolume", m_SoundVolume);

                if (m_Voice != null)
                    m_Voice.volume = m_SoundVolume;

                for (int i = 0; i < m_Sounds.Count; i++)
                    m_Sounds[i].volume = m_SoundVolume;
            }
        }

        /// <summary>
        /// 返回音频音量
        /// </summary>
        /// <returns></returns>
        public float GetSoundVolume()
        {
            return m_SoundVolume;
        }

        #region Background
        public void PlayMusic(int key)
        {
            this.PlayMusic(key, m_MusicVolume, 1.0f);
        }

        public void PlayMusic(int key, float volume, float pitch)
        {
            AudioClip clip = this.Get(key);
            if (clip != null)
            {
                m_Background.clip = clip;
                m_Background.volume = volume;
                m_Background.pitch = pitch;
                m_Background.Play();
            }
            m_Background.mute = m_IsMute;
        }

        /// <summary>
        /// 暂停播放背景音
        /// </summary>
        public void PauseMusic()
        {
            if (m_Background != null)
                m_Background.Pause();
        }

        /// <summary>
        /// 停止播放背景音
        /// </summary>
        public void StopMusic()
        {
            if (m_Background != null)
                m_Background.Stop();
        }

        /// <summary>
        /// 背景音是否在播放
        /// </summary>
        /// <returns></returns>
        public bool IsPlayMusic()
        {
            return m_Background != null ? m_Background.isPlaying : false;
        }
        #endregion

        #region voice
        /// <summary>
        /// 播放语音
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isExclusive"></param>
        /// <param name="callback"></param>
        public bool PlayVoice(int key, bool isExclusive, System.Action callback)
        {
            return this.PlayVoice(key, false, m_SoundVolume, 1, callback, null);
        }

        public bool PlayVoice(int key, bool isExclusive, LuaFunction callback)
        {
            return this.PlayVoice(key, false, m_SoundVolume, 1, null, callback);
        }

        /// <summary>
        /// 播放语音
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isExclusive"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        /// <param name="callback"></param>
        protected bool PlayVoice(int key, bool isExclusive, float volume, float pitch, System.Action callback, LuaFunction luaCallback)
        {
            AudioClip clip = this.Get(key);
            if (m_IsVoiceExclusive)
            {
                if (callback != null)
                    callback();

                if (luaCallback != null)
                    luaCallback.Call();

                return false;
            }

            //发送播放完信息
            if (m_VoiceCallback != null)
            {
                System.Action onCallback = m_VoiceCallback;
                m_VoiceCallback = null;
                onCallback();
            }

            if (m_LuaCallback != null)
            {
                LuaFunction onCallback = m_LuaCallback;
                m_LuaCallback = null;
                onCallback.Call();
            }

            m_Voice.clip = clip;
            m_Voice.volume = volume;
            m_Voice.pitch = pitch;
            m_Voice.mute = m_IsMute;
            m_Voice.Play();

            //
            m_IsVoiceExclusive = isExclusive;
            m_VoiceCallback = callback;
            m_LuaCallback = luaCallback;

            return true;
        }

        /// <summary>
        /// 暂停播放语音
        /// </summary>
        public void PauseVoice()
        {
            if (m_Voice != null)
                m_Voice.Pause();
        }

        /// <summary>
        /// 停止播放语音
        /// </summary>
        public void StopVoice()
        {
            if (m_Voice != null)
                m_Voice.Stop();

            //发送播放完信息
            if (m_VoiceCallback != null)
            {
                System.Action onCallback = m_VoiceCallback;
                m_VoiceCallback = null;
                onCallback();
            }

            if (m_LuaCallback != null)
            {
                LuaFunction onCallback = m_LuaCallback;
                m_LuaCallback = null;
                onCallback.Call();
            }

            m_IsVoiceExclusive = false;
        }

        public bool CanPlayVoice()
        {
            return m_IsVoiceExclusive;
        }

        public bool IsPlayVoice()
        {
            return m_Voice != null ? m_Voice.isPlaying : false;
        }
        #endregion

        #region sound
        public void PlaySound(int key)
        {
            this.PlaySound(key, m_SoundVolume, 1.0f);
        }

        public void PlaySound(int key, float volume, float pitch)
        {
            AudioClip clip = this.Get(key);
            if (clip == null)
                return;

            //
            AudioSource target = null;
            for (int i = 0; i < m_Sounds.Count; i++)
            {
                if (!m_Sounds[i].isPlaying)
                    target = m_Sounds[i];
            }

            //
            if (target == null)
            {
                target = this.gameObject.AddComponent<AudioSource>();
                target.loop = false;
                target.priority = 128;
                m_Sounds.Add(target);
            }

            //
            target.volume = volume;
            target.pitch = pitch;
            target.mute = m_IsMute;
            target.clip = clip;
            target.Play();
        }

        public void StopSound()
        {
            for (int i = 0; i < m_Sounds.Count; i++)
                m_Sounds[i].Stop();
        }
        #endregion
    }
}
