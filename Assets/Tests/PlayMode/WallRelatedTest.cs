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

        public void LoadTestScene() {
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
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlayerFSM.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(-15, 0, 0), Quaternion.identity);

            player.AddComponent<WallSlide>();
            player.AddComponent<Walk>();
            player.AddComponent<Jump>();

            yield return new WaitForSeconds(0.5f);

            // Act
            Player playerScript = player.GetComponent<Player>();
            Assert.IsFalse(playerScript.isTouchingWall);
            Assert.IsFalse(playerScript.isWallSliding);

            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LEFT);
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);

            yield return new WaitForSeconds(1f);

            // Assert
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y ==
                -playerScript.GetComponent<WallSlide>().wallSlidingSpeed);
            Assert.IsTrue(playerScript.isTouchingWall);
            Assert.IsTrue(playerScript.isWallSliding);
        }

        [UnityTest]
        public IEnumerator player_can_wall_jump() {
            // ~~~~~~~~~~
            // Load
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            // ~~~~~~~~~~

            // Prepare
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlayerFSM.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(-15, 0, 0), Quaternion.identity);

            player.AddComponent<Walk>();
            player.AddComponent<Jump>();
            player.AddComponent<WallSlide>();
            player.AddComponent<WallJump>();

            yield return new WaitForSeconds(0.5f);

            // Act
            Player playerScript = player.GetComponent<Player>();

            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LEFT);
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);

            yield return new WaitForSeconds(1f);

            // Assert
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y ==
                -playerScript.GetComponent<WallSlide>().wallSlidingSpeed);
            Assert.IsTrue(playerScript.isTouchingWall);
            Assert.IsTrue(playerScript.isWallSliding);

            IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LEFT);
            IS.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.UP);

            yield return new WaitForSeconds(0.2f);

            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y < 0);
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.x <= 0);

            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);

            float wallJumpDuration = player.GetComponent<WallJump>().wallJumpTime;
            yield return new WaitForSeconds(wallJumpDuration * 0.7f);

            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.y > 0);
            Assert.IsTrue(player.GetComponent<Rigidbody2D>().velocity.x > 0);
        }
    }
}
