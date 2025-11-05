using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonoGameLibrary.GameCore
{
    /// <summary>
    /// 音频管理器，负责管理游戏中的音效和背景音乐
    /// </summary>
    public class AudioManager
    {
        private Dictionary<string, SoundEffect> _soundEffects;
        private Dictionary<string, Song> _songs;
        private float _masterVolume;
        private float _sfxVolume;
        private float _musicVolume;

        public AudioManager()
        {
            _soundEffects = new Dictionary<string, SoundEffect>();
            _songs = new Dictionary<string, Song>();
            _masterVolume = 1.0f;
            _sfxVolume = 1.0f;
            _musicVolume = 1.0f;
        }

        /// <summary>
        /// 加载音效
        /// </summary>
        public void LoadSoundEffect(string name, SoundEffect soundEffect)
        {
            _soundEffects[name] = soundEffect;
        }

        /// <summary>
        /// 加载背景音乐
        /// </summary>
        public void LoadSong(string name, Song song)
        {
            _songs[name] = song;
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        public void PlaySoundEffect(string name, float volume = 1.0f)
        {
            if (_soundEffects.ContainsKey(name))
            {
                _soundEffects[name].Play(_masterVolume * _sfxVolume * volume, 0.0f, 0.0f);
            }
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public void PlaySong(string name, bool isRepeating = true)
        {
            if (_songs.ContainsKey(name))
            {
                MediaPlayer.Volume = _masterVolume * _musicVolume;
                MediaPlayer.IsRepeating = isRepeating;
                MediaPlayer.Play(_songs[name]);
            }
        }

        /// <summary>
        /// 停止背景音乐
        /// </summary>
        public void StopMusic()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// 设置主音量
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            _masterVolume = MathHelper.Clamp(volume, 0.0f, 1.0f);
            MediaPlayer.Volume = _masterVolume * _musicVolume;
        }

        /// <summary>
        /// 设置音效音量
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            _sfxVolume = MathHelper.Clamp(volume, 0.0f, 1.0f);
        }

        /// <summary>
        /// 设置音乐音量
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            _musicVolume = MathHelper.Clamp(volume, 0.0f, 1.0f);
            MediaPlayer.Volume = _masterVolume * _musicVolume;
        }
    }
}
