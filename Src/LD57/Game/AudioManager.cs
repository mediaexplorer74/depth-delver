
// Type: LD57.AudioManager
// Assembly: LD57, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA033186-302C-4CE9-B79A-BD6D93232982
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

#nullable disable
namespace LD57
{
    internal static class AudioManager
    {
        public static Dictionary<string, Song> songs = new Dictionary<string, Song>();
        public static Dictionary<string, SoundEffect> sfx = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, List<SoundEffectInstance>> sfxInstances = new Dictionary<string, List<SoundEffectInstance>>();
        private const int kMaxSounds = 20;
        private static string s_curSong = "";
        private static float s_songVolume = 1f;
        private static float s_sfxVolume = 0.3f;
        private static float s_masterVolume = 0.3f;
        private static float s_curSongVolume = 1f;
        private static bool s_justSetSong = false;
        private static bool s_soundPaused = false;
        private static float s_songVolumeModifier = 1f;
        private static float s_songDipTime = 0.0f;
        private static float s_songDipVol = 0.3f;
        private static string s_queueName = "";
        private static bool s_queueLoop = true;
        private static float s_queueVolume = 1f;
        private static bool s_queueSong = false;

        public static void SetUpLooper()
        {
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        public static void AddSong(string name, Song song) => songs[name] = song;

        public static void QueueSong(string name, bool loop = false, float volume = 1f)
        {
            s_queueName = name;
            s_queueLoop = loop;
            s_queueVolume = volume;
            s_queueSong = true;
        }

        public static void PlaySong(string name, bool loop = false, float volume = 1f)
        {
            if (s_curSong == name && (MediaPlayer.State == MediaState.Playing || MediaPlayer.State == MediaState.Paused))
                return;
            s_curSongVolume = volume;
            volume *= s_songVolume * s_masterVolume;
            StopSong();
            MediaPlayer.IsRepeating = loop && !songs.ContainsKey(name + " Intro");
            MediaPlayer.Volume = volume;
            s_curSong = name;
            s_justSetSong = true;
            MediaPlayer.Play(songs[name]);
        }

        public static void StopSong()
        {
            s_curSong = "";
            MediaPlayer.Stop();
        }

        private static void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            if (s_soundPaused)
                return;
            if (!s_justSetSong && songs.ContainsKey(s_curSong + " Loop"))
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(songs[s_curSong + " Loop"]);
            }
            s_justSetSong = false;
        }

        public static bool IsSongPlaying() => MediaPlayer.State == MediaState.Playing;

        public static void AddSFX(string name, SoundEffect sound)
        {
            sfx[name] = sound;
            sfxInstances[name] = new List<SoundEffectInstance>();
            for (int index = 0; index < 20; ++index)
                sfxInstances[name].Add(sfx[name].CreateInstance());
        }

        public static void PlaySFX(
          string name,
          float volume = 1f,
          float pitch = 0.0f,
          float pan = 0.0f,
          bool jingle = false,
          bool song = false)
        {
            if (!sfx.ContainsKey(name))
                return;
            volume *= (song ? s_songVolume : s_sfxVolume) * s_masterVolume;
            SoundEffectInstance soundEffectInstance = sfxInstances[name][0];
            sfxInstances[name].RemoveAt(0);
            sfxInstances[name].Add(soundEffectInstance);
            soundEffectInstance.Volume = volume;
            soundEffectInstance.Pitch = pitch;
            soundEffectInstance.Pan = pan;
            soundEffectInstance.Play();
            if (!jingle)
                return;
            s_songDipTime = (float)(int)sfx[name].Duration.TotalSeconds;
        }

        public static void SetSongVolume(float volume)
        {
            if (volume < 0.0f)
                volume = 0.0f;
            else if (volume > 1.0f)
                volume = 1f;
            s_songVolume = volume;
            UpdateSongVolume();
        }

        private static void UpdateSongVolume()
        {
            MediaPlayer.Volume = s_songVolume * s_masterVolume * s_songVolumeModifier;
        }

        public static float GetSongVolume() => s_songVolume;

        public static void SetSFXVolume(float volume)
        {
            if (volume < 0.0f)
                volume = 0.0f;
            else if (volume > 1.0f)
                volume = 1f;
            s_sfxVolume = volume;
        }

        public static float GetSFXVolume() => s_sfxVolume;

        public static void SetMasterVolume(float volume)
        {
            if (volume < 0.0f)
                volume = 0.0f;
            else if (volume > 1.0f)
                volume = 1f;
            s_masterVolume = volume;
            MediaPlayer.Volume = s_songVolume * s_masterVolume;
        }

        public static float GetMasterVolume() => s_masterVolume;

        public static void PauseSound()
        {
            if (s_soundPaused)
                return;
            s_soundPaused = true;
            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Pause();
            foreach (KeyValuePair<string, List<SoundEffectInstance>> sfxInstance in sfxInstances)
            {
                foreach (SoundEffectInstance soundEffectInstance in sfxInstance.Value)
                {
                    if (soundEffectInstance.State == SoundState.Playing)
                        soundEffectInstance.Pause();
                }
            }
        }

        public static void UnpauseSound()
        {
            if (!s_soundPaused)
                return;
            if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();
            foreach (KeyValuePair<string, List<SoundEffectInstance>> sfxInstance in sfxInstances)
            {
                foreach (SoundEffectInstance soundEffectInstance in sfxInstance.Value)
                {
                    if (soundEffectInstance.State == SoundState.Paused)
                        soundEffectInstance.Resume();
                }
            }
            s_soundPaused = false;
        }

        public static void Update(GameTime gameTime)
        {
            if (s_soundPaused)
                return;
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (s_songDipTime > 0.0f)
            {
                s_songDipTime -= totalSeconds;
                if (s_songVolumeModifier > s_songDipVol)
                {
                    s_songVolumeModifier -= 0.05f * (totalSeconds * 60.0f);
                    if (s_songVolumeModifier < s_songDipVol)
                        s_songVolumeModifier = s_songDipVol;
                    UpdateSongVolume();
                }
            }
            else if (s_songVolumeModifier < 1.0f)
            {
                s_songVolumeModifier += 0.1f * (totalSeconds * 60.0f);
                if (s_songVolumeModifier > 1.0f)
                    s_songVolumeModifier = 1f;
                UpdateSongVolume();
            }
            if (!s_queueSong)
                return;
            PlaySong(s_queueName, s_queueLoop, s_queueVolume);
            s_queueSong = false;
        }
    }
}
