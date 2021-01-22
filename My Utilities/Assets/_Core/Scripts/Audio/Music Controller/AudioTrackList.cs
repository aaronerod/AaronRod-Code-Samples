using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rod.Utilities.MusicController
{/// <summary>
/// Contenedor de tracos usados por el controlador de música
/// </summary>
    [CreateAssetMenu(fileName = "Tracklist", menuName = "Rod/Music Controller/Tracklist")]
    public class AudioTrackList : ScriptableObject
    {
        [SerializeField]
        private List<MusicTrackData> tracks = new List<MusicTrackData>();

        /// <summary>
        /// Lista de tracks
        /// </summary>
        public List<MusicTrackData> Tracks { get => tracks; }

        /// <summary>
        /// Obtener un track por su key id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public MusicTrackData GetTrack(string key)
        {
            MusicTrackData track = Tracks.Find(x => x.Key == key);
            return track;
        }
    }
}