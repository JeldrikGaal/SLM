using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkPooling : MonoBehaviour
{

    [SerializeField] private List<GameObject> _sparks = new List<GameObject>();
    private List<ParticleSystem> _sparkParticles = new List<ParticleSystem>();

    private int _currentSpark;
    private Vector3 _startPos;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;

        foreach(GameObject g in _sparks)
        {
            _sparkParticles.Add(g.GetComponent<ParticleSystem>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnSpark (Vector3 position, float scale)
    {
        _sparks[_currentSpark].transform.position = new Vector3(position.x, position.y, -0.5f);
        _sparks[_currentSpark].transform.localScale = new Vector3(scale, scale, scale);

        _sparkParticles[_currentSpark].Play();

        _currentSpark++;
        if (_currentSpark == _sparks.Count)
        {
            _currentSpark = 0;
        }
        

    }

    private IEnumerator RemoveSpark(int index)
    {
        yield return new WaitForSeconds(0.2f);
        _sparks[index].transform.position = _startPos;
    }
}
