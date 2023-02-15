using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Compass : MonoBehaviour
{
    [SerializeField] private RawImage spaceShipIndicator;
    [SerializeField] private RawImage questGiverIndicator;
    private Transform player;
    [SerializeField] private Transform spaceShip;

    private GameObject questGiver;

    public GameObject QuestGiver { get => questGiver; set => questGiver = value; }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            CalculateSpaceShip();
            CalculateQuestGiver();
        }
    }

    private void CalculateQuestGiver()
    {
        if (questGiver != null)
        {
            float angle = AngleDifferenceLimitedToYAxis(player.transform, questGiver.transform);
            questGiverIndicator.rectTransform.localPosition = new Vector3(angle * 5f, 0, 0);

            // Set on or off when not visible in compass
            if (angle * 5 > -179 && angle * 5 < 179)
            {
                questGiverIndicator.GetComponent<RawImage>().enabled = true;
            }
            else
            {
                questGiverIndicator.GetComponent<RawImage>().enabled = false;
            }
        }
        else
        {
            questGiverIndicator.GetComponent<RawImage>().enabled = false;
        }
    }

    private void CalculateSpaceShip()
    {
        float angle = AngleDifferenceLimitedToYAxis(player.transform, spaceShip);
        spaceShipIndicator.rectTransform.localPosition = new Vector3(angle * 5f, 0, 0);

        // Set on or off when not visible in compass
        if (angle * 5 > -179 && angle * 5 < 179)
        {
            spaceShipIndicator.GetComponent<RawImage>().enabled = true;
        }
        else
        {
            spaceShipIndicator.GetComponent<RawImage>().enabled = false;
        }
    }

    public float AngleDifferenceLimitedToYAxis(Transform player, Transform spaceship)
    {
        Vector3 playerForward = player.forward;
        Vector3 directionToSpaceship = (spaceship.position - player.position).normalized;

        // Project the directionToSpaceship onto the plane that is perpendicular to the player's Y-axis
        Vector3 projectedDirection = Vector3.ProjectOnPlane(directionToSpaceship, player.up);

        // Calculate the angle between the player's forward direction and the projected direction
        float angle = Vector3.SignedAngle(playerForward, projectedDirection, player.up);

        return angle;
    }
}
