using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using WindowsInput;
using UnityEngine.SceneManagement;

namespace Tests {
    public class AttackTest {
        bool sceneLoaded;

        public void LoadTestScene() {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            sceneLoaded = true;
        }

        [UnityTest]
        public IEnumerator player_can_attack_enemy() {
            // ~~~~~~~~~~
            // Load
            LoadTestScene();
            yield return new WaitWhile(() => sceneLoaded == false);
            // ~~~~~~~~~~

            // Prepare
            var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlayerEmpty.prefab");
            GameObject player = GameObject.Instantiate(playerAsset, new Vector3(0, 0, 0), Quaternion.identity);

            Attack attackScript = player.AddComponent<Attack>();
            attackScript.attackPoint = GameObject.Find("AttackPoint").transform;
            attackScript.slashEffect = GameObject.Find("Circular");
            // attackScript.enemyLayers += LayerMask.NameToLayer("Enemies");
            attackScript.enemyLayers = LayerMask.GetMask("Enemies");


            var enemyAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Slime.prefab");
            GameObject enemy = GameObject.Instantiate(enemyAsset, new Vector3(1f, 0, 0), Quaternion.identity);

            yield return new WaitForSeconds(1f);

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            // Act
            Assert.IsTrue(enemyScript.currentHealth == enemyScript.maxHealth);
            // Assert.IsFalse(GameObject.Find("Circular").activeSelf);

            InputSimulator IS = new InputSimulator();
            IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_X);

            yield return null;
            Assert.IsTrue(GameObject.Find("Circular").activeSelf);

            yield return new WaitForSeconds(0.2f);

            Assert.IsTrue(enemyScript.currentHealth < enemyScript.maxHealth);
        }
    }
}
