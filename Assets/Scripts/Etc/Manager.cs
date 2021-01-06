using UnityEngine;

static class Manager {
    public static AudioManager audio;

    static Manager() {
        GameObject preloadObj = SafeFindObject("PreloadObject");

        audio = (AudioManager)SafeComponentRetrieval(preloadObj, "AudioManager");
    }

    private static GameObject SafeFindObject(string objName) {
        GameObject preloadObj = GameObject.Find(objName);
        if (preloadObj == null) PrintErrors("GameObject " + objName + "  not on preload scene.");
        return preloadObj;
    }

    private static Component SafeComponentRetrieval(GameObject preloadObj, string componentName) {
        Component component = preloadObj.GetComponent(componentName);
        if (component == null) PrintErrors("Component " + componentName + " not on preload scene.");
        return component;
    }

    private static void PrintErrors(string error) {
        Debug.Log(">>> Cannot proceed... " + error);
        Debug.Log(">>> It is very likely you just forgot to launch");
        Debug.Log(">>> from scene zero, the preload scene.");
    }
}