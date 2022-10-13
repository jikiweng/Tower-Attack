using UnityEngine;

namespace TowerAttack.Title
{
    //Attach to main camera.
    public class PersistentObjectSpawner : MonoBehaviour
    {
        //includes fader and audio sources.
        [SerializeField] GameObject persistentObjectPrefab;
        //after spawn for the first time, this bool will be false forever,
        static bool hasSpawned = false;

        private void Awake()
        {
            //spawn the prefab if it has not been spawned yet.
            if (hasSpawned) return;
            {
                GameObject persistentObject = Instantiate(persistentObjectPrefab);
                //all the object insides will be brought to the new scene.
                DontDestroyOnLoad(persistentObject);
            }
            hasSpawned = true;
        }
    }
}