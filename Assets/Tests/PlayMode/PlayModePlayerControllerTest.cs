using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using NUnit.Framework.Constraints;
using System.Linq;

public class PlayModePlayerControllerTest : InputTestFixture
{


    // A mocked keyboard we can control from our test.
    Keyboard _keyboard;
    // A mocked mouse we can control from our test.
    // While we don't control the mouse, we need both in order
    // for the Keyboard&Mouse scheme to apply!
    Mouse _mouse;
    // We store the player so we can envoke cleanup methods in `Teardown`.
    GameObject player;


    [SetUp]
    override public void Setup()
    {
        // Let the InputTestFixture get ready
        base.Setup();
        // Add devices because After InputTestFixture is set, there are no devices.
        _keyboard = InputSystem.AddDevice<Keyboard>();
        _mouse = InputSystem.AddDevice<Mouse>();
        
        /*
        * Q: How do I create a test version of my game?
        * ----------------------------------------
        * We have the same options as when any game begins!
        * 1. We can use `SceneManager.LoadScene`
        * 2. We can [dynamically retrieve assets](https://docs.unity3d.com/Manual/LoadingResourcesatRuntime.html) from Resources directories or `AssetBundles`.
        *
        * More dynamic loading comes at a cost, because the Unity build cannot
        * prune anything stored as a `Resource` or an `AssetBundle`. So we should prefer
        * to avoid these. We will use `SceneManager.LoadScene`.
        */
        SceneManager.LoadScene("MiniGame");
    }

    [UnityTest]
    public IEnumerator PlayModePlayerControllerTestWithEnumeratorPasses()
    {
        /**
        * When you're not sure what to test, you can fall back to "Given | When | Then".

        * Given
        * ---
        * The MiniGame Scene:
        *   - A "player" GameObject sphere
        *   - A "ground" GameObject plane
        *
        * Q: How do I retieve these objects to test them?
        * ----------------------------------------
        * 1. We can find them in the scene with `GameObject.find*` static methods
        *    - [`Find`](https://docs.unity3d.com/ScriptReference/GameObject.Find.html) singular object by name.
        *    - [`FindObjectsOfType`](https://docs.unity3d.com/ScriptReference/Object.FindObjectsOfType.html) list of objects by type.
        **/

        var sceneName = SceneManager.GetActiveScene().name;
        Assert.That(sceneName, Is.EqualTo("MiniGame"));
        player = GameObject.Find("Player");
        
        /*
        * Let's check how we're wiring the inputs together.
        * Our player has a Unity provided `PlayerInput` which means
        * We "bind" different kinds of input tools (gamepad, touchscreen, keyboard, etc..) to
        * "actions" that our player then handles as events:
        Player:
            * `public void OnMove(Vector2d input) ...`
            * `public void OnLook(Vector2d input) ...`
            * `public void OnFire(bool input) ...`
        Actions (this is defined in the `InputActionAsset` in the `PlayerInput`)
            * Move
            * Look
            * Fire
        Bindings (this is defined in the `InputActionAsset` in the `PlayerInput`)
            * Move
                * /Keyboard/w, Keyboard/a, ...
            * Look
                * /Mouse/delta
            * Fire
                * /Mouse/leftButton
        
        You should see lines for these bindings logged below:
        */
        var inputActions = player.GetComponent<PlayerInput>();
        inputActions.actions.ToList().ForEach( m => Debug.Log(m));

        var initialPosition = player.transform.position;

        /**
        * When
        * ---
        * The user inputs "up"
        *
        * We can programmatically control test inputs using [`InputTestFixture`](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.InputTestFixture.html)
        **/
        Press(_keyboard.wKey);
        yield return new WaitForSeconds(2f);

        /**
        * Then
        * ---
        * The ball should roll in the positive z axis
        **/
        var newPosition = player.transform.position;
        Vector3 expectedDelta = Vector3.zero;
        var delta = newPosition - initialPosition;

        // Check out [this natural vocabulary of assert](https://dotnetpattern.com/nunit-assert-examples)!
        Assert.That(delta.x, Is.EqualTo(0));
        Assert.That(delta.z, Is.GreaterThan(0));
    }

    [TearDown]
    override public void TearDown()
    {
        // We need to "deconstruct the user" in order to have a tidy teardown.
        // I would prefer to use `SceneManager.UnloadScene("Minigame")`.
        // However that is a deprecated method and the alternative is async which
        // may not reliably complete before we get to Teardown.
        player.SetActive(false);
        base.TearDown();
    }


}
