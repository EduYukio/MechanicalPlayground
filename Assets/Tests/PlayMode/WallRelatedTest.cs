using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using WindowsInput;
using UnityEngine.SceneManagement;

namespace Tests {
    public class WallRelatedTest {
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
        public IEnumerator player_can_wall_slide() {
            // ~~~~~~~~~~
            // Load
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            // ~~~~~~~~~~

            // Prepare
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/PlayerFSM.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(-15, 0, 0), Quaternion.identity);

            PlayerFSM playerScript = player.GetComponent<PlayerFSM>();
            playerScript.ignoreCheckpoints = true;
            playerScript.ignoreCheckpoints = true;
            playerScript.mechanics.SaveState();
            playerScript.mechanics.ResetMechanics();
            playerScript.mechanics.Activate("Walk");
            playerScript.mechanics.Activate("Jump");
            playerScript.mechanics.Activate("Wall Slide");

            yield return new WaitForSeconds(0.5f);

            // Act
            Assert.IsFalse(playerScript.isTouchingWall);

            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LEFT);
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_Z);

            yield return new WaitForSeconds(1f);

            // Assert
            Assert.IsTrue(playerScript.rb.velocity.y == -playerScript.config.wallSlidingSpeed);
            Assert.IsTrue(playerScript.isTouchingWall);
            playerScript.mechanics.RestoreState();
        }

        [UnityTest]
        public IEnumerator player_can_wall_jump() {
            // ~~~~~~~~~~
            // Load
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            // ~~~~~~~~~~

            // Prepare
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/PlayerFSM.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(-15, 0, 0), Quaternion.identity);

            PlayerFSM playerScript = player.GetComponent<PlayerFSM>();
            playerScript.ignoreCheckpoints = true;
            playerScript.mechanics.SaveState();
            playerScript.mechanics.ResetMechanics();
            playerScript.mechanics.Activate("Walk");
            playerScript.mechanics.Activate("Jump");
            playerScript.mechanics.Activate("Wall Slide");
            playerScript.mechanics.Activate("Wall Jump");

            yield return new WaitForSeconds(0.5f);

            // Act
            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LEFT);
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_Z);

            yield return new WaitForSeconds(1f);

            // Assert
            Assert.IsTrue(playerScript.rb.velocity.y == -playerScript.config.wallSlidingSpeed);
            Assert.IsTrue(playerScript.isTouchingWall);

            IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LEFT);
            IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_Z);

            yield return new WaitForSeconds(0.2f);

            Assert.IsTrue(playerScript.rb.velocity.y < 0);
            Assert.IsTrue(playerScript.rb.velocity.x <= 0);

            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_Z);

            float wallJumpDuration = playerScript.config.startWallJumpDurationTime;
            yield return new WaitForSeconds(wallJumpDuration * 0.7f);

            Assert.IsTrue(playerScript.rb.velocity.y > 0);
            Assert.IsTrue(playerScript.rb.velocity.x > 0);
            playerScript.mechanics.RestoreState();
        }
    }
}
