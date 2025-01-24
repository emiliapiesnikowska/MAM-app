using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed;

    private Animator animator;
    private Quaternion targetRotation; // Docelowy obrót

    private int desiredLane = 1;
    public float laneDistance = 4; //odlegloœæ pomiedzy 2 liniami 
    
    public float jumpForce;
    public float rotationSpeed = 5f;
    private bool isRotating = false; // Flaga blokuj¹ca zmianê pasa podczas obrotu
    private bool canJump = true; // Flaga umo¿liwiaj¹ca skakanie
    public float gravity = -20;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        targetRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Ruch postaci w przód
        direction.z = forwardSpeed;

        // Grawitacja
        if (!controller.isGrounded)
        {
            direction.y += gravity * Time.deltaTime;
        }

        // Skok (odblokowany po obrocie i gdy postaæ jest na ziemi)
        if (IsGrounded() && canJump && (Input.GetKeyDown(KeyCode.UpArrow) || SwipeManager.swipeUp))
        {
            Jump();
        }

        // Zmiana pasa (blokowana podczas obrotu)
        if (!isRotating)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) || SwipeManager.swipeRight)
            {
                if (desiredLane < 2) // Maksymalny pas to 2
                {
                    desiredLane++;
                    StartCoroutine(RotatePlayer(90)); // Obrót w prawo
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || SwipeManager.swipeLeft)
            {
                if (desiredLane > 0) // Minimalny pas to 0
                {
                    desiredLane--;
                    StartCoroutine(RotatePlayer(-90)); // Obrót w lewo
                }
            }
        }

        // Obliczenie pozycji docelowej na podstawie pasa
        Vector3 targetPosition = transform.position.z * Vector3.forward + transform.position.y * Vector3.up;
        if (desiredLane == 0) targetPosition += Vector3.left * laneDistance;
        if (desiredLane == 2) targetPosition += Vector3.right * laneDistance;

        // Ruch postaci w kierunku docelowym
        Vector3 moveDir = (targetPosition - transform.position).normalized * 25 * Time.deltaTime;
        if (moveDir.sqrMagnitude < (targetPosition - transform.position).sqrMagnitude)
        {
            controller.Move(moveDir);
        }
        else
        {
            controller.Move(targetPosition - transform.position);
        }

        // Aktualizacja obrotu postaci
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void Jump()
    {
        direction.y = jumpForce;
        animator.SetTrigger("Jump");
    }

    private IEnumerator RotatePlayer(float angle)
    {
        isRotating = true; // Blokada zmiany pasa
        canJump = false; // Tymczasowa blokada skoku
        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + angle, 0);

        // Trwa obrót przez okreœlony czas
        float elapsed = 0f;
        float rotationDuration = 0.15f; // D³ugoœæ obrotu
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Powrót do patrzenia na wprost
        targetRotation = Quaternion.Euler(0, 0, 0);
        isRotating = false; // Odblokowanie zmiany pasa

        canJump = true; // Skok odblokowany tylko, gdy postaæ jest na ziemi
    }

    private IEnumerator ForceJumpUnlock()
    {
        // Dodatkowe sprawdzenie w razie b³êdów z `isGrounded`
        yield return new WaitForSeconds(0.04f); // Krótkie opóŸnienie
        canJump = true;
        Debug.Log("Skok odblokowany wymuszeniem!");
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.isGameStarted) return;

        // Ruch postaci w przód
        controller.Move(direction * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Obstacle"))
        {
            PlayerManager.gameOver = true;
        }
    }
    private bool IsGrounded()
    {
        return controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}