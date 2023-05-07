using UnityEngine;

    public class TouchMotionUI : MonoBehaviour
    {
        Animation anim;

        private void Awake()
        {
            anim = GetComponent<Animation>();
        }

        private void OnEnable()
        {
            anim.Play();
        }

        private void Update()
        {
            if(anim.isPlaying == false)
            {
                gameObject.SetActive(false);
            }
        }
    }
