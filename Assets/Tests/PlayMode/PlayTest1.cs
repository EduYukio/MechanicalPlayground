using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using WindowsInput;
using UnityEngine.SceneManagement;

namespace Tests {
    public class PlayTest1 {
        bool sceneLoaded;

        public void LoadTestScene() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            sceneLoaded = true;
        }

        [UnityTest]
        public IEnumerator player_can_walk_to_the_right() {
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);

            var root = new GameObject();
            root = GameObject.Instantiate(root);

            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
            var playerPrefab = GameObject.Instantiate(playerAsset, new Vector3(0, 0, 0), Quaternion.identity);

            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.RIGHT);

            yield return new WaitForSeconds(2f);

            Assert.IsTrue(playerPrefab.transform.position.x > 0, "Player is not walking to the right.");
        }

        [UnityTest]
        public IEnumerator player_can_walk_to_the_left() {
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            var root = new GameObject();
            root = GameObject.Instantiate(root);

            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
            var playerPrefab = GameObject.Instantiate(playerAsset, new Vector3(0, 0, 0), Quaternion.identity);

            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LEFT);

            yield return new WaitForSeconds(2f);

            Assert.IsTrue(playerPrefab.transform.position.x < 0, "Player is not walking to the right.");
        }
    }
}
