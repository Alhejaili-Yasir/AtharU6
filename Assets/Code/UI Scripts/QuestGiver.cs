using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class QuestGiver : MonoBehaviour
{
    public int questIndex;
    public GameObject questPanel;
    public TextMeshProUGUI questText;
    public TextMeshProUGUI floatingText;

    [TextArea]
    public string idleText = "Hello there!";
    public string talkHint = "Click E to talk";

    [TextArea(2, 5)]
    public List<string> dialogueLines = new List<string>();
    public List<AudioClip> dialogueVoices = new List<AudioClip>();

    [Header("Voice Lines")]
    public AudioClip onQuestAcceptedVoice;
    public AudioClip onQuestProgressVoice;
    public AudioClip onQuestCompletedVoice;
    public AudioClip onQuestAlreadyDoneVoice;
    public AudioClip onQuestLockedVoice; // ✅ الصوت الجديد

    private AudioSource audioSource;

    private bool playerInRange = false;
    private bool questGiven = false;
    private bool hasInteracted = false;

    private int dialogueIndex = 0;
    private bool showingDialogue = false;

    void Start()
    {
        if (floatingText != null)
            floatingText.text = idleText;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (playerInRange && floatingText != null && !hasInteracted && !showingDialogue)
            floatingText.text = talkHint;

        if (!playerInRange && floatingText != null)
            floatingText.text = idleText;

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            hasInteracted = true;

            if (!questGiven && dialogueLines.Count > 0 && dialogueIndex < dialogueLines.Count
                && questIndex == QuestManager.Instance.currentQuestIndex)
            {
                if (!showingDialogue)
                    showingDialogue = true;

                questPanel.SetActive(true);
                questText.text = dialogueLines[dialogueIndex];

                if (dialogueIndex < dialogueVoices.Count && dialogueVoices[dialogueIndex] != null)
                {
                    audioSource.Stop();
                    audioSource.PlayOneShot(dialogueVoices[dialogueIndex]);
                }

                dialogueIndex++;

                if (dialogueIndex >= dialogueLines.Count)
                {
                    showingDialogue = false;
                    Quest currentQuest = QuestManager.Instance.GetCurrentQuest();
                    QuestManager.Instance.quests[questIndex].questAccepted = true;
                    questGiven = true;

                    questText.text = $"Collect {currentQuest.requiredAmount} of {currentQuest.requiredItem}.";
                    PlayVoice(onQuestAcceptedVoice);
                }

                return;
            }

            Quest currentQuestCheck = QuestManager.Instance.GetCurrentQuest();

            if (questIndex < QuestManager.Instance.currentQuestIndex)
            {
                questPanel.SetActive(true);
                questText.text = "You already completed this quest.";
                PlayVoice(onQuestAlreadyDoneVoice);
                return;
            }

            if (questIndex > QuestManager.Instance.currentQuestIndex)
            {
                questPanel.SetActive(true);

                if (questIndex - 1 >= 0 && questIndex - 1 < QuestManager.Instance.quests.Count)
                {
                    questText.text = $"Finish the previous quest: {QuestManager.Instance.quests[questIndex - 1].questName}";
                    PlayVoice(onQuestLockedVoice); // ✅ تشغيل الصوت
                }
                else
                {
                    questText.text = "This quest is not available yet.";
                    PlayVoice(onQuestLockedVoice); // ✅ تشغيل الصوت
                }

                return;
            }

            if (!questGiven)
            {
                questPanel.SetActive(true);
                questText.text = $"Collect {currentQuestCheck.requiredAmount} of {currentQuestCheck.requiredItem}.";
                questGiven = true;
                QuestManager.Instance.quests[questIndex].questAccepted = true;
                PlayVoice(onQuestAcceptedVoice);
                return;
            }

            if (!currentQuestCheck.isCompleted)
            {
                int collected = QuestManager.Instance.GetCollectedAmount(currentQuestCheck.requiredItem);
                int remaining = currentQuestCheck.requiredAmount - collected;

                questPanel.SetActive(true);

                if (remaining > 0)
                {
                    questText.text = $"You still need {remaining} more {currentQuestCheck.requiredItem}.";
                    PlayVoice(onQuestProgressVoice);
                }
                else
                {
                    currentQuestCheck.isCompleted = true;
                    QuestManager.Instance.RemoveItem(currentQuestCheck.requiredItem, currentQuestCheck.requiredAmount);
                    QuestManager.Instance.AddMoney(currentQuestCheck.rewardMoney);
                    QuestManager.Instance.MoveToNextQuest();

                    questText.text = $"Quest completed! You earned {currentQuestCheck.rewardMoney} coins.";
                    PlayVoice(onQuestCompletedVoice);
                }
            }
            else
            {
                questPanel.SetActive(true);
                questText.text = "You already completed this quest.";
                PlayVoice(onQuestAlreadyDoneVoice);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            questPanel.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            hasInteracted = false;
            dialogueIndex = 0;
            showingDialogue = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            questPanel.SetActive(false);
            hasInteracted = false;
            dialogueIndex = 0;
            showingDialogue = false;
            audioSource.Stop();
        }
    }

    void PlayVoice(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(clip);
        }
    }
}
