using RPG.Core;
using RPG.Quests;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] List<Audio> audios;

        int enabledIndex = -1;

        QuestList playerQuestList;

        [System.Serializable]
        public struct Audio
        {
            public int index;
            public AudioClip audioClip;
            public Condition condition;
        }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            playerQuestList = player.GetComponent<QuestList>();
        }

        private void Update()
        {
            EnableAudio();
        }

        private bool CanEnableAudio(Condition condition, QuestList questList)
        {
            return condition.Check(questList.GetComponents<IPredicateEvaluator>());
        }

        private void EnableAudio()
        {
            if (!playerQuestList) return;

            foreach (var audio in audios)
            {
                if (CanEnableAudio(audio.condition, playerQuestList) && (enabledIndex == -1 || enabledIndex != audio.index))
                {
                    GetComponent<AudioSource>().PlayOneShot(audio.audioClip);
                    enabledIndex = audio.index;
                }

                if (!CanEnableAudio(audio.condition, playerQuestList) && enabledIndex == audio.index)
                {
                    Resources.UnloadAsset(audio.audioClip);
                    GetComponent<AudioSource>().clip = null;
                    enabledIndex = -1;
                }
            }
        }
    }
}
