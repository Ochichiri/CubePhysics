using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private float _splitChance = 100f;

    public void Init(float splitChance)
    {
        ResetScale();
        SetRandomColor();
        ResetSplitChance(splitChance);
    }

    public void TryToSplit()
    {
        float minChanceValue = 0f;
        float maxChanceValue = 100f;

        float chanceValue = Random.Range(minChanceValue, maxChanceValue);

        if (_splitChance >= chanceValue)
        {
            Split();
        }
        else
        {
            Destroy();
        }
    }

    private void ResetScale()
    {
        float decreaseCoefficient = 0.5f;
        transform.localScale = transform.localScale * decreaseCoefficient; 
    }

    private void SetRandomColor()
    {
        Renderer cubeRenderer = GetComponent<Renderer>();

        Color randomColor = new Color
            (
            Random.value,
            Random.value, 
            Random.value, 
            Random.value
            );

        cubeRenderer.material.color = randomColor;
    }

    private void ResetSplitChance(float previousChance)
    {
        float decreaseCoefficient = 0.5f;
        _splitChance = previousChance * decreaseCoefficient;
    }

    private void Split()
    {
        float minAmount = 2f;
        float maxAmount = 6f;

        float positionXRange = 1f;
        float positionZRange = 1f;

        float amountOfNewCubes = Random.Range(minAmount, maxAmount);

        List<Rigidbody> _newCubes = new List<Rigidbody>();

        for (int i = 0; i < amountOfNewCubes; i++)
        {
            float positionX = Random.Range(-positionXRange, positionXRange);
            float positionZ = Random.Range(-positionZRange, positionZRange);

            Vector3 newPosition = new Vector3
                (
                transform.position.x + positionX, 
                transform.position.y, 
                transform.position.z + positionZ
                );

            Cube newCube = Instantiate(this, newPosition, Quaternion.identity);
            newCube.Init(_splitChance);
            _newCubes.Add(newCube.GetComponent<Rigidbody>());
        }

        ExplodeCurrentCubes(_newCubes);
        Destroy(gameObject);
    }

    private void Destroy()
    {
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        float explosionForce = 500f;
        float explosionRadius = 20f; 

        foreach (Rigidbody explodableObject in GetExplodableObjects(explosionRadius))
        {
            explodableObject.AddExplosionForce(explosionForce, transform.position, explosionRadius);
        }
    }

    private void ExplodeCurrentCubes(List<Rigidbody> cubes)
    {
        float scaleRatio = (1 / transform.localScale.x);

        float explosionForce = 500f;
        float explosionRadius = 20f;

        explosionForce *= scaleRatio;
        explosionRadius *= scaleRatio;

        foreach (Rigidbody explodableObject in cubes)
        {
            float differenceRatio = ((transform.position - explodableObject.position).magnitude) / explosionRadius;

            explodableObject.AddExplosionForce(explosionForce * differenceRatio, transform.position, explosionRadius);
        }
    }

    private List<Rigidbody> GetExplodableObjects(float explosionRadius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        List<Rigidbody> objects = new();

        foreach (Collider hit in hits)
        {
            if (hit.attachedRigidbody != null)
            {
                objects.Add(hit.attachedRigidbody);
            }
        }

        return objects;
    }
}