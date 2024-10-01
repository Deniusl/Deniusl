using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreeSpawner : MonoBehaviour
{
    [Header("Tree Prefabs")]
    public GameObject[] treePrefabs; // Array of tree prefabs

    [Header("Spawn Settings")]
    public int numberOfTrees = 10; // Number of trees to spawn
    public float spawnRadius = 50f; // Radius within which trees will spawn
    public float clearRadius = 10f; // Radius around the player tree where no other trees will spawn
    public float minimumTreeDistance = 2f; // Minimum distance between trees
    public Transform playerTree; // Reference to the player's tree
    public float planeYPosition = 0f; // Y position of the plane

    [Header("Rotation Settings")]
    public Vector3 rotationAngles = Vector3.zero; // Rotation angles to apply (if not zero)

    [Header("Testing Settings")]
    public bool autoGenerateTMP = true; // Automatically generate TextMeshPro parameters
    public int minWateringDays = 1;
    public int maxWateringDays = 21;
    public string[] walletNumbers = { "wallet1", "wallet2", "wallet3" }; // Example wallet numbers

    private List<Vector3> treePositions = new List<Vector3>(); // List of tree positions

    void Start()
    {
        SpawnTrees();
    }

    void SpawnTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            // Randomly select a tree prefab
            GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];

            // Generate a random position
            Vector3 position = GenerateRandomPosition();

            if (position == Vector3.zero)
            {
                Debug.LogWarning("Failed to find a valid position for tree " + i);
                continue;
            }

            // Instantiate the tree
            GameObject treeInstance = Instantiate(treePrefab, position, Quaternion.identity);

            // Apply specified rotation angles
            ApplyRotation(treeInstance);

            // Get the TreeGrowthAnimationController component
            TreeGrowthAnimationController growthController = treeInstance.GetComponent<TreeGrowthAnimationController>();

            if (growthController == null)
            {
                Debug.LogError("Tree prefab does not have a TreeGrowthAnimationController component.");
                continue;
            }

            int wateringDays = 0;
            string walletNumber = "";

            if (autoGenerateTMP)
            {
                // Randomly generate watering days and wallet number
                wateringDays = Random.Range(minWateringDays, maxWateringDays + 1);
                walletNumber = walletNumbers[Random.Range(0, walletNumbers.Length)];
            }
            else
            {
                // Obtain data from the server
                wateringDays = 1; // Replace with server data
                walletNumber = "default_wallet"; // Replace with server data
            }

            // Set currentWateringDays in TreeGrowthAnimationController
            growthController.currentWateringDays = wateringDays;

            // Update the TextMeshPro component
            TextMeshPro tmp = treeInstance.GetComponentInChildren<TextMeshPro>();

            if (tmp != null)
            {
                tmp.text = wateringDays + " days (" + walletNumber + ")";

                // Make the text face the camera
                RotateTextToFaceCamera(tmp);
            }
            else
            {
                Debug.LogError("Tree prefab does not have a TextMeshPro component.");
            }

            // Add the tree's position to the list
            treePositions.Add(position);
        }
    }

    void ApplyRotation(GameObject treeInstance)
    {
        // Get the current rotation of the tree
        Vector3 currentRotation = treeInstance.transform.eulerAngles;

        // Check each axis and apply the rotation if the value is not zero
        if (rotationAngles.x != 0)
        {
            currentRotation.x = rotationAngles.x;
        }

        if (rotationAngles.y != 0)
        {
            currentRotation.y = rotationAngles.y;
        }

        if (rotationAngles.z != 0)
        {
            currentRotation.z = rotationAngles.z;
        }

        // Apply the new rotation to the tree
        treeInstance.transform.eulerAngles = currentRotation;
    }

    void RotateTextToFaceCamera(TextMeshPro tmp)
    {
        // Get the camera's transform
        Transform cameraTransform = Camera.main.transform;

        // Calculate direction from text to camera
        Vector3 directionToCamera = cameraTransform.position - tmp.transform.position;

        // Optionally, keep the text upright by zeroing the Y component
        directionToCamera.y = 0;

        // If you want the text to face the camera directly without any tilt, comment out the line above

        // Set the rotation of the text to face the camera
        tmp.transform.rotation = Quaternion.LookRotation(directionToCamera);

        // Adjust the rotation if the text appears backward
        tmp.transform.Rotate(0, 180f, 0); // Flip the text if necessary
    }

    Vector3 GenerateRandomPosition()
    {
        int maxAttempts = 100; // Limit of attempts
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            attempts++;

            // Generate a random point within the spawn radius
            Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
            Vector3 position = playerTree.position + new Vector3(randomPoint.x, 0f, randomPoint.y);
            position.y = planeYPosition; // Set the Y position

            // Check the distance from the player's tree
            if (Vector3.Distance(position, playerTree.position) < clearRadius)
            {
                continue;
            }

            // Check for overlaps with other trees
            bool overlaps = false;

            foreach (Vector3 existingPos in treePositions)
            {
                if (Vector3.Distance(position, existingPos) < minimumTreeDistance)
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                return position;
            }
        }

        // If a valid position was not found
        return Vector3.zero;
    }
}
