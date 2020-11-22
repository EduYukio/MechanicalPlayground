using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using WindowsInput;
using UnityEngine.SceneManagement;

namespace Tests {
    public class DashTest {
        bool sceneLoaded;

        public void LoadTestScene() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            sceneLoaded = true;
        }

        [UnityTest]
        public IEnumerator player_can_dash_while_idle() {
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
            playerScript.mechanics.dash = true;

            yield return new WaitForSeconds(1f);

            // Act
            Assert.IsTrue(playerScript.isGrounded);
            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.x > 0);
            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C);

            float dashDuration = playerScript.config.startDashDurationTime;

            yield return new WaitForSeconds(dashDuration / 2);

            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.x > 0);

            yield return new WaitForSeconds(dashDuration);

            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.x > 0);
        }

        [UnityTest]
        public IEnumerator player_dash_to_the_player_facing_direction() {
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
            playerScript.mechanics.dash = true;
            playerScript.mechanics.walk = true;

            yield return new WaitForSeconds(1f);

            // Act
            Assert.IsTrue(playerScript.isGrounded);
            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.x > 0);
            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C);

            float dashDuration = playerScript.config.startDashDurationTime;

            yield return new WaitForSeconds(dashDuration / 2);

            Assert.IsTrue(playerScript.lastDirection == 1);
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.x > 0);

            yield return new WaitForSeconds(dashDuration * 1.5f);

            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.x > 0);

            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LEFT);

            yield return new WaitForSeconds(0.5f);

            IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LEFT);

            yield return null;

            IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C);

            yield return new WaitForSeconds(dashDuration / 2);

            Assert.IsTrue(playerScript.lastDirection == -1);
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.x < 0);

            yield return new WaitForSeconds(dashDuration);

            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.x < 0);
        }

        [UnityTest]
        public IEnumerator player_can_not_dash_twice() {
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
            playerScript.mechanics.dash = true;

            yield return new WaitForSeconds(1f);

            // Act
            Assert.IsTrue(playerScript.isGrounded);
            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.x > 0);
            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C);

            float dashDuration = playerScript.config.startDashDurationTime;

            yield return new WaitForSeconds(dashDuration * 0.5f);

            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.x > 0);

            IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C);

            yield return new WaitForSeconds(dashDuration * 0.6f);

            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.x > 0);
        }

        [UnityTest]
        public IEnumerator player_can_not_dash_while_on_cooldown() {
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
            playerScript.mechanics.dash = true;

            yield return new WaitForSeconds(1f);

            // Act
            Assert.IsTrue(playerScript.isGrounded);
            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.x > 0);
            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C);

            float dashDuration = playerScript.config.startDashDurationTime;

            yield return new WaitForSeconds(dashDuration * 0.5f);

            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.x > 0);

            IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C);

            yield return new WaitForSeconds(dashDuration * 0.6f);

            IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C);

            Assert.IsTrue(playerScript.dashCooldownTimer > 0);
            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.x > 0);
        }

        [UnityTest]
        public IEnumerator player_can_dash_after_cooldown() {
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
            playerScript.mechanics.dash = true;

            yield return new WaitForSeconds(1f);

            // Act
            Assert.IsTrue(playerScript.isGrounded);
            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.x > 0);
            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C);

            float dashDuration = playerScript.config.startDashDurationTime;

            yield return new WaitForSeconds(dashDuration * 1.2f);
            yield return null;

            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.x > 0);
            Assert.IsTrue(playerScript.dashCooldownTimer > 0);

            float dashCooldown = playerScript.config.startDashCooldownTime;
            yield return new WaitForSeconds(dashCooldown);

            Assert.IsFalse(player.GetComponent<Rigidbody2D>().velocity.x > 0);

            IS.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_C);

            yield return new WaitForSeconds(dashDuration * 0.5f);

            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.x > 0);
        }
    }
}
