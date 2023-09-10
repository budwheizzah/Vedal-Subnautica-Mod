using SCHIZO.Sounds;
using UnityEngine;

namespace SCHIZO.Creatures.Tutel
{
    public class TutelExtension : MonoBehaviour
    {
        private float playProbability = 0.15f;
        private FMOD_CustomEmitter MyEmitter;

        private void Start()
        {
            MyEmitter = GetComponent<WorldSoundPlayer>().emitter;
            TWITCH.onSubscriber += OnSubscriber;
            LOGGER.LogDebug($"{GetType().Name} Latched to Twitch Subscriber event");
        }

        private void OnSubscriber()
        {
            float myDraw = Random.Range(0f, 1f);
            if (myDraw <= playProbability)
            {
                LOGGER.LogDebug($"{GetType().Name} Subscription detected! Sound draw positive");
                TutelLoader.SubscriberSounds.Play(MyEmitter);
            }
            else
            {
                LOGGER.LogDebug($"{GetType().Name} Subscription detected! Sound draw negative");
            }
        }

        private void OnDestroy()
        {
            TWITCH.onSubscriber -= OnSubscriber;
            LOGGER.LogDebug($"{GetType().Name} Destroyed, unlatch from Twitch Subscriber event");
        }
    }
}
