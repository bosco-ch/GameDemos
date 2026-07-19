using DG.Tweening;
using UnityEngine;

namespace character
{
    public class Disappear : MonoBehaviour
    {
        // public Transform target;
        public new ParticleSystem particleSystem;

        private Material _material;

        // Start is called before the first frame update
        void Start()
        {
            _material = GetComponent<Renderer>().material;
            // particleSystem = GetComponent<ParticleSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Die();
            }
        }

        private void Die()
        {
            particleSystem.Play();
            _material.DOFloat(-1, "_Strength", 1.5f);
        }
    }
}