// using System.Collections;
// using System.Collections.Generic;
// using NUnit.Framework;
// using UnityEngine;
// using UnityEditor;
// using UnityEngine.TestTools;
// using WindowsInput;
// using UnityEngine.SceneManagement;

// namespace Tests {
//     public class NAME {
//         bool sceneLoaded;

//         public void LoadTestScene() {
//             SceneManager.sceneLoaded += OnSceneLoaded;
//             SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
//         }

//         void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
//             sceneLoaded = true;
//         }

//         [UnityTest]
//         public IEnumerator test_name() {
//             // ~~~~~~~~~~
//             // Load
//             LoadTestScene();
//             yield return new WaitWhile(() => sceneLoaded == false);
//             // ~~~~~~~~~~

//             // Prepare
//             var playerAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PlayerEmpty.prefab");
//             GameObject player = GameObject.Instantiate(playerAsset, new Vector3(0, 0, 0), Quaternion.identity);

//             player.AddComponent<Walk>();

//             yield return null;

//             // Act

//             InputSimulator IS = new InputSimulator();
//             IS.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.RIGHT);

//             yield return new WaitForSeconds(1f);

//             // Assert
//             Assert.IsTrue(player.transform.position.x > 0);
//         }
//     }
// }
