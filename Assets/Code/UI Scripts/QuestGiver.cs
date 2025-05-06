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

            // حوار تمهيدي
            if (!questGiven && dialogueLines.Count > 0 && dialogueIndex < dialogueLines.Count)
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
                    showingDialogue = false;

                return;
            }

            Quest currentQuest = QuestManager.Instance.GetCurrentQuest();

            // المهمة منتهية سابقًا
            if (questIndex < QuestManager.Instance.currentQuestIndex)
            {
                questPanel.SetActive(true);
                questText.text = "✅ You already completed this quest.";
                PlayVoice(onQuestAlreadyDoneVoice);
                return;
            }

            // لم يتم الوصول للمهمة بعد
            if (questIndex > QuestManager.Instance.currentQuestIndex)
            {
                questPanel.SetActive(true);

                if (questIndex - 1 >= 0 && questIndex - 1 < QuestManager.Instance.quests.Count)
                {
                    questText.text = $"❌ Finish the previous quest: {QuestManager.Instance.quests[questIndex - 1].questName}";
                }
                else
                {
                    questText.text = "❌ This quest is not available yet.";
                }

                return;
            }

            // إعطاء المهمة لأول مرة
            if (!questGiven)
            {
                questPanel.SetActive(true);
                questText.text = $"📜 Quest: Collect {currentQuest.requiredAmount} of {currentQuest.requiredItem}.";
                questGiven = true;
                PlayVoice(onQuestAcceptedVoice);
                return;
            }

            // التقدم بالمهمة
            if (!currentQuest.isCompleted)
            {
                int collected = QuestManager.Instance.GetCollectedAmount(currentQuest.requiredItem);
                int remaining = currentQuest.requiredAmount - collected;

                questPanel.SetActive(true);

                if (remaining > 0)
                {
                    questText.text = $"📦 You still need {remaining} more {currentQuest.requiredItem}.";
                    PlayVoice(onQuestProgressVoice);
                }
                else
                {
                    currentQuest.isCompleted = true;
                    QuestManager.Instance.RemoveItem(currentQuest.requiredItem, currentQuest.requiredAmount);
                    QuestManager.Instance.AddMoney(currentQuest.rewardMoney);
                    QuestManager.Instance.MoveToNextQuest();

                    questText.text = $"✅ Quest completed! You earned {currentQuest.rewardMoney} coins.";
                    PlayVoice(onQuestCompletedVoice);
                }
            }
            else
            {
                questPanel.SetActive(true);
                questText.text = "✅ You already completed this quest.";
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
