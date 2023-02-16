using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Compass : MonoBehaviour
{
    #region Members
    [SerializeField] private RawImage spaceShipIndicator;
    [SerializeField] private RawImage questGiverIndicator;
    [SerializeField] private RawImage followUpQuestIndicator;
    [SerializeField] private Transform spaceShip;
    private Transform player;
    private GameObject questGiver;
    private GameObject followUpQuestTarget;
    private QuestManager questManager;

    public GameObject QuestGiver { get => questGiver; set => questGiver = value; }
    public GameObject FollowUpQuestTarget { get => followUpQuestTarget; set => followUpQuestTarget = value; }
    public RawImage QuestGiverIndicator { get => questGiverIndicator; set => questGiverIndicator = value; }
    #endregion

    #region Unity Methods
    private void Start()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

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
            CalculateFollowQuestTarget();
        }
    }
    #endregion

    #region Public Methods
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
    #endregion

    #region Private Methods
    private void CalculateQuestGiver()
    {
        if (questGiver != null)
        {
            float angle = AngleDifferenceLimitedToYAxis(player.transform, questGiver.transform);
            QuestGiverIndicator.rectTransform.localPosition = new Vector3(angle * 5f, 0, 0);

            // Set on or off when not visible in compass
            if (angle * 5 > -179 && angle * 5 < 179)
            {
                QuestGiverIndicator.GetComponent<RawImage>().enabled = true;
            }
            else
            {
                QuestGiverIndicator.GetComponent<RawImage>().enabled = false;
            }
        }
        else
        {
            QuestGiverIndicator.GetComponent<RawImage>().enabled = false;
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
    private void CalculateFollowQuestTarget()
    {
        if (followUpQuestTarget)
        {
            float angle = AngleDifferenceLimitedToYAxis(player.transform, followUpQuestTarget.transform);
            followUpQuestIndicator.rectTransform.localPosition = new Vector3(angle * 5f, 0, 0);

            if (questManager.ConfrontationQuest.Active)
            {
                // Set on or off when not visible in compass
                if (angle * 5 > -179 && angle * 5 < 179)
                {
                    followUpQuestIndicator.GetComponent<RawImage>().enabled = true;
                }
                else
                {
                    followUpQuestIndicator.GetComponent<RawImage>().enabled = false;
                }
            }
            else
            {
                
            }
        }
        else
        {
            followUpQuestIndicator.GetComponent<RawImage>().enabled = false;
        }
    }
    #endregion
}
