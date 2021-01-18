using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]
public class PianoColliderKey : MonoBehaviour
{
    public Transform visualMove;
    public AudioClip pianoKeyAudio;

    private AudioSource audioSource;
    private BoxCollider boxCollider;
    private Vector3 initLocalPosition = Vector3.zero;
    private Tween moveTween;

    private bool isPianoKeyDown = false;
    private float duration = 0.1f;

    private int enterCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!CheckIsFrontPress(other))
        {
            Debug.Log("NotFront");
            return;
        }

        if (enterCount == 0)
        {
            PianoKeyDown();
        }
        else
        {
            Debug.Log("EnterCount != 0 " + other.gameObject.name);
        }

        enterCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (enterCount == 0)
        {
            return;
        }
        
        enterCount--;

        if (enterCount == 0)
        {
            PianoKeyUp();
        }
    }

    private void Awake()
    {
        if (visualMove != null)
        {
            initLocalPosition = visualMove.transform.localPosition;
        }
        boxCollider = this.GetComponent<BoxCollider>();
        audioSource = this.GetComponent<AudioSource>();
    }

    private void PianoKeyDown()
    {
        if (isPianoKeyDown)
        {
            return;
        }

        isPianoKeyDown = true;
        if (visualMove != null)
        {
            audioSource.PlayOneShot(pianoKeyAudio);

            if (moveTween != null && moveTween.IsPlaying())
            {
                moveTween.Kill();
            }

            moveTween = visualMove.transform.DOLocalMoveZ(initLocalPosition.z + (boxCollider.size.z / 2f * 0.35f), duration);
        }
    }

    private void PianoKeyUp()
    {
        if (!isPianoKeyDown)
        {
            return;
        }

        isPianoKeyDown = false;
        if (visualMove != null)
        {
            if (moveTween != null && moveTween.IsPlaying())
            {
                moveTween.Kill();
            }

            moveTween = visualMove.transform.DOLocalMoveZ(initLocalPosition.z, duration);
        }
    }

    private bool CheckIsFrontPress(Collider collider)
    {
        Vector3 keyUp = -Vector3.forward;
        Vector3 pokeVector = transform.InverseTransformPoint(collider.transform.position) - boxCollider.center;
        float distance = Vector3.Dot(pokeVector, keyUp.normalized);

        float colliderHalfLength = boxCollider.size.z / 2f;
        return distance >= (colliderHalfLength * 0.8f);
    }
}
