using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using WindowsInput;
using UnityEngine.SceneManagement;

namespace Tests {
    public class DoubleJumpTest {
        bool sceneLoaded;

        public void LoadTestScene() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            sceneLoaded = true;
        }

        [UnityTest]
        public IEnumerator player_can_double_jump_off_the_ground() {
            // ~~~~~~~~~~
            // Load
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            // ~~~~~~~~~~

            // Prepare
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/PlayerFSM.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(0, 0, 0), Quaternion.identity);

            PlayerFSM playerScript = player.GetComponent<PlayerFSM>();
            playerScript.ignoreCheckpoints = true;
            playerScript.mechanics.SaveState();
            playerScript.mechanics.ResetMechanics();
            playerScript.mechanics.Activate("Jump");
            playerScript.mechanics.Activate("Double Jump");

            yield return new WaitForSeconds(1f);

            // Act
            var groundY = player.transform.position.y;

            Assert.IsTrue(playerScript.isGrounded);

            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);

            yield return new WaitForSeconds(0.2f);

            IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.UP);
            yield return new WaitForSeconds(0.05f);
            var airY = player.transform.position.y;

            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);

            yield return new WaitForSeconds(0.2f);

            var finalY = player.transform.position.y;

            // Assert
            Assert.IsFalse(playerScript.isGrounded);
            Assert.IsTrue(airY > groundY);
            Assert.IsTrue(finalY > airY);
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y > 0);
            playerScript.mechanics.RestoreState();
        }

        [UnityTest]
        public IEnumerator player_can_double_jump_while_falling() {
            // ~~~~~~~~~~
            // Load
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            // ~~~~~~~~~~

            // Prepare
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/PlayerFSM.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(0, 20, 0), Quaternion.identity);

            PlayerFSM playerScript = player.GetComponent<PlayerFSM>();
            playerScript.ignoreCheckpoints = true;
            playerScript.mechanics.SaveState();
            playerScript.mechanics.ResetMechanics();
            playerScript.mechanics.Activate("Jump");
            playerScript.mechanics.Activate("Double Jump");

            yield return new WaitForSeconds(0.5f);

            // Act
            Assert.IsFalse(playerScript.isGrounded);
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y < 0);

            var initialY = player.transform.position.y;
            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);

            yield return new WaitForSeconds(0.2f);

            var finalY = player.transform.position.y;
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y > 0);
            Assert.IsTrue(finalY > initialY);
            playerScript.mechanics.RestoreState();
        }

        [UnityTest]
        public IEnumerator player_can_not_double_jump_twice() {
            // ~~~~~~~~~~
            // Load
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            // ~~~~~~~~~~

            // Prepare
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Characters/PlayerFSM.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(0, 20, 0), Quaternion.identity);

            PlayerFSM playerScript = player.GetComponent<PlayerFSM>();
            playerScript.ignoreCheckpoints = true;
            playerScript.mechanics.SaveState();
            playerScript.mechanics.ResetMechanics();
            playerScript.mechanics.Activate("Jump");
            playerScript.mechanics.Activate("Double Jump");

            yield return new WaitForSeconds(0.5f);

            // Act
            Assert.IsFalse(playerScript.isGrounded);
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y < 0);

            var initialY = player.transform.position.y;
            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);

            yield return new WaitForSeconds(0.2f);

            var doubleJumpY = player.transform.position.y;
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y > 0);
            Assert.IsTrue(doubleJumpY > initialY);

            IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.UP);
            yield return new WaitForSeconds(0.3f);

            Assert.IsFalse(playerScript.isGrounded);
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y < 0);

            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);
            yield return new WaitForSeconds(0.2f);
            var finalY = player.transform.position.y;
            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.y > 0);
            Assert.IsFalse(finalY > doubleJumpY);
            playerScript.mechanics.RestoreState();
        }
    }
}
