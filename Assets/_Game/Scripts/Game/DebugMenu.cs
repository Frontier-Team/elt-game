using System.Threading.Tasks;
using _Game.Scripts.Game;
using _Game.Scripts.Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] private Slider gameSpeed;
    [SerializeField] private GameObject debugRoot;
    [SerializeField] private UIPopup mainUI;
    [SerializeField] private Slider voteChoice;
    [SerializeField] private TMP_Text voteLabel;
    [SerializeField] private Button voteButton;
    [SerializeField] private Toggle hasVotedToggle;
    [SerializeField] private Toggle forceCanVote;

    private VotingController debugVoteController;
    private bool sending;

    private void Awake()
    {
        gameSpeed.onValueChanged.AddListener(UpdateSpeed);
        debugVoteController = FindFirstObjectByType<VotingController>() ?? gameObject.AddComponent<VotingController>();

        hasVotedToggle.interactable = false;
        voteChoice.onValueChanged.AddListener(UpdateVoteIdLabel);
        forceCanVote.onValueChanged.AddListener(ForceCanVote);
        voteButton.interactable = debugVoteController.CanVote;
        hasVotedToggle.isOn = debugVoteController.HasVoted;
    }

    private void UpdateVoteIdLabel(float val)
    {
        voteLabel.text = $"Vote #{Mathf.RoundToInt(val+1)}";
    }

    private void UpdateSpeed(float speed) => Time.timeScale = speed;

    public async void SendVote()
    {
        var img = voteButton.image ?? voteButton.GetComponent<Image>();

        if (!debugVoteController.CanVote)
        {
            img.color = Color.blue;
            Debug.Log("Already voted, skipping!");
            return;
        }

        if (sending)
        {
            return;
        }
        sending = true;

        var original = img != null ? img.color : Color.white;

        if (img)
        {
            img.color = Color.yellow;
        }

        var choice = Mathf.RoundToInt(voteChoice.value);
        var ok = await debugVoteController.PostVote(choice);

        if (img)
        {
            img.color = ok ? Color.green : Color.red;
        }

        hasVotedToggle.isOn = debugVoteController.HasVoted;

        await Task.Delay(800);

        sending = false;
    }

    public void ForceCanVote(bool canVote)
    {
        if (canVote)
        {
            debugVoteController.EnableVoting();
        }
        else
        {
            debugVoteController.DisableVoting();
        }
    }

    public void OnClose() => debugRoot.SetActive(false);
    public void OnOpen() => debugRoot.SetActive(true);
    public void ShowUI() => mainUI.ShowPage();
    public void HideUI() => mainUI.HidePage();
}