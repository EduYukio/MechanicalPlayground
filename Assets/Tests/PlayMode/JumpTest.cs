using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using WindowsInput;
using UnityEngine.SceneManagement;

namespace Tests {
    public class JumpTest {
        bool sceneLoaded;

        public void LoadTestScene() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            sceneLoaded = true;
        }

        [UnityTest]
        public IEnumerator player_can_jump_off_the_ground() {
            // ~~~~~~~~~~
            // Load
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            // ~~~~~~~~~~

            // Prepare
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlayerFSM.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(0, 0, 0), Quaternion.identity);

            PlayerFSM playerScript = player.GetComponent<PlayerFSM>();
            playerScript.mechanics.ResetMechanics();
            playerScript.mechanics.jump = true;

            yield return new WaitForSeconds(1f);

            // Act
            var initialY = player.transform.position.y;

            Assert.IsTrue(playerScript.isGrounded);

            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);

            yield return new WaitForSeconds(0.15f);

            // Assert
            Assert.IsTrue(player.transform.position.y > initialY);
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y > 0);
        }

        [UnityTest]
        public IEnumerator player_can_not_jump_in_the_air() {
            // ~~~~~~~~~~
            // Load
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            // ~~~~~~~~~~

            // Prepare
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlayerFSM.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(0, 50, 0), Quaternion.identity);

            PlayerFSM playerScript = player.GetComponent<PlayerFSM>();
            playerScript.mechanics.ResetMechanics();
            playerScript.mechanics.jump = true;

            yield return new WaitForSeconds(0.3f);

            // Act
            var initialY = player.transform.position.y;

            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);

            yield return new WaitForSeconds(0.15f);

            // Assert
            Assert.IsFalse(playerScript.isGrounded);
            Assert.IsFalse(player.transform.position.y > initialY);
            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.y > 0);
        }

        [UnityTest]
        public IEnumerator player_can_not_jump_twice() {
            // ~~~~~~~~~~
            // Load
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            // ~~~~~~~~~~

            // Prepare
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlayerFSM.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(0, 0, 0), Quaternion.identity);

            PlayerFSM playerScript = player.GetComponent<PlayerFSM>();
            playerScript.mechanics.ResetMechanics();
            playerScript.mechanics.jump = true;

            yield return new WaitForSeconds(1f);

            // Act
            var initialY = player.transform.position.y;

            Assert.IsTrue(playerScript.isGrounded);

            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);

            yield return new WaitForSeconds(0.2f);

            var airY = player.transform.position.y;
            // Assert
            Assert.IsTrue(airY > initialY);
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y > 0);

            IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.UP);

            yield return new WaitForSeconds(0.1f);

            Assert.IsFalse(playerScript.isGrounded);
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y < 0);

            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);
            yield return new WaitForSeconds(0.1f);
            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.y > 0);

        }
    }
}