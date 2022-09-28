using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using WindowsInput;
using UnityEngine.SceneManagement;

namespace Tests {
    public class WalkTest {
        bool sceneLoaded;

        public void PreloadIfNeeded() {
            if (GameObject.Find("PreloadObject") != null) return;
            SceneManager.LoadScene("Preload", LoadSceneMode.Single);
        }

        public void LoadTestScene() {
            PreloadIfNeeded();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            sceneLoaded = true;
        }

        [UnityTest]
        public IEnumerator player_can_walk_to_the_right() {
            // ~~~~~~~~~~
            // Load
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            // ~~~~~~~~~~

            // Prepare
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/PlayerFSM.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(0, 0, 0), Quaternion.identity);

            PlayerFSM playerScript = player.GetComponent<PlayerFSM>();
            playerScript.IgnoreCheckpoints = true;
            playerScript.mechanics.SaveState();
            playerScript.mechanics.ResetMechanics();
            playerScript.mechanics.Activate("Walk");

            yield return new WaitForSeconds(0.3f);

            // Act
            var initialX = player.transform.position.x;

            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.RIGHT);

            yield return new WaitForSeconds(1f);

            // Assert
            Assert.IsTrue(player.transform.position.x > initialX);
            playerScript.mechanics.RestoreState();
        }

        [UnityTest]
        public IEnumerator player_can_walk_to_the_left() {
            // ~~~~~~~~~~
            // Load
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            // ~~~~~~~~~~

            // Prepare
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/PlayerFSM.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(0, 0, 0), Quaternion.identity);

            PlayerFSM playerScript = player.GetComponent<PlayerFSM>();
            playerScript.IgnoreCheckpoints = true;
            playerScript.mechanics.SaveState();
            playerScript.mechanics.ResetMechanics();
            playerScript.mechanics.Activate("Walk");

            yield return new WaitForSeconds(0.3f);

            // Act
            var initialX = player.transform.position.x;

            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LEFT);

            yield return new WaitForSeconds(1f);

            // Assert
            Assert.IsTrue(player.transform.position.x < initialX);
            playerScript.mechanics.RestoreState();
        }
    }
}
