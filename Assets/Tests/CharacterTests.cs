using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using StarterAssets;

public class CharacterTests : InputTestFixture
{

    private GameObject character = Resources.Load<GameObject>("Character");
    private Keyboard keyboard;

    public override void Setup()
    {
        SceneManager.LoadScene("SimpleTesting");
        base.Setup();
        keyboard = InputSystem.AddDevice<Keyboard>();

        var mouse = InputSystem.AddDevice<Mouse>();
        Press(mouse.rightButton);
        Release(mouse.rightButton); ;
    }

    [Test]
    public void TestPlayerInstantiation()
    {
        GameObject characterInstance = GameObject.Instantiate(character, Vector3.zero, Quaternion.identity);
        Assert.That(characterInstance, !Is.Null);
    }

    [UnityTest]
    public IEnumerator TestPlayerMoves()
    {
        GameObject characterInstance = GameObject.Instantiate(character, Vector3.zero, Quaternion.identity);

        Press(keyboard.upArrowKey);
        yield return new WaitForSeconds(1f);
        Release(keyboard.upArrowKey);
        yield return new WaitForSeconds(1f);

        Assert.That(characterInstance.transform.GetChild(0).transform.position.z, Is.GreaterThan(1.5f));
    }

    [UnityTest]
    public IEnumerator TestPlayerFallDamage()
    {
        // spawn the character in a high enough area in the test scene
        GameObject characterInstance = GameObject.Instantiate(character, new Vector3(0f, 4f, 17.2f), Quaternion.identity);

        // Get a reference to the PlayerHealth component and assert currently at full health (1f)
        var characterHealth = characterInstance.GetComponent<PlayerHealth>();
        Assert.That(characterHealth.Health, Is.EqualTo(1f));

        // Walk off the ledge and wait for the fall
        Press(keyboard.upArrowKey);
        yield return new WaitForSeconds(0.5f);
        Release(keyboard.upArrowKey);
        yield return new WaitForSeconds(2f);

        // Assert that 1 health point was lost due to the fall damage
        Assert.That(characterHealth.Health, Is.EqualTo(0.9f));
    }

    [TearDown]
    public void Teardown()
    {
        foreach(var gameObject in GameObject.FindObjectsOfType<GameObject>())
        {
            GameObject.Destroy(gameObject);
        }

        InputSystem.RemoveDevice(keyboard);
    }
    
}
